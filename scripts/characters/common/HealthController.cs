using System;

public enum HealthState
{
    Died,
    TakeDamage,
    Heal,
}


public class HealthController
{
    public float MaxHealth;
    public float CurrentHealth;
    public event Action<HealthState> HealthStateChanged;
    public event Action<float, float> HealthChanged;

    public HealthController(float maxHealth)
    {
        MaxHealth = maxHealth;
        CurrentHealth = MaxHealth;
    }

    
    public void TakeDamage(float damage)
    {
        CurrentHealth -= damage;
        HealthChanged?.Invoke(CurrentHealth, MaxHealth);   
        
        if (CurrentHealth > 0)
            HealthStateChanged?.Invoke(HealthState.TakeDamage);     

        
        else
            CurrentHealth = 0;
        
        if (IsDead())
            HealthStateChanged?.Invoke(HealthState.Died);
    }

    public void Heal(float amount)
    {
        CurrentHealth += amount;
        if (CurrentHealth > MaxHealth)
            CurrentHealth = MaxHealth;
    }

    public bool IsDead() => CurrentHealth <= 0;
}
