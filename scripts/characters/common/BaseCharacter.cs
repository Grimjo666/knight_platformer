using System.Collections.Generic;
using Godot;


public abstract partial class BaseCharacter : CharacterBody2D
{
	public Health health;
	private List<HitBox> hitboxes = new();
	private HurtBox hurtbox;
	public MovementController movementController;

	private bool deathHandled = false;

	// Неуязвимость
	private bool isInvincible = false;
	private float invincibilityTimer = 0f;

	public void InitCharacter(CharacterSettings settings, MovementController movementController)
	{
		health = new Health(settings.MaxHealth);
		this.movementController = movementController;
	}

	public override void _PhysicsProcess(double delta)
	{
		// Обновление таймера неуязвимости
		if (isInvincible)
		{
			invincibilityTimer -= (float)delta;
			if (invincibilityTimer <= 0f)
				isInvincible = false;
		}

		if (health.IsDead() && !deathHandled)
		{
			GD.Print($"{Name} умер.");
			movementController.SetInput(NullInput.Input);
			DisableHitboxes();
			DisableHurtbox();
			deathHandled = true;
		}

		movementController.Update(delta);
		PhysicsUpdate(delta);
	}

	public virtual void PhysicsUpdate(double delta) { }

	public void ReceiveHit(AttackData attack, BaseCharacter attacker)
	{
		if (health.IsDead() || isInvincible)
			return;

		health.TakeDamage(attack.Damage);

		if (!health.IsDead())
			movementController.ApplyKnockback(attacker.GlobalPosition, attack);

	}

	public void EnableInvincibility(float duration)
	{
		isInvincible = true;
		invincibilityTimer = duration;
	}

	private void DisableHitboxes()
	{
		foreach (var hitbox in hitboxes)
			hitbox.Disable();
	}

	private void DisableHurtbox() => hurtbox?.Disable();

	public void RegisterHitbox(HitBox hitbox) => hitboxes.Add(hitbox);
	public void RegisterHurtbox(HurtBox hurtbox) => this.hurtbox = hurtbox;
}
