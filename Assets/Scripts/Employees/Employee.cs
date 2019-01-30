using System.Collections;
using System.Collections.Generic;
using Pathfinding;
using UE.Events;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Tilemaps;
using Utils;
using Wth.ModApi.Employees;

/// <summary>
/// This class represents an employee in the Game. All data is saved in the EmployeeData Scriptable Object.
/// All employee related logic is implemented here.
/// </summary>
public class Employee : MonoBehaviour {

    public const float minPathUpdateTime = .2f;
    public const float walkingSpeed = 1.0f;
    public const float regularAnimationSpeed = 1.0f;
    public const float idleAnimationSpeed = 0.75f;

    public UnityEvent stateEvent = new UnityEvent();

    public EmployeeData EmployeeData;
    private EmployeeFactory factory;
    private ContentHub contentHub;
    private GameObject employeeLayer;
    private SpriteRenderer spriteRenderer;
    protected internal Animator animator;
    private AnimationClip[] clipsMale, clipsFemale;
    private AGrid grid;
    private Vector2Event tileBlockedEvent;
    private UnityAction<Vector2> tileBlockedAction;
    private BoxCollider2D collider;

    private Tilemap tilemap;

    //Properties for the current employeeState
    private bool walking = false;
    private bool idle = true;

    private GameObject EmployeeShadow;
    private EmployeeShadow shadow;
    
    public bool Walking
    {
        get => walking;
        private set { walking = value; stateEvent.Invoke(); }
    }
    public bool Idle
    {
        get => idle;
        private set { idle = value; stateEvent.Invoke(); }
    }

    private List<Node> path;
    private static readonly int walkingProperty = Animator.StringToHash("walking");
    private static readonly int idleProperty = Animator.StringToHash("idle");

    public void Awake()
    {
        contentHub = ContentHub.Instance;
        clipsMale = contentHub.maleAnimationClips;
        clipsFemale = contentHub.femaleAnimationClips;

        tileBlockedAction = OnTileBlocked;
        tileBlockedEvent = ContentHub.Instance.TileBlockedEvent;
        tileBlockedEvent.AddListener(tileBlockedAction);
    }

    void Start () {
        employeeLayer = GameObject.FindWithTag("EmployeeLayer");
        this.grid = GameObject.FindWithTag("Pathfinding").GetComponent<AGrid>();
        tilemap = GameObject.FindWithTag("Tilemap").GetComponent<Tilemap>();
        gameObject.transform.parent = employeeLayer.transform;
        var tile = grid.getRandomFreeNode();

        gameObject.transform.position = tilemap.GetCellCenterWorld(new Vector3Int(tile.gridX, tile.gridY, 0));
        
        IdleWalking(true);
    }

    /// <summary>
    /// This needs to be called before the employee is used.
    /// Fills the object with specific data.
    /// </summary>
    /// <param name="employeeData">Data for this employee.</param>
    public void init(EmployeeData employeeData)
    {
        this.factory = new EmployeeFactory();
        this.EmployeeData = employeeData;
        spriteRenderer = gameObject.AddComponent<SpriteRenderer>();

        EmployeeShadow = new GameObject("EmployeeShadow");
        shadow = EmployeeShadow.AddComponent<EmployeeShadow>();

        // place the right animations.
        this.animator = gameObject.AddComponent<Animator>();
        RuntimeAnimatorController run = ContentHub.Instance.EmployeeAnimations;
        var animatorOverrideController = new AnimatorOverrideController(run);

        AnimationClip idle_anim;
        AnimationClip walking_anim;
        AnimationClip working_anim;

        if(employeeData.EmployeeDefinition != null)
        {
            Debug.Log("This is a special Employee");
            //special employee
            idle_anim = employeeData.EmployeeDefinition.IdleAnimation;
            walking_anim = employeeData.EmployeeDefinition.WalkingAnimation;
            working_anim = employeeData.EmployeeDefinition.WorkingAnimation;
        } else
        {
            //generated Employee. Animation needs to be set.
            var anims = (employeeData.generatedData.gender == "female") ? clipsFemale : clipsMale;
            idle_anim = anims[employeeData.generatedData.idleClipIndex];
            walking_anim = anims[employeeData.generatedData.walkingClipIndex];
            working_anim = anims[employeeData.generatedData.workingClipIndex];
            // Add the Material.
            spriteRenderer.material = factory.GenerateMaterialForEmployee(employeeData.generatedData);
        }

        idle_anim.events = SetAnimationEventFunction(idle_anim.events);
        walking_anim.events = SetAnimationEventFunction(walking_anim.events);
        working_anim.events = SetAnimationEventFunction(working_anim.events);
        animatorOverrideController["dummy_idle"] = idle_anim;
        animatorOverrideController["dummy_walking"] = walking_anim;
        animatorOverrideController["dummy_working"] = working_anim;
        this.animator.runtimeAnimatorController = animatorOverrideController;
        SetBoxCollider();
    }

    private void SetBoxCollider()
    {
        collider = gameObject.AddComponent<BoxCollider2D>();
        collider.offset = new Vector2(-0.001f, 0.571f);
        collider.size = new Vector2(0.638f, 1.381f);
    }

    public AnimationEvent[] SetAnimationEventFunction(AnimationEvent[] events)
    {
        for (var index = 0; index < events.Length; index++)
        {
            if (events[index].functionName == "ShadowEvent")
            {
                events[index].functionName = "SetSpriteThroughScript";
            }
        }

        var ret = events;
        return ret;

    }

    private void SetSpriteThroughScript(Object sprite) => shadow.SetSpriteThroughObject(sprite);

    /// <summary>
    /// Start/Stop idle walking.
    /// </summary>
    /// <param name="start"></param>
    public void IdleWalking(bool start)
    {
        Idle = start;
        if (!start & Walking)
        {
            Walking = false;
        }
    }

    private void RequestNewIdleWalk()
    {
        var callback = new System.Action<List<Node>, bool>(PathFound);
        var position = gameObject.transform.position;
        var go = this.grid.getNode(position).gridPosition;
        var end = this.grid.getRandomFreeNode().gridPosition;
        Pathfinding.PathRequestManager.RequestPath(new Pathfinding.PathRequest(go, end, callback));
        Debug.unityLogger.Log(LogType.Log, "Start: " + go.x.ToString() + ":" + go.y.ToString());
        Debug.unityLogger.Log(LogType.Log, "End: " + end.x.ToString() + ":" + end.y.ToString());
    }

    /// <summary>
    /// Follow the calculated path.
    /// </summary>
    /// <returns></returns>
    IEnumerator FollowPath() {
        bool followingPath = true;
        int pathIndex = 0;
        this.Walking = true;
        this.animator.SetTrigger(walkingProperty);
        this.animator.speed = regularAnimationSpeed;

        while (followingPath & Walking)
        {
            var stepLength = walkingSpeed * Time.deltaTime;
            var step = (path[pathIndex].worldPosition - transform.position).normalized * stepLength;
            var distance = Vector3.Distance(transform.position, path[pathIndex].worldPosition);

            while (stepLength > distance & followingPath)
            {
                if (pathIndex == path.Count - 1)
                {
                    transform.position = path[pathIndex].worldPosition;
                    followingPath = false;
                }
                else
                {
                    step = (path[pathIndex].worldPosition - transform.position).normalized * distance;
                    transform.Translate(step);
                    stepLength -= distance;
                    pathIndex++;
                    distance = Vector3.Distance(transform.position, path[pathIndex].worldPosition);
                    step = (path[pathIndex].worldPosition - transform.position).normalized * stepLength;
                }
            }

            if (followingPath)
            {
                if (step.x < 0) {
                    spriteRenderer.flipX = false;
                }
                else if (step.x > 0) {
                    spriteRenderer.flipX = true;
                }

                var oldPos = transform.position;
                transform.Translate(step);
                // set the sorting order
                spriteRenderer.sortingOrder = grid.CalculateSortingLayer(oldPos + step, true);
                yield return null;
            }
            shadow.Position = transform.position;
        }
        
        this.animator.SetTrigger(idleProperty);
        this.animator.speed = idleAnimationSpeed;
        yield return new WaitForSeconds(4);
        Walking = false;
    }
	
    /// <summary>
    /// Get's called when the PathRequester found a path for this employee.
    /// </summary>
    /// <param name="path"></param>
    /// <param name="success"></param>
    private void PathFound(List<Node> path, bool success)
    {
        if (success)
        {
            if (this.path != null)
                this.path[this.path.Count - 1].SetState(Enums.TileState.FREE);
            this.path = path;
            path[path.Count - 1].SetState(Enums.TileState.OCCUPIED);
            foreach (var node in path)
            {
                Debug.unityLogger.Log(LogType.Log, "Node: " + node.gridX.ToString() + ":" + node.gridY.ToString());
            }
            StopCoroutine(nameof(FollowPath));
            StartCoroutine(nameof(FollowPath));
        }
    }

    private void OnTileBlocked(Vector2 tile)
    {
        Vector3Int tileInt = new Vector3Int((int) tile.x, (int) tile.y, 0);
        Vector2Int tileInt2 = new Vector2Int((int) tile.x, (int) tile.y);
        // Test if employee is on tile
        if (grid.go_grid.WorldToCell(transform.position).Equals(tileInt))
        {
            // TODO: Test for state
            ResetWalkingPath();
        }
        // Test if tile is in path
        else if (Walking)
        {
            bool tileInPath = false;
            foreach (var node in path)
            {
                if (node.gridPosition.Equals(tileInt2))
                {
                    tileInPath = true;
                }
            }

            if (tileInPath)
            {
                ResetWalkingPath();
            }
        }
    }

    private void ResetWalkingPath()
    {
        Walking = false;
        StopCoroutine(nameof(FollowPath));
        RequestNewIdleWalk();
    }

    // Update is called once per frame
    void Update () {
        if (Idle & !Walking)
        {
            RequestNewIdleWalk();
        }
    }

    void OnDestroy()
    {
        Destroy(EmployeeShadow);
    }
}
