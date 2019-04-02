using System.Collections.Generic;
using System.Linq;
using ModTool.Interface;
using UnityEngine.UI;

public class ToggleHookVerifier : ModBehaviour
{
    public Button OkButton;
    public List<Toggle> ValidToggles;
    public List<Toggle> InvalidToggles;
    public MissionHook Hook;

    private void Start()
    {
        OkButton.onClick.AddListener(Verify);
    }

    private void Verify()
    {
        bool success = true;

        success = ValidToggles.All(t => t.isOn);
        if (success)
            success = InvalidToggles.All(t => !t.isOn);
        
        if (success)
            Hook.RaiseSuccess();
        else
            Hook.RaiseFailed();
    }

    private void OnDestroy()
    {
        OkButton.onClick.RemoveListener(Verify);
    }
}