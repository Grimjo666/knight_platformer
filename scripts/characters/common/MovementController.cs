using System;
using Godot;


public enum MovementState
{
    Idle,
    Run,
    Jump,
    TurnAround,
    Walk
}


public class MovementController
{
    protected CharacterBody2D character;

    // Определение характеристик движения
    protected float maxSpeed;
    protected float acceleration;
    protected float friction;
    protected float jumpVelocity;
    protected float currentSpeed;
    protected int previousDirectionX;

    protected IInputProvider input;

    public event Action<string> OnMovementStateChanged;
    protected MovementState currentState = MovementState.Idle;



    public MovementController(CharacterBody2D character, IInputProvider input, float maxSpeed, float acceleration, float friction, float jumpVelocity)
    {
        this.character = character;
        this.input = input;
        this.maxSpeed = maxSpeed;
        this.acceleration = acceleration;
        this.friction = friction;
        this.jumpVelocity = jumpVelocity;
    }


    public float SpeedRatio => Mathf.Clamp(Mathf.Abs(currentSpeed) / maxSpeed, 0.1f, 1f);

    public void Update(double delta)
    {
        Vector2 velocity = character.Velocity;

        if (!character.IsOnFloor())
            velocity += character.GetGravity() * (float)delta;

        Vector2 direction = input.GetDirection();
        currentSpeed = Mathf.MoveToward(velocity.X, direction.X * maxSpeed, acceleration * (float)delta);

        MovementState newState = currentState;

        if (input.IsJumpPressed() && character.IsOnFloor())
        {
            velocity.Y = jumpVelocity;
            newState = MovementState.Jump;
        }
        else if (character.IsOnFloor())
        {
            if (CheckIsTurnAround(direction.X))
                newState = MovementState.TurnAround;
            else if (Mathf.Abs(currentSpeed) > 0.01f)
            {
                newState = MovementState.Run;
                velocity.X = currentSpeed;
            }
            else
            {
                newState = MovementState.Idle;
                velocity.X = Mathf.MoveToward(velocity.X, 0, friction * (float)delta);
            }
        }
        else
        {
            velocity.X = currentSpeed;
        }

        if (newState != currentState)
        {
            currentState = newState;
            OnMovementStateChanged?.Invoke(newState.ToString());
        }

        character.Velocity = velocity;
        if (direction.X != 0) previousDirectionX = (int)direction.X;

        character.MoveAndSlide();
    }

    protected bool CheckIsTurnAround(float directionX)
    {
        return directionX != 0 && previousDirectionX != 0 &&
               Mathf.Sign(directionX) != Mathf.Sign(previousDirectionX) &&
               (Mathf.Abs(Mathf.Abs(currentSpeed) - maxSpeed) < 70f);
    }
}
