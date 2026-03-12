using System;
using Godot;

public static class EventBus
{
    public static event Action<float, float> HealthChanged;

    public static void PlayerHealthChanged(float currentHealth, float maxHealth)
    {
        GD.Print($"Событие: здоровье изменилось. Текущее: {currentHealth}, Макс: {maxHealth}");
        HealthChanged?.Invoke(currentHealth, maxHealth);
    }
}
