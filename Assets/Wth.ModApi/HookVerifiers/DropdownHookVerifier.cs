using System.Collections.Generic;
using ModTool.Interface;
using UnityEngine.UI;


public class DropdownHookVerifier : ModBehaviour
{
    public Button OkButton;
    public Dropdown Dropdown;
    public MissionHook Hook;
    public List<int> ValidOptions;

    private void Start()
    {
        OkButton.onClick.AddListener(Verify);
    }

    private void Verify()
    {
        if (ValidOptions.Contains(Dropdown.value))
            Hook.RaiseSuccess();
        else
            Hook.RaiseFailed();
    }

    private void OnDestroy()
    {
        OkButton.onClick.RemoveListener(Verify);
    }
}