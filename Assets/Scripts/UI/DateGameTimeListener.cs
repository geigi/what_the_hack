using GameTime;
using UE.Events;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Object = UnityEngine.Object;

/// <summary>
/// This class listens to a event and updates a text field with
/// the received Date object.
/// </summary>
public class DateGameTimeListener : MonoBehaviour
{
    /// <summary>
    /// The event that should be listened to.
    /// </summary>
    public ObjectEvent GameTimeDayEvent;
    /// <summary>
    /// Separation string between day of week and date.
    /// </summary>
    public string DayDateSeparator = "   ";

    private Text dateText;
    private UnityAction<Object> dayChangedAction;
    // Start is called before the first frame update
    void Awake()
    {
        dateText = GetComponent<Text>();
        dayChangedAction += TickListener;
        GameTimeDayEvent.AddListener(dayChangedAction);
    }

    /// <summary>
    /// Update the text object.
    /// </summary>
    /// <param name="date"></param>
    private void TickListener(Object date)
    {
        var dateObject = (GameDate) date;
        var day = dateObject.DayOfWeek.ToString().Substring(0, 3);
        var dateString = dateObject.DateTime.ToString("dd MMM y");
        dateText.text = day + DayDateSeparator + dateString;
    }
}
