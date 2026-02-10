using System;
using Godot;


public enum BaseCharacterState
{
    Idle,
    Running,
    Jumping,
    TurnedAround
}


public abstract class BaseMovementController
{
    protected CharacterBody2D character;

    // Определение характеристик движения
    protected float maxSpeed;
    protected float acceleration;
    protected float friction;
    protected float jumpVelocity;
    protected IInputProvider input;
    protected float currentSpeed;
    protected int previousDirectionX;
    
    protected BaseCharacterState currentState = BaseCharacterState.Idle;

    public event Action<BaseCharacterState> AnimationRequested;


    protected BaseMovementController(CharacterBody2D character, IInputProvider input)
    {
        this.character = character;
        this.input = input;
    }


    public float SpeedRatio => Mathf.Clamp(Mathf.Abs(currentSpeed) / maxSpeed, 0.1f, 1f);

    public void Update(double delta)
    {
        Vector2 velocity = character.Velocity;

        if (!character.IsOnFloor())
            velocity += character.GetGravity() * (float)delta;

        Vector2 direction = input.GetDirection();
        currentSpeed = Mathf.MoveToward(velocity.X, direction.X * maxSpeed, acceleration * (float)delta);

        BaseCharacterState newState = currentState;

        if (input.IsJumpPressed() && character.IsOnFloor())
        {
            velocity.Y = jumpVelocity;
            newState = BaseCharacterState.Jumping;
        }
        else if (character.IsOnFloor())
        {
            if (CheckIsTurnAround(direction.X))
                newState = BaseCharacterState.TurnedAround;
            else if (Mathf.Abs(currentSpeed) > 0.01f)
            {
                newState = BaseCharacterState.Running;
                velocity.X = currentSpeed;
            }
            else
            {
                newState = BaseCharacterState.Idle;
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
            AnimationRequested?.Invoke(newState);
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
