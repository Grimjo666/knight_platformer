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
}

public partial class Player : CharacterBody2D
{

	private PlayerMovementController movementController;
	private PlayerAnimationController animationController;



	public override void _Ready()
	{
		var inputProvider = new PlayerInputProvider();
		movementController = new PlayerMovementController(this, inputProvider);
		animationController = new PlayerAnimationController(this, movementController); 

	}
	public override void _PhysicsProcess(double delta)
	{
		movementController.Update(delta);
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
