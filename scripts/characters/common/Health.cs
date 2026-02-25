using System;
using Godot;

public enum HealthState
{
    Died,
    TakeDamage,
    Heal,
}


public class Health
{
    int MaxHealth = 100;
    public int CurrentHealth;
    public event Action<string> OnHealthStateChanged;

    public Health()
    {
        CurrentHealth = MaxHealth;
    }

    
    public void TakeDamage(int damage)
    {
        CurrentHealth -= damage;
        EventBus.PublishHealthChanged(CurrentHealth, MaxHealth);

        if (CurrentHealth > 0)
            OnHealthStateChanged?.Invoke(HealthState.TakeDamage.ToString());        
        
        else
            CurrentHealth = 0;
        
        if (IsDead())
            OnHealthStateChanged?.Invoke(HealthState.Died.ToString());
    }

    public void Heal(int amount)
    {
        CurrentHealth += amount;
        if (CurrentHealth > MaxHealth)
            CurrentHealth = MaxHealth;
    }

    public bool IsDead() => CurrentHealth <= 0;

    
}
