using Godot;
using System.Linq;

public partial class Skeleton : BaseEnemy
{
	private MovementController movementController;
	private AnimationController animationController;
	private AIBehaviorController inputProvider;

	[Export] public float MaxSpeed = 80f;
	[Export] public float Acceleration = 500f;
	[Export] public float Friction = 1000f;
	[Export] public float JumpVelocity = -400f;



	public override void _Ready()
	{
		inputProvider = new AIBehaviorController(this);
		movementController = new MovementController(this, inputProvider, MaxSpeed, Acceleration, Friction, JumpVelocity);
		animationController = new AnimationController(this, movementController); 

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


}
