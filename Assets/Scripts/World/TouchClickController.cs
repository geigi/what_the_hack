using Interfaces;
using Missions;
using Team;
using UE.StateMachine;
using UnityEngine;

namespace World
{
    public class TouchClickController : MonoBehaviour
    {
        //Change me to change the touch phase used.
        TouchPhase touchPhase = TouchPhase.Ended;
        public Camera Camera;

        public State SelectMissionState;

        /// <summary>
        /// Contains the currently selected employee.
        /// Null, if no employee is selected.
        /// </summary>
        public GameObject SelectedEmployee;

        /// <summary>
        /// Contains the currently selected workplace.
        /// Null, if no workplace is selected.
        /// </summary>
        public GameObject SelectedWorkspace;

        void Update()
        {
            //We check if we have more than one touch happening.
            //We also check if the first touches phase is Ended (that the finger was lifted)
            if (Input.touchCount > 0 && Input.GetTouch(0).phase == touchPhase || Input.GetMouseButtonDown(0))
            {
                Vector3 touchClickPos;
                if (Input.GetMouseButtonDown(0))
                {
                    touchClickPos = Input.mousePosition;
                }
                else
                {
                    touchClickPos = Input.GetTouch(0).position;
                }

                //We transform the touch position into word space from screen space and store it.
                var touchPosWorld = Camera.ScreenToWorldPoint(touchClickPos);

                Vector2 touchPosWorld2D = new Vector2(touchPosWorld.x, touchPosWorld.y);

                //We now raycast with this information. If we have hit something we can process it.
                RaycastHit2D hitInformation = Physics2D.Raycast(touchPosWorld2D, Camera.transform.forward);

                if (hitInformation.collider != null)
                {
                    //We should have hit something with a 2D Physics collider!
                    GameObject touchedObject = hitInformation.transform.gameObject;
                    //touchedObject should be the object someone touched.
                    Debug.Log("Touched " + touchedObject.transform.name);

                    if (touchedObject.GetComponent<Employee>() != null)
                    {
                        SelectedWorkspace = null;
                        SelectedEmployee = touchedObject;
                    }
                    else if (touchedObject.GetComponent<Workplace>() != null)
                    {
                        // Send employee to workplace
                        if (SelectedEmployee != null && !touchedObject.GetComponent<Workplace>().IsOccupied())
                        {
                            SelectedWorkspace = touchedObject;
                            SelectMissionState.Enter();
                        }
                        // Stop working if employee is already working at this workplace
                        else if (SelectedEmployee != null &&
                                 touchedObject.GetComponent<Workplace>().GetOccupyingEmployee() ==
                                 SelectedEmployee.GetComponent<Employee>())
                        {
                            touchedObject.GetComponent<Workplace>().StopWorking();
                            SelectedEmployee = null;
                        }
                        else
                        {
                            SelectedEmployee = null;
                            SelectedWorkspace = touchedObject;
                        }
                    }

                    var touchable = touchedObject.GetComponent(typeof(Touchable)) as Touchable;
                    touchable?.Touched();
                }
                else if (!SelectMissionState.IsActive())
                {
                    SelectedEmployee = null;
                    SelectedWorkspace = null;
                }
            }
        }

        public void MissionSelected(Mission mission)
        {
            if (SelectedEmployee != null && SelectedWorkspace != null)
            {
                var employeeComponent = SelectedEmployee.GetComponent<Employee>();
                employeeComponent.GoToWorkplace(SelectedWorkspace, mission);
                SelectedEmployee = null;
                SelectedWorkspace = null;
                SelectMissionState.stateManager.InitialState.Enter();
            }
        }
    }
}