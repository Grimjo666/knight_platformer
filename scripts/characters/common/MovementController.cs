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

    protected CharacterSettings settings;
    protected float currentSpeed;
    protected int previousDirectionX;

    public bool IsKnockbackActive { get; private set; }
    private float knockbackTimer;
    private float knockbackDuration;
    private float knockbackHorizontalDamping;

    protected IInputProvider input;

    public event Action<string> OnMovementStateChanged;
    protected MovementState currentState = MovementState.Idle;
    public float SpeedRatio => Mathf.Clamp(Mathf.Abs(currentSpeed) / settings.MaxSpeed, 0.1f, 1f);

    public MovementController(CharacterBody2D character, IInputProvider input, CharacterSettings settings)
    {
        this.character = character;
        this.input = input;
        this.settings = settings;
    }
    
    public void SetInput(IInputProvider input)
    {
        this.input = input;
    }
    

    public void Update(double delta)
    {
        float deltaF = (float)delta;

        if (IsKnockbackActive)
        {
            UpdateKnockback(deltaF);
            return;
        }

        Vector2 velocity = character.Velocity;

        if (!character.IsOnFloor())
            velocity += character.GetGravity() * deltaF;

        Vector2 direction = input.GetDirection();
        currentSpeed = Mathf.MoveToward(velocity.X, direction.X * settings.MaxSpeed, settings.Acceleration * deltaF);

        MovementState newState = currentState;

        if (input.IsJumpPressed() && character.IsOnFloor())
        {
            velocity.Y = settings.JumpVelocity;
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
                velocity.X = Mathf.MoveToward(velocity.X, 0, settings.Friction * deltaF);
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
               (Mathf.Abs(Mathf.Abs(currentSpeed) - settings.MaxSpeed) < 70f);
    }

    public void ApplyKnockback(Vector2 source)
    {
        float directionX = Mathf.Sign(character.GlobalPosition.X - source.X);
        if (Mathf.Abs(directionX) < Mathf.Epsilon)
            directionX = previousDirectionX != 0 ? previousDirectionX : 1f;

        knockbackDuration = Mathf.Max(0.01f, settings.KnockbackDuration);
        knockbackHorizontalDamping = Mathf.Max(0f, settings.KnockbackDamping);
        knockbackTimer = knockbackDuration;

        IsKnockbackActive = true;
        currentSpeed = 0f;

        character.Velocity = new Vector2(directionX * Mathf.Abs(settings.KnockbackHorizontal), -Mathf.Abs(settings.KnockbackUpward));
    }

    private void UpdateKnockback(float delta)
    {
        Vector2 velocity = character.Velocity;
        velocity += character.GetGravity() * delta;
        velocity.X = Mathf.MoveToward(velocity.X, 0f, knockbackHorizontalDamping * delta);

        character.Velocity = velocity;
        character.MoveAndSlide();

        knockbackTimer -= delta;
        if (knockbackTimer <= 0f)
        {
            IsKnockbackActive = false;
            currentSpeed = character.Velocity.X;
        }
    }
}
