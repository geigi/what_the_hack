using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Extensions;
using GameTime;
using Interfaces;
using Items;
using Missions;
using Pathfinding;
using Team;
using UE.Common;
using UE.Events;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.Tilemaps;
using Utils;
using World;
using Wth.ModApi.Employees;
using Wth.ModApi.Tools;

/// <summary>
/// This class represents an employee in the Game. All data is saved in the EmployeeData Scriptable Object.
/// All employee related logic is implemented here.
/// </summary>
public class Employee : MonoBehaviour, ISelectable, IPointerUpHandler, IPointerDownHandler
{
    public const float minPathUpdateTime = .2f;
    public const float walkingSpeed = 1.0f;
    public const float regularAnimationSpeed = 1.0f;
    public const float idleAnimationSpeed = 0.75f;
    public const float SCORE_MISSION_COMPLETED = 1;
    public const float SCORE_MISSION_COMPLETED_PERLEVEL = 0.1f;
    private const int COST_INCREMENT_ALLPURPOSE = 12;
    private const int COST_NEW_SKILL = 14;
    private const int COST_INCREMENT_SKILL = 6;
    private const int MAX_SKILL_NUMBER = 4;

    public UnityEvent stateEvent = new UnityEvent();

    public EmployeeData EmployeeData;

    public string Name
    {
        get
        {
            if (EmployeeData.generatedData != null)
            {
                return EmployeeData.generatedData.name;
            }
            else
            {
                return EmployeeData.EmployeeDefinition.EmployeeName;
            }
        }
    }

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
    private Coroutine followPathCoroutine;

    private Tilemap tilemap;

    private GameObject EmployeeShadow;
    private EmployeeShadow shadow;
    private SpriteOutline spriteOutline;
    private Vector3 defaultScale = new Vector3(1f, 1f, 1f);
    private Vector3 flippedScale = new Vector3(-1f, 1f, 1f);
    private Workplace workplace;

    public Enums.EmployeeState State
    {
        get => EmployeeData.State;
        private set
        {
            EmployeeData.State = value;
            stateEvent.Invoke();

            switch (value)
            {
                case Enums.EmployeeState.IDLE:
                case Enums.EmployeeState.PAUSED:
                    this.animator.ResetTrigger(walkingProperty);
                    this.animator.ResetTrigger(workingProperty);
                    this.animator.SetTrigger(idleProperty);
                    this.animator.speed = idleAnimationSpeed;
                    break;
                case Enums.EmployeeState.WALKING:
                    this.animator.ResetTrigger(idleProperty);
                    this.animator.ResetTrigger(workingProperty);
                    this.animator.SetTrigger(walkingProperty);
                    this.animator.speed = regularAnimationSpeed;
                    break;
                case Enums.EmployeeState.WORKING:
                    this.animator.ResetTrigger(walkingProperty);
                    this.animator.ResetTrigger(idleProperty);
                    this.animator.SetTrigger(workingProperty);
                    this.animator.speed = regularAnimationSpeed;
                    break;
            }
        }
    }

    private List<Node> path;
    private static readonly int walkingProperty = Animator.StringToHash("walking");
    private static readonly int idleProperty = Animator.StringToHash("idle");
    private static readonly int workingProperty = Animator.StringToHash("working");

    public void Awake()
    {
        contentHub = ContentHub.Instance;
        clipsMale = contentHub.maleAnimationClips;
        clipsFemale = contentHub.femaleAnimationClips;

        tileBlockedAction = OnTileBlocked;
        tileBlockedEvent = ContentHub.Instance.TileBlockedEvent;
        tileBlockedEvent.AddListener(tileBlockedAction);

        employeeLayer = GameObject.FindWithTag("EmployeeLayer");
        this.grid = GameObject.FindWithTag("Pathfinding").GetComponent<AGrid>();
        tilemap = GameObject.FindWithTag("Tilemap").GetComponent<Tilemap>();
        gameObject.transform.parent = employeeLayer.transform;
    }

    void Start()
    {
    }

    /// <summary>
    /// This needs to be called before the employee is used.
    /// Fills the object with specific data.
    /// </summary>
    /// <param name="employeeData">Data for this employee.</param>
    /// <param name="newEmployee">Is this a freshman?</param>
    public void init(EmployeeData employeeData, bool isFreshman)
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

        if (employeeData.EmployeeDefinition != null)
        {
            Debug.Log("This is a special Employee");
            //special employee
            idle_anim = employeeData.EmployeeDefinition.IdleAnimation;
            walking_anim = employeeData.EmployeeDefinition.WalkingAnimation;
            working_anim = employeeData.EmployeeDefinition.WorkingAnimation;
        }
        else
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

        spriteOutline = gameObject.AddComponent<SpriteOutline>();
        spriteOutline.enabled = false;

        if (isFreshman) PlaceOnRandomTile();
        else
        {
            gameObject.transform.position =
                tilemap.GetCellCenterWorld(new Vector3Int(EmployeeData.Position.x, EmployeeData.Position.y, 0));
            grid.getNode(employeeData.Position).SetState(Enums.TileState.OCCUPIED);
            if (EmployeeData.State != Enums.EmployeeState.WORKING)
                EmployeeData.State = Enums.EmployeeState.IDLE;
        }
    }

    private void PlaceOnRandomTile()
    {
        var tile = grid.getRandomFreeNode();

        gameObject.transform.position = tilemap.GetCellCenterWorld(new Vector3Int(tile.gridX, tile.gridY, 0));
        EmployeeData.Position = tile.gridPosition;

        tile.SetState(Enums.TileState.OCCUPIED);

        IdleWalking(true);
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

    public void GoToWorkplace(Workplace workplace, Mission mission)
    {
        workplace.Occupy(this, mission);
        State = Enums.EmployeeState.PAUSED;

        StopFollowPath();

        RequestNewWalkToWorkplace(workplace);
    }

    /// <summary>
    /// Stop working and return to IDLE.
    /// Levels the employee if the mission was completed successfully.
    /// </summary>
    /// <param name="completedSuccessfully"></param>
    public void StopWorking(bool completedSuccessfully)
    {
        if (completedSuccessfully)
        {
            EmployeeData.IncrementFreeScore(SCORE_MISSION_COMPLETED +
                                            workplace.Mission.Difficulty * SCORE_MISSION_COMPLETED_PERLEVEL);
            LevelUp();
        }

        workplace = null;
        grid.getNode(EmployeeData.Position).SetState(Enums.TileState.FREE);
        State = Enums.EmployeeState.IDLE;
        IdleWalking(true);
    }

    private void SetSpriteThroughScript(Object sprite) => shadow.SetSpriteThroughObject(sprite);

    /// <summary>
    /// Start/Stop idle walking.
    /// </summary>
    /// <param name="start"></param>
    public void IdleWalking(bool start)
    {
        if (start)
        {
            State = Enums.EmployeeState.IDLE;
        }
        else
        {
            State = Enums.EmployeeState.PAUSED;
        }
    }

    private void RequestNewIdleWalk()
    {
        var callback = new System.Action<List<Node>, bool, object>(PathFound);
        var position = gameObject.transform.position;
        var go = this.grid.getNode(position).gridPosition;
        var end = this.grid.getRandomFreeNode().gridPosition;
        Pathfinding.PathRequestManager.RequestPath(new Pathfinding.PathRequest(go, end, null, callback));
    }

    private void RequestNewWalkToWorkplace(Workplace workplace)
    {
        var callback = new System.Action<List<Node>, bool, object>(WorkplacePathFound);
        var position = gameObject.transform.position;
        var go = this.grid.getNode(position).gridPosition;

        var employeeCell = grid.go_grid.WorldToCell(position);
        if (employeeCell.ToVector2Int().Equals(workplace.GetChairTile()))
        {
            StartWorking(workplace);
        }
        else
        {
            PathRequestManager.RequestPath(new PathRequest(go, workplace.GetChairTile(), workplace, callback));
        }
    }

    /// <summary>
    /// Follow the calculated path.
    /// </summary>
    /// <returns></returns>
    IEnumerator FollowPath(Workplace workplace)
    {
        bool followingPath = true;
        int pathIndex = 0;
        State = Enums.EmployeeState.WALKING;
        this.animator.SetTrigger(walkingProperty);
        this.animator.speed = regularAnimationSpeed;

        while (followingPath & State == Enums.EmployeeState.WALKING)
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
                    EmployeeData.Position = path[pathIndex].gridPosition;
                    distance = Vector3.Distance(transform.position, path[pathIndex].worldPosition);
                    step = (path[pathIndex].worldPosition - transform.position).normalized * stepLength;
                }
            }

            if (followingPath)
            {
                if (step.x < 0)
                {
                    transform.localScale = defaultScale;
                }
                else if (step.x > 0)
                {
                    transform.localScale = flippedScale;
                }

                var oldPos = transform.position;
                transform.Translate(step);
                // set the sorting order
                spriteRenderer.sortingOrder = grid.CalculateSortingLayer(oldPos + step, true);
                yield return null;
            }

            shadow.Position = transform.position;
        }

        if (workplace == null)
        {
            State = Enums.EmployeeState.PAUSED;
            yield return new WaitForSeconds(4);
            State = Enums.EmployeeState.IDLE;
        }
        else
        {
            StartWorking(workplace);
            yield return null;
        }
    }

    private void StartWorking(Workplace workplace)
    {
        this.workplace = workplace;
        State = Enums.EmployeeState.WORKING;
        spriteRenderer.sortingOrder = workplace.GetEmployeeSortingOrder();
        transform.localScale = defaultScale;
        transform.Translate(-0.07f, 0, 0);
        workplace.StartWorking();
    }

    #region Level System
    /// <summary>
    /// Call this to trigger a level up.
    /// Aborts, if the employee doesn't have enough score points.
    /// </summary>
    private void LevelUp()
    {
        if (EmployeeData.FreeScore < EmployeeData.LevelUpScoreNeeded) return;
        Debug.Log(Name + " levels up!");

        EmployeeData.SkillPoints += 1;
        spendScorePoints();
        EmployeeData.Level += 1;
        EmployeeData.UseScore(EmployeeData.LevelUpScoreNeeded);
        onLevelUp();
    }

    /// <summary>
    /// Spend the score points by educating the employee.
    /// </summary>
    private void spendScorePoints()
    {
        var pointsToSpend = EmployeeData.LevelUpScoreNeeded;
        Educate(pointsToSpend);
    }

    /// <summary>
    /// Educating can have a number of different effects like increasing a skill, learning a new one or learning a new special.
    /// </summary>
    /// <param name="pointsToSpend"></param>
    private void Educate(float pointsToSpend)
    {
        bool success = false;
        while (!success)
        {
            int roll = RandomUtils.RollDice(12);

            switch (roll)
            {
                case 1:
                    success = learnSkill(pointsToSpend);
                    break;
                case 2:
                case 3:
                case 4:
                case 5:
                case 6:
                case 7:
                case 8:
                case 9:
                case 10:
                case 11:
                    success = rollSpecial();
                    break;
                case 12:
                    return; //wasting time playing video games
                case 13:
                    break;
                //TODO: Implement more features for Employees!
                case 20:
                    //this is a magic employee and gets a super skill!
                    break;
            }
        }
    }

    /// <summary>
    /// Increment a skill.
    /// The method first verifies, that enough skill points are available.
    /// </summary>
    /// <param name="skill"></param>
    /// <returns></returns>
    public void IncrementSkill(Skill skill)
    {
        if (EmployeeData.SkillPoints >= skill.LevelUpCost)
        {
            EmployeeData.SkillPoints -= skill.LevelUpCost;
            skill.LevelUp();
            
            Debug.Log(Name + " incremented " + skill.GetName() + ".");
        }
    }

    /// <summary>
    /// Learn a new skill and add the first points to it.
    /// </summary>
    /// <param name="pointsToSpend"></param>
    /// <returns></returns>
    private bool learnSkill(float pointsToSpend)
    {
        if (EmployeeData.Skills.Count >= MAX_SKILL_NUMBER) return false;

        SkillDefinition skillDef;

        do
        {
            var skillSet = ContentHub.Instance.GetSkillSet();
            skillDef = skillSet.keys.RandomElement();
        } while (!isUniqueSkill(skillDef));

        var skill = new Skill(skillDef);
        addSkill(skill);

        for (int i = 0; i < (int) (pointsToSpend / 2); i++)
        {
            skill.LevelUp();
        }

        Debug.Log(Name + " learned new skill: " + skillDef.skillName);
        return true;
    }

    /// <summary>
    /// Add a new employee special.
    /// </summary>
    /// <returns></returns>
    private bool rollSpecial()
    {
        //TODO: Add a new special
        return true;
    }

    /// <summary>
    /// Tests, if a skill is already present.
    /// </summary>
    /// <param name="skill"></param>
    /// <returns></returns>
    private bool isUniqueSkill(SkillDefinition skill)
    {
        return EmployeeData.Skills.All(s => s.SkillData != skill);
    }

    /// <summary>
    /// Adds a new skill.
    /// </summary>
    /// <param name="skill"></param>
    private void addSkill(Skill skill)
    {
        EmployeeData.Skills.Add(skill);
    }

    /// <summary>
    /// Notify event listeners.
    /// </summary>
    private void onLevelUp()
    {
        
    }
    #endregion


    /// <summary>
    /// Get's called when the PathRequester found a path for this employee.
    /// </summary>
    /// <param name="path"></param>
    /// <param name="success"></param>
    private void PathFound(List<Node> path, bool success, object none)
    {
        if (success)
        {
            if (this.path != null && this.path[this.path.Count - 1].GetState() == Enums.TileState.OCCUPIED)
                this.path[this.path.Count - 1].SetState(Enums.TileState.FREE);
            this.path = path;
            path[path.Count - 1].SetState(Enums.TileState.OCCUPIED);
            
            //foreach (var node in path)
            //{
            //    Debug.unityLogger.Log(LogType.Log, "Node: " + node.gridX.ToString() + ":" + node.gridY.ToString());
            //}

            StopFollowPath();
            followPathCoroutine = StartCoroutine(FollowPath(null));
        }
    }

    /// <summary>
    /// Get's called when the PathRequester found a path for this employee.
    /// </summary>
    /// <param name="path"></param>
    /// <param name="success"></param>
    /// <param name="workplace"></param>
    private void WorkplacePathFound(List<Node> path, bool success, object workplace)
    {
        if (success)
        {
            if (this.path != null && this.path[this.path.Count - 1].GetState() == Enums.TileState.OCCUPIED)
                this.path[this.path.Count - 1].SetState(Enums.TileState.FREE);
            this.path = path;
            path[path.Count - 1].SetState(Enums.TileState.OCCUPIED);
            StopFollowPath();
            followPathCoroutine = StartCoroutine(FollowPath((Workplace) workplace));
        }
    }

    private void OnTileBlocked(Vector2 tile)
    {
        Vector3Int tileInt = new Vector3Int((int) tile.x, (int) tile.y, 0);
        Vector2Int tileInt2 = new Vector2Int((int) tile.x, (int) tile.y);
        // Test if employee is on tile
        if (grid.go_grid.WorldToCell(transform.position).Equals(tileInt))
        {
            if (State == Enums.EmployeeState.IDLE)
                ResetWalkingPath();
        }
        // Test if tile is in path
        else if (State == Enums.EmployeeState.WALKING)
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
        State = Enums.EmployeeState.PAUSED;
        StopFollowPath();
        RequestNewIdleWalk();
        State = Enums.EmployeeState.IDLE;
    }

    private void StopFollowPath()
    {
        if (followPathCoroutine != null)
        {
            StopCoroutine(followPathCoroutine);
            followPathCoroutine = null;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (State == Enums.EmployeeState.IDLE)
        {
            RequestNewIdleWalk();
        }
    }

    void OnDestroy()
    {
        Destroy(EmployeeShadow);
    }

    public void OnDeselect()
    {
        spriteOutline.enabled = false;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        GameSelectionManager.Instance.ClearWorkplace();
        GameSelectionManager.Instance.Employee = this;
        spriteOutline.enabled = true;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        spriteOutline.enabled = true;
    }
}