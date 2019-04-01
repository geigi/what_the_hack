using System;
using System.Collections;
using System.Collections.Generic;
using GameSystem;
using UI;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class DifficultySelection : MonoBehaviour
{
    public GameObject DifficultyTitleText;
    public GameObject DifficultyText;
    public Dropdown Dropdown;
    public MissionList DefaultMissionList;

    private UnityAction<int> dropdownAction;
    void Awake()
    {
        dropdownAction += ChangeDescription;
        Dropdown.onValueChanged.AddListener(dropdownAction);
    }

    private void OnEnable()
    {
        var mission = ModHolder.Instance.GetMissionList() ?? DefaultMissionList;
        List<MissionList.DifficultyOption> availableDifficulties = new List<MissionList.DifficultyOption>();
        foreach (var m in mission.missionList)
        {
            if (!availableDifficulties.Contains(m.Difficulty))
                availableDifficulties.Add(m.Difficulty);
        }

        Dropdown.options.Clear();
        
        foreach (var difficulty in availableDifficulties)
        {
            Dropdown.options.Add(new Dropdown.OptionData(difficulty.ToString()));
        }
        
        ChangeDescription(Dropdown.value);
    }

    private void ChangeDescription(int difficulty)
    {
        MissionList.DifficultyOption diff;
        Enum.TryParse(Dropdown.options[difficulty].text, true, out diff);
        var missionList = ModHolder.Instance.GetMissionList() ?? DefaultMissionList;
        string description = "";
        string title = diff.ToString();
        switch (diff)
        {
            case MissionList.DifficultyOption.Easy:
                description = missionList.easyDifficultyDescription;
                GameSettings.Difficulty = MissionList.DifficultyOption.Easy;
                break;
            case MissionList.DifficultyOption.Normal:
                description = missionList.normalDifficultyDescription;
                GameSettings.Difficulty = MissionList.DifficultyOption.Normal;
                break;
            case MissionList.DifficultyOption.Hard:
                description = missionList.hardDifficultyDescription;
                GameSettings.Difficulty = MissionList.DifficultyOption.Hard;
                break;
            case MissionList.DifficultyOption.Guru:
                description = missionList.guruDifficultyDescription;
                GameSettings.Difficulty = MissionList.DifficultyOption.Guru;
                break;
        }

        DifficultyTitleText.GetComponent<Text>().text = title;
        DifficultyText.GetComponent<Text>().text = description;
    }

}
