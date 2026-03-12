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

	private AnimationController animationController;
	private AttackController attackController;
	[Export] public HitBox SwordHitBox;

	[Export]
	public CharacterSettings PlayerSettings;

	public override void _Ready()
	{	
		var inputProvider = new PlayerInputProvider();
		movementController = new MovementController(this, inputProvider, PlayerSettings);
		InitCharacter(PlayerSettings, movementController);
		animationController = new AnimationController(this, movementController); 
		attackController = new AttackController(this, SwordHitBox, inputProvider);

		// Проброс событий в анимационный контроллер
		health.OnHealthStateChanged += animationController.OnStateChanged;
		movementController.OnMovementStateChanged += animationController.OnStateChanged;
		attackController.OnAttackStateChanged += animationController.OnStateChanged;

		health.OnHealthChanged += OnHealthChanged;

		SwordHitBox.Disable(); // Деактивируем хитбокс атаки по умолчанию

	}

	public override void PhysicsUpdate(double delta)
	{
		attackController.Update(delta);
	}

	public override void _Process(double delta)
	{
		animationController.Update();
		HandleFlip();
	}

	private void OnHealthChanged(float currentHealth, float maxHealth)
	{
		EventBus.PlayerHealthChanged(currentHealth, maxHealth);
		EnableInvincibility(2.5f);
	}

	private void HandleFlip()
	{
		if (Mathf.Abs(Velocity.X) < 0.001f)
			return;

		var rotationRoot = GetNode<Node2D>("RotationRoot");
		rotationRoot.Scale = new Vector2(Velocity.X < 0 ? -1 : 1, 1);
	}


}
