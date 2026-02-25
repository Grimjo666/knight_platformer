using System;

public static class EventBus
{
    public static event Action<string> StateChanged;

    public static void Publish(string eventName)
    {
        StateChanged?.Invoke(eventName);
    }
}