using System.Collections.Generic;
using Godot;


public abstract partial class BaseCharacter : CharacterBody2D
{
	public HealthController healthController;
	private List<HitBox> hitboxes = new();
	private HurtBox hurtbox;
	public MovementController movementController;
	public AnimationController animationController;

	private bool deathHandled = false;

	// Неуязвимость
	private bool isInvincible = false;
	private float invincibilityTimer = 0f;

	public void InitCharacter(CharacterSettings settings, MovementController movementController, AnimationController animationController)
	{
		healthController = new HealthController(settings.MaxHealth);
		this.movementController = movementController;
		this.animationController = animationController;
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

		if (healthController.IsDead() && !deathHandled)
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

	public override void _Process(double delta)
	{
		animationController.Update();
		HandleFlip();
	}

	public virtual void PhysicsUpdate(double delta) { }

	public void ReceiveHit(AttackData attack, BaseCharacter attacker)
	{
		if (healthController.IsDead() || isInvincible)
			return;

		healthController.TakeDamage(attack.Damage);

		if (!healthController.IsDead())
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

	private void HandleFlip()
	{
		float flipSpeed = movementController.IsKnockbackActive ? movementController.currentSpeed : Velocity.X;
		if (Mathf.Abs(flipSpeed) < 0.01f)
			return;

		var rotationRoot = GetNode<Node2D>("RotationRoot");
		rotationRoot.Scale = new Vector2(flipSpeed < 0 ? -1 : 1, 1);
	}

}
