using Godot;
using System.Linq;

public partial class Skeleton : BaseEnemy
{
	private MovementController movementController;
	private AnimationController animationController;
	private AIBehaviorController inputProvider;
	private Health health;

	[Export]
	public CharacterSettings SkeletonSettings;



	public override void _Ready()
	{
		inputProvider = new AIBehaviorController(this);
		movementController = new MovementController(this, inputProvider, SkeletonSettings);
		animationController = new AnimationController(this, movementController); 
		health = new Health();
		movementController.OnMovementStateChanged += animationController.OnStateChanged;
		health.OnHealthStateChanged += animationController.OnStateChanged;

	}

	public override void _PhysicsProcess(double delta)
	{
		movementController.Update(delta);
		inputProvider.Update(delta);
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

	// public void _on_hurt_box_area_entered(Area2D area)
	// {
	// 	if (area.IsInGroup("mobs_hitbox"))
	// 	{
	// 		var player = area.Owner as Player;
	// 		if (player != null)
	// 		{


	// 			health.TakeDamage(player.CollisionDamage);
	// 			if (!health.IsDead())
	// 			{
	// 				movementController.ApplyKnockback(player.GlobalPosition);
	// 			}
	// 		}
	// 	}
	// }

}
