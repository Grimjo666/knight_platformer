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
    public float MaxHealth;
    public float CurrentHealth;
    public event Action<string> OnHealthStateChanged;
    public event Action<float, float> OnHealthChanged;

    public Health(float maxHealth)
    {
        MaxHealth = maxHealth;
        CurrentHealth = MaxHealth;
    }

    
    public void TakeDamage(float damage)
    {
        CurrentHealth -= damage;
        OnHealthChanged?.Invoke(CurrentHealth, MaxHealth);   
        
        if (CurrentHealth > 0)
            OnHealthStateChanged?.Invoke(HealthState.TakeDamage.ToString());     

        
        else
            CurrentHealth = 0;
        
        if (IsDead())
            OnHealthStateChanged?.Invoke(HealthState.Died.ToString());
    }

    public void Heal(float amount)
    {
        CurrentHealth += amount;
        if (CurrentHealth > MaxHealth)
            CurrentHealth = MaxHealth;
    }

    public bool IsDead() => CurrentHealth <= 0;

    
}
