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
        ChangeDescription(0);
    }

    private void ChangeDescription(int difficulty)
    {
        var diff = (MissionList.DifficultyOption) difficulty;
        var missionList = ModHolder.Instance.GetMissionList() ?? DefaultMissionList;
        string description = "";
        string title = diff.ToString();
        switch (diff)
        {
            case MissionList.DifficultyOption.Easy:
                description = missionList.easyDifficultyDescription;
                break;
            case MissionList.DifficultyOption.Normal:
                description = missionList.normalDifficultyDescription;
                break;
            case MissionList.DifficultyOption.Hard:
                description = missionList.hardDifficultyDescription;
                break;
            case MissionList.DifficultyOption.Guru:
                description = missionList.guruDifficultyDescription;
                break;
        }

        DifficultyTitleText.GetComponent<Text>().text = title;
        DifficultyText.GetComponent<Text>().text = description;
    }

}
