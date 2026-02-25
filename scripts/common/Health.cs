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

    public Health()
    {
        CurrentHealth = MaxHealth;
    }

    
    public void TakeDamage(int damage)
    {
        CurrentHealth -= damage;
        if (CurrentHealth > 0) {
            EventBus.Publish(HealthState.TakeDamage.ToString());
        }
        else
            CurrentHealth = 0;
        
        if (IsDead())
            EventBus.Publish(HealthState.Died.ToString());
    }

    public void Heal(int amount)
    {
        CurrentHealth += amount;
        if (CurrentHealth > MaxHealth)
            CurrentHealth = MaxHealth;
    }

    public bool IsDead() => CurrentHealth <= 0;

    
}