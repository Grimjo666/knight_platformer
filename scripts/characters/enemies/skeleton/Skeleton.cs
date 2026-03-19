using Godot;
using System.Linq;

public partial class Skeleton : BaseCharacter
{
	private AIBehaviorController inputProvider;

	[Export]
	public CharacterSettings SkeletonSettings;



	public override void _Ready()
	{

		inputProvider = new AIBehaviorController(this);
		movementController = new MovementController(this, inputProvider, SkeletonSettings);
		animationController = new AnimationController(this, movementController); 
		InitCharacter(SkeletonSettings, movementController, animationController);

		movementController.MovementStateChanged += animationController.OnMovementState;
		healthController.HealthStateChanged += animationController.OnHealthState;


	}

	public override void PhysicsUpdate(double delta)
	{
		inputProvider.Update(delta);
	}

}
