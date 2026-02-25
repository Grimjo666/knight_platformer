using System;
using Godot;

public static class EventBus
{
    public static event Action<string> StateChanged;
    public static event Action<int, int> HealthChanged;

    public static void PublishCharacterStateChanged(string stateName)
    {
        StateChanged?.Invoke(stateName);
    }

    public static void PublishHealthChanged(int currentHealth, int maxHealth)
    {
        HealthChanged?.Invoke(currentHealth, maxHealth);
    }
}
