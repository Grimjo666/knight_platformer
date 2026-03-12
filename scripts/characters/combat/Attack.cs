using Godot;

public class Attack
{
    public float Damage { get; private set; }
    public float Knockback { get; private set; }

    public Attack(float damage, float knockback)
    {
        Damage = damage;
        Knockback = knockback;
    }

    public void Apply(BaseCharacter target)
    {
        var health = target.health;
        if (health != null)
            health.TakeDamage(Damage);

        // Простейший knockback
    }
}