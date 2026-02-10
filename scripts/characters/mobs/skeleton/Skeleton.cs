using Godot;
using System.Linq;

public partial class Skeleton : CharacterBody2D
{

	private SkeletonMovementController movementController;
	private SkeletonAnimationController animationController;
	private AIBehaviorController inputProvider;



	public override void _Ready()
	{
		inputProvider = new AIBehaviorController(this);
		movementController = new SkeletonMovementController(this, inputProvider);
		animationController = new SkeletonAnimationController(this, movementController); 

	}
	public override void _PhysicsProcess(double delta)
	{
		movementController.Update(delta);
		inputProvider.Update(delta);
	}

	public override void _Process(double delta)
	{
		animationController.Update();
		HandleSpritesFlip();
	}


	private void HandleSpritesFlip()
	{
		if (Mathf.Abs(Velocity.X) < 0.001f)
			return;

		var animatedSprite = GetChildren().OfType<AnimatedSprite2D>().FirstOrDefault();
		if (animatedSprite != null)
			animatedSprite.FlipH = Velocity.X < 0;
	}

}
