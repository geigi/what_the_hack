using System.Collections;
using GameSystem;
using UE.Events;
using UE.StateMachine;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace UI
{
    /// <summary>
    /// This component guides the player through a interactive tutorial.
    /// </summary>
    public class TutorialController : MonoBehaviour
    {
        [Header("States")] public State TutorialState;
        public State Page1, Page2, Page3, Page4, Page5, Page6;
        public StateManager MainUiManager;

        [Header("Own UI Elements")] public Button AbortButton;
        public Button GotItButton;
        public Toggle StartShowAgainToggle, EndShowAgainToggle;
        
        [Header("Events")] 
        public ObjectEvent FirstEmployeeHired;
        public GameEvent FirstMissionAccepted, FirstMissionSelected, WorkplaceInfoOpened;

        private bool active = false;
        private int stage = 0;

        private Button ShopButton, MissionsButton, EmployeesButton, OptionsButton;
        
        private UnityAction abortAction, nextAction, firstMissionAcceptedAction, firstMissionSelectedAction, workplaceInfoOpenedAction;
        private UnityAction<bool> showAgainToggleAction;
        private UnityAction<Object> firstEmployeeHiredAction;

        private void Awake()
        {
            // find foreign game objects
            ShopButton = GameObject.Find("ShopButton").GetComponent<Button>();
            MissionsButton = GameObject.Find("MissionButton").GetComponent<Button>();
            EmployeesButton = GameObject.Find("EmployeeButton").GetComponent<Button>();
            OptionsButton = GameObject.Find("OptionButton").GetComponent<Button>();
        }

        private void Start()
        {
            active = true;

            GotItButton.GetComponentInChildren<Text>().text = "Start";

            Page1.Enter();
            TutorialState.Enter();

            // connect UI events
            abortAction = Abort;
            nextAction = Next;
            showAgainToggleAction = onShowAgainChanged;

            AbortButton.onClick.AddListener(abortAction);
            GotItButton.onClick.AddListener(nextAction);
            StartShowAgainToggle.onValueChanged.AddListener(showAgainToggleAction);
            EndShowAgainToggle.onValueChanged.AddListener(showAgainToggleAction);
            
            // connect events to track user progress
            firstEmployeeHiredAction = onFirstEmployeeHired;
            firstMissionAcceptedAction = onFirstMissionAccepted;
            firstMissionSelectedAction = onFirstMissionSelected;
            workplaceInfoOpenedAction = onWorkplaceInfoOpened;
            
            FirstEmployeeHired.AddListener(firstEmployeeHiredAction);
            FirstMissionAccepted.AddListener(firstMissionAcceptedAction);
            FirstMissionSelected.AddListener(firstMissionSelectedAction);
            WorkplaceInfoOpened.AddListener(workplaceInfoOpenedAction);

            // disable buttons
            EnableUi(false);
        }

        private void Abort()
        {
            active = false;
            EnableUi(true);
            Destroy(gameObject);
        }

        private void Next()
        {
            if (Page1.IsActive())
            {
                Page2.Enter();
                GotItButton.GetComponentInChildren<Text>().text = "Got it";
                GotItButton.gameObject.SetActive(false);
                EmployeesButton.interactable = true;
            }
            else if (Page4.IsActive() || Page5.IsActive())
            {
                MainUiManager.InitialState.Enter();
            }
            else if (Page6.IsActive())
            {
                active = false;
                EnableUi(true);
                Destroy(gameObject);
                GameTime.GameTime.Instance.StartGame();
            }
        }

        private void onShowAgainChanged(bool showAgain)
        {
            SettingsManager.SetTutorialState(showAgain);
        }

        // Page 3
        private void onFirstEmployeeHired(Object obj)
        {
            EmployeesButton.interactable = false;
            MissionsButton.interactable = true;
            Page3.Enter();
            TutorialState.Enter();
        }

        // Page 4
        private void onFirstMissionAccepted()
        {
            MissionsButton.interactable = false;
            GotItButton.gameObject.SetActive(true);
            GotItButton.GetComponentInChildren<Text>().text = "Got it";
            Page4.Enter();
            TutorialState.Enter();
        }

        // Page 5
        private void onFirstMissionSelected()
        {
            StartCoroutine(DelayPage5());
        }

        private IEnumerator DelayPage5()
        {
            yield return new WaitForSeconds(3);
            Page5.Enter();
            TutorialState.Enter();
        }
        
        // Page 6
        private void onWorkplaceInfoOpened()
        {
            StartCoroutine(DelayPage6());
        }
        
        private IEnumerator DelayPage6()
        {
            yield return new WaitForSeconds(1);
            AbortButton.gameObject.SetActive(false);
            GotItButton.GetComponentInChildren<Text>().text = "Finish";
            Page6.Enter();
            TutorialState.Enter();
        }
        
        private void EnableUi(bool enable)
        {
            ShopButton.interactable = enable;
            MissionsButton.interactable = enable;
            EmployeesButton.interactable = enable;
            OptionsButton.interactable = enable;
        }
        
        private void OnDestroy()
        {
            AbortButton.onClick.RemoveListener(abortAction);
            GotItButton.onClick.RemoveListener(nextAction);
            StartShowAgainToggle.onValueChanged.RemoveListener(showAgainToggleAction);
            EndShowAgainToggle.onValueChanged.RemoveListener(showAgainToggleAction);
            FirstEmployeeHired.RemoveListener(firstEmployeeHiredAction);
            FirstMissionAccepted.RemoveListener(firstMissionAcceptedAction);
            FirstMissionSelected.RemoveListener(firstMissionSelectedAction);
            WorkplaceInfoOpened.RemoveListener(workplaceInfoOpenedAction);
        }
    }
}