using Interfaces;
using Missions;
using Team;
using UE.StateMachine;
using UnityEngine;
using Utils;

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
            if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began || Input.GetMouseButtonDown(0))
            {
                var hitInformation = RaycastHit();

                if (hitInformation.collider != null)
                {
                    GameObject touchedObject = hitInformation.transform.gameObject;
                    
                    Debug.Log("Start click: " + touchedObject.transform.name);

                    var touchable = touchedObject.GetComponent(typeof(ITouchable)) as ITouchable;
                    
                    if (touchable != null)
                    {
                        touchable.TouchStarted();
                    }
                }
            }
            //We check if we have more than one touch happening.
            //We also check if the first touches phase is Ended (that the finger was lifted)
            else if (Input.touchCount > 0 && Input.GetTouch(0).phase == touchPhase || Input.GetMouseButtonUp(0))
            {
                var hitInformation = RaycastHit();

                if (hitInformation.collider != null)
                {
                    //We should have hit something with a 2D Physics collider!
                    GameObject touchedObject = hitInformation.transform.gameObject;
                    //touchedObject should be the object someone touched.
                    Debug.Log("End click: " + touchedObject.transform.name);

                    var employeeComponent = touchedObject.GetComponent<Employee>();
                    var workplaceComponent = touchedObject.GetComponent<Workplace>();
                    
                    // Employee gets selected
                    if (employeeComponent != null)
                    {
                        DeselectWorkplace();
                        SelectedWorkspace = null;
                        SelectedEmployee = touchedObject;
                    }
                    // Workplace gets selected
                    else if (workplaceComponent != null)
                    {
                        if (SelectedWorkspace != null)
                        {
                            SelectedWorkspace.GetComponent<Workplace>().OnSelect(false);
                        }
                        
                        // Send employee to workplace
                        if (SelectedEmployee != null && !workplaceComponent.IsOccupied())
                        {
                            if (SelectedEmployee.GetComponent<Employee>().State != Enums.EmployeeState.WORKING)
                            {
                                SelectedWorkspace = touchedObject;
                                workplaceComponent.OnSelect(true);
                                SelectMissionState.Enter();
                            }
                        }
                        // Stop working if employee is already working at this workplace
                        else if (SelectedEmployee != null &&
                                 workplaceComponent.GetOccupyingEmployee() ==
                                 SelectedEmployee.GetComponent<Employee>())
                        {
                            workplaceComponent.StopWorking();
                            SelectedEmployee = null;
                        }
                        else
                        {
                            SelectedEmployee = null;
                            SelectedWorkspace = touchedObject;
                            workplaceComponent.OnSelect(true);
                        }
                    }

                    var touchable = touchedObject.GetComponent(typeof(ITouchable)) as ITouchable;
                    touchable?.TouchEnded();
                }
                // clear selection
                else if (!SelectMissionState.IsActive())
                {
                    DeselectWorkplace();
                    SelectedWorkspace = null;
                    
                    SelectedEmployee = null;
                }
            }
        }

        private void DeselectWorkplace()
        {
            if (SelectedWorkspace != null) SelectedWorkspace.GetComponent<Workplace>().OnSelect(false);
        }

        private RaycastHit2D RaycastHit()
        {
            Vector3 touchClickPos;
            if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonUp(0))
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
            return hitInformation;
        }

        public void MissionSelected(Mission mission)
        {
            if (SelectedEmployee != null && SelectedWorkspace != null)
            {
                var employeeComponent = SelectedEmployee.GetComponent<Employee>();
                employeeComponent.GoToWorkplace(SelectedWorkspace, mission);
                SelectedEmployee = null;
                DeselectWorkplace();
                SelectedWorkspace = null;
                SelectMissionState.stateManager.InitialState.Enter();
            }
        }
    }
}