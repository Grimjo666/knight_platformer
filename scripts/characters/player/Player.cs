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
}

public partial class Player : CharacterBody2D
{

	private MovementController movementController;
	private AnimationController animationController;
	private AttackController attackController;
	private Health health;
	[Export] public PackedScene AttackScene;


	[Export] public float MaxSpeed = 250f;
	[Export] public float Acceleration = 700f;
	[Export] public float Friction = 1200f;
	[Export] public float JumpVelocity = -400f;

	public override void _Ready()
	{	
		var inputProvider = new PlayerInputProvider();
		health = new Health();
		movementController = new MovementController(this, inputProvider, MaxSpeed, Acceleration, Friction, JumpVelocity);
		animationController = new AnimationController(this, movementController); 
		attackController = new AttackController(this, AttackScene, inputProvider);

	}
	public override void _PhysicsProcess(double delta)
	{
		movementController.Update(delta);
		attackController.Update(delta);
	}

	public override void _Process(double delta)
	{
		animationController.Update();
		HandleFlip();
	}


	private void HandleFlip()
	{
		if (Mathf.Abs(Velocity.X) < 0.001f)
			return;

		var rotationRoot = GetNode<Node2D>("RotationRoot");
		rotationRoot.Scale = new Vector2(Velocity.X < 0 ? -1 : 1, 1);
	}
	

	public void _on_hurt_box_area_entered(Area2D area)
	{
		if (area.IsInGroup("mobs_hitbox"))
		{
			var enemy = area.Owner as BaseEnemy;
			if (enemy != null)
				health.TakeDamage(enemy.CollisionDamage);
			{
				
				GD.Print($"Player took {enemy.CollisionDamage} damage, health now {health.CurrentHealth}");
			}
		}
	}

}
