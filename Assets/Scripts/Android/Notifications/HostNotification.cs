namespace Android.Notifications
{
    /// <summary>
    /// Represents a host notification.
    /// </summary>
    public class HostNotification
    {
        public string Title, Body;
        public int Delay;

        public HostNotification(string title, string body, int delay)
        {
            Title = title;
            Body = body;
            Delay = delay;
        }
    }
}