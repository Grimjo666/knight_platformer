using Godot;




public class SkeletonMovementController : BaseMovementController
{   
    private bool wantsJump = false;

    public SkeletonMovementController(CharacterBody2D character, IInputProvider input) 
        : base(character, input)
    {

        maxSpeed = 80;
        acceleration = 500f;
        friction = 1000f;
        jumpVelocity = -400f;

    }

}
