using Godot;
using System;
using System.Linq;

public class PlayerInputProvider : IInputProvider
{
	public Vector2 GetDirection()
	{
		return Input.GetVector("ui_left", "ui_right", "ui_up", "ui_down");
	}

	public bool IsJumpPressed()
	{
		return Input.IsActionJustPressed("ui_accept");
	}
	public bool IsAttackPressed()
	{
		return Input.IsActionJustPressed("attack");
	}

	public bool IsJumpHeld()
	{
		return Input.IsActionPressed("ui_accept");
	}
}

public partial class Player : BaseCharacter
{

	private AttackController attackController;
	[Export] public HitBox SwordHitBox;

	[Export]
	public CharacterSettings PlayerSettings;

	public override void _Ready()
	{	
		var inputProvider = new PlayerInputProvider();
		movementController = new MovementController(this, inputProvider, PlayerSettings);
		animationController = new AnimationController(this, movementController); 
		InitCharacter(PlayerSettings, movementController, animationController);

		attackController = GetNode<AttackController>("AttackController");
		attackController.Init(this, SwordHitBox, inputProvider);

		// Проброс событий в анимационный контроллер
		healthController.HealthStateChanged += animationController.OnHealthState;
		movementController.MovementStateChanged += animationController.OnMovementState;
		attackController.AttackStateChanged += animationController.OnAttackState;

		healthController.HealthChanged += OnHealthChanged;

	}

	public override void PhysicsUpdate(double delta)
	{
		attackController.Update(delta);
	}


	private void OnHealthChanged(float currentHealth, float maxHealth)
	{
		EventBus.PlayerHealthChanged(currentHealth, maxHealth);
		EnableInvincibility(2.5f);
	}

}
