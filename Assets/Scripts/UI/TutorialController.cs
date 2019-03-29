using System.Collections;
using GameSystem;
using SaveGame;
using UE.Events;
using UE.StateMachine;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Wth.ModApi.Employees;

namespace UI
{
    /// <summary>
    /// This component guides the player through a interactive tutorial.
    /// </summary>
    public class TutorialController : MonoBehaviour
    {
        [Header("States")] public State TutorialState;
        public State Page1, Page2, Page3, Page4, Page5, Page6, Page7;
        public StateManager MainUiManager;

        [Header("Own UI Elements")] public Button AbortButton;
        public Button GotItButton;
        public Toggle StartShowAgainToggle, EndShowAgainToggle;
        public Text Page7EmployeeName;
        
        [Header("Events")] 
        public ObjectEvent FirstEmployeeHired;
        public GameEvent FirstMissionAccepted, FirstMissionSelected, WorkplaceInfoOpened;
        public NetObjectEvent FirstLevelUp;

        private bool active = false;

        private Button ShopButton, MissionsButton, EmployeesButton, OptionsButton;

        private UnityAction abortAction,
            nextAction,
            firstMissionAcceptedAction,
            firstMissionSelectedAction,
            workplaceInfoOpenedAction;
        private UnityAction<bool> showAgainToggleAction;
        private UnityAction<Object> firstEmployeeHiredAction;
        private UnityAction<object> firstLevelUpAction;

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

            GotItButton.GetComponentInChildren<Text>().text = "Show";

            // connect UI events
            abortAction = End;
            nextAction = Next;
            showAgainToggleAction = onShowAgainChanged;

            AbortButton.onClick.AddListener(abortAction);
            GotItButton.onClick.AddListener(nextAction);
            StartShowAgainToggle.onValueChanged.AddListener(showAgainToggleAction);
            EndShowAgainToggle.onValueChanged.AddListener(showAgainToggleAction);
            
            // connect events to track user progress
            firstEmployeeHiredAction = EnterPage3;
            firstMissionAcceptedAction = EnterPage4;
            firstMissionSelectedAction = EnterPage5;
            workplaceInfoOpenedAction = EnterPage6;
            firstLevelUpAction = EnterPage7;
            
            FirstEmployeeHired.AddListener(firstEmployeeHiredAction);
            FirstMissionAccepted.AddListener(firstMissionAcceptedAction);
            FirstMissionSelected.AddListener(firstMissionSelectedAction);
            WorkplaceInfoOpened.AddListener(workplaceInfoOpenedAction);
            FirstLevelUp.AddListener(firstLevelUpAction);

            // disable buttons
            EnableUi(false);

            if (!GameSettings.NewGame)
            {
                var lastStage = SaveGameSystem.Instance.GetCurrentSaveGame().TutorialStage;

                if (lastStage == 1)
                {
                    Page1.Enter();
                    TutorialState.Enter();
                }
                else if (lastStage == 2)
                    EnterPage2();
                else if (lastStage == 3)
                    EnterPage3(null);
                else if (lastStage == 4)
                    EnterPage4();
                else if (lastStage == 5)
                    StartCoroutine(DelayPage5(0));
                else if (lastStage == 6)
                    StartCoroutine(DelayPage6(0));
                else if (lastStage == 7)
                {
                    EnableUi(true);
                    MainUiManager.InitialState.Enter();
                    GameTime.GameTime.Instance.StartGame();
                }
            }
            else
            {
                SaveGameSystem.Instance.SetTutorialLevel(1);
                Page1.Enter();
                TutorialState.Enter();
            }
        }

        public void End()
        {
            active = false;
            EnableUi(true);
            SaveGameSystem.Instance.SetTutorialLevel(-1);
            GameTime.GameTime.Instance.StartGame();
            Destroy(gameObject);
        }

        private void Next()
        {
            if (Page1.IsActive())
            {
                EnterPage2();
            }
            else if (Page4.IsActive() || Page5.IsActive())
            {
                MainUiManager.InitialState.Enter();
            }
            else if (Page6.IsActive())
            {
                EnableUi(true);
                GameTime.GameTime.Instance.StartGame();
                SaveGameSystem.Instance.SetTutorialLevel(7);
                MainUiManager.InitialState.Enter();
            }
            else if (Page7.IsActive())
            {
                active = false;
                Destroy(gameObject);
            }
        }

        private void EnterPage2()
        {
            Page2.Enter();
            GotItButton.gameObject.SetActive(false);
            EmployeesButton.interactable = true;
            SaveGameSystem.Instance.SetTutorialLevel(2);
        }

        // Page 3
        private void EnterPage3(Object obj)
        {
            GotItButton.gameObject.SetActive(false);
            EmployeesButton.interactable = false;
            MissionsButton.interactable = true;
            Page3.Enter();
            TutorialState.Enter();
            SaveGameSystem.Instance.SetTutorialLevel(3);
        }

        // Page 4
        private void EnterPage4()
        {
            MissionsButton.interactable = false;
            GotItButton.gameObject.SetActive(true);
            GotItButton.GetComponentInChildren<Text>().text = "Got it";
            Page4.Enter();
            TutorialState.Enter();
            SaveGameSystem.Instance.SetTutorialLevel(4);
        }

        // Page 5
        private void EnterPage5()
        {
            StartCoroutine(DelayPage5(3));
        }

        private IEnumerator DelayPage5(int delay)
        {
            yield return new WaitForSeconds(delay);
            GotItButton.GetComponentInChildren<Text>().text = "Got it";
            Page5.Enter();
            TutorialState.Enter();
            SaveGameSystem.Instance.SetTutorialLevel(5);
        }
        
        // Page 6
        private void EnterPage6()
        {
            if (!GameSettings.NewGame && SaveGameSystem.Instance.GetCurrentSaveGame().TutorialStage > 5)
                return;
            StartCoroutine(DelayPage6(1));
        }
        
        private IEnumerator DelayPage6(int delay)
        {
            yield return new WaitForSeconds(delay);
            AbortButton.gameObject.SetActive(false);
            GotItButton.GetComponentInChildren<Text>().text = "Finish";
            Page6.Enter();
            TutorialState.Enter();
            SaveGameSystem.Instance.SetTutorialLevel(6);
        }
        
        // Page 7
        private void EnterPage7(object emp)
        {
            StartCoroutine(DelayPage7(3, emp as EmployeeData));
        }
        
        private IEnumerator DelayPage7(int delay, EmployeeData emp)
        {
            yield return new WaitForSeconds(delay);
            AbortButton.gameObject.SetActive(false);
            GotItButton.GetComponentInChildren<Text>().text = "Got it";
            Page7EmployeeName.text = emp.Name;
            Page7.Enter();
            TutorialState.Enter();
            SaveGameSystem.Instance.SetTutorialLevel(-1);
        }
        
        private void onShowAgainChanged(bool showAgain)
        {
            SettingsManager.SetTutorialState(showAgain);
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
            FirstLevelUp.RemoveListener(firstLevelUpAction);
        }
    }
}