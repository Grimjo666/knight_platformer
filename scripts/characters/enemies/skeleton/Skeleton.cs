using Godot;
using System.Linq;

public partial class Skeleton : BaseCharacter
{
	private AnimationController animationController;
	private AIBehaviorController inputProvider;

	[Export]
	public CharacterSettings SkeletonSettings;



	public override void _Ready()
	{

		inputProvider = new AIBehaviorController(this);
		movementController = new MovementController(this, inputProvider, SkeletonSettings);
		InitCharacter(SkeletonSettings, movementController);
		animationController = new AnimationController(this, movementController); 
		movementController.OnMovementStateChanged += animationController.OnStateChanged;
		health.OnHealthStateChanged += animationController.OnStateChanged;

	}

	public override void PhysicsUpdate(double delta)
	{
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
