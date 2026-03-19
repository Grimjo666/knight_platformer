using System;
using Godot;


public enum MovementState
{
    Idle,
    Run,
    Jump,
    TurnAround
}


public class MovementController
{
    protected CharacterBody2D character;

    protected CharacterSettings settings;
    public float currentSpeed;
    protected int previousDirectionX;

    public bool IsKnockbackActive { get; private set; }
    private float knockbackTimer;
    private float knockbackDuration;
    private float knockbackHorizontalDamping;

    // Настройки окна прыжков
    private float coyoteTimer, jumpBufferTimer = 0f;
    private float coyoteTime = 0.12f;
    private float jumpBufferTime = 0.12f;

    public float JumpCutMultiplier = 0.5f;
    private bool jumpCutApplied = false;


    // Настройки knockback
    private float KnockbackDamping = 500f;

    protected IInputProvider input;

    public event Action<MovementState> MovementStateChanged;
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

        ApplyGravity(ref velocity, deltaF);

        Vector2 direction = input.GetDirection();
        UpdateJumpTimers(deltaF);

        MovementState newState = HandleJump(ref velocity);

        ApplyHalfJump(ref velocity);

        if (newState == currentState)
            newState = HandleGroundMovement(ref velocity, direction, deltaF);

        UpdateState(newState);

        character.Velocity = velocity;
        UpdateDirection(direction);
        character.MoveAndSlide();
    }

    private void UpdateJumpTimers(float delta)
    {
        if (character.IsOnFloor())
            coyoteTimer = coyoteTime;
        else
            coyoteTimer -= delta;

        if (input.IsJumpPressed())
            jumpBufferTimer = jumpBufferTime;
        else
            jumpBufferTimer -= delta;
    }

    private MovementState HandleJump(ref Vector2 velocity)
    {
        if (CanJump())
        {
            velocity.Y = settings.JumpVelocity;

            jumpBufferTimer = 0f;
            coyoteTimer = 0f;

            return MovementState.Jump;
        }

        return currentState;
    }

    private MovementState HandleGroundMovement(ref Vector2 velocity, Vector2 direction, float delta)
    {
        currentSpeed = Mathf.MoveToward(
            velocity.X,
            direction.X * settings.MaxSpeed,
            settings.Acceleration * delta
        );

        if (!character.IsOnFloor())
        {
            velocity.X = currentSpeed;
            return currentState;
        }

        if (CheckIsTurnAround(direction.X))
            return MovementState.TurnAround;

        if (Mathf.Abs(currentSpeed) > 0.01f)
        {
            velocity.X = currentSpeed;
            return MovementState.Run;
        }

        velocity.X = Mathf.MoveToward(velocity.X, 0, settings.Friction * delta);
        return MovementState.Idle;
    }

    private void UpdateState(MovementState newState)
    {
        if (newState == currentState)
            return;

        currentState = newState;
        MovementStateChanged?.Invoke(newState);
    }

    private void UpdateDirection(Vector2 direction)
    {
        if (direction.X != 0)
            previousDirectionX = (int)direction.X;
    }

    private void ApplyGravity(ref Vector2 velocity, float delta)
    {
        if (!character.IsOnFloor())
            velocity += character.GetGravity() * delta;
    }

    private bool CanJump()
    {
        return jumpBufferTimer > 0f && coyoteTimer > 0f;
    }

    private void ApplyHalfJump(ref Vector2 velocity)
    {
        if (!jumpCutApplied && !input.IsJumpHeld() && velocity.Y < 0)
        {
            velocity.Y *= JumpCutMultiplier;
            jumpCutApplied = true;
        }

        if (character.IsOnFloor())
            jumpCutApplied = false;
    }

    protected bool CheckIsTurnAround(float directionX)
    {
        return directionX != 0 && previousDirectionX != 0 &&
               Mathf.Sign(directionX) != Mathf.Sign(previousDirectionX) &&
               (Mathf.Abs(Mathf.Abs(currentSpeed) - settings.MaxSpeed) < 70f);
    }

    public void ApplyKnockback(Vector2 source, AttackData attackData)
    {
        float directionX = Mathf.Sign(character.GlobalPosition.X - source.X);
        if (Mathf.Abs(directionX) < Mathf.Epsilon)
            directionX = previousDirectionX != 0 ? previousDirectionX : 1f;

        // горизонтальная и вертикальная сила отбрасывания с учётом веса
        float horizontalKnockback = attackData.KnockbackPower / settings.Weight;
        float upwardKnockback = (attackData.KnockbackPower / settings.Weight) * 0.8f; // вверх чуть меньше

        knockbackDuration = Mathf.Max(0.01f, attackData.KnockbackDuration);
        knockbackHorizontalDamping = Mathf.Max(0f, KnockbackDamping);
        knockbackTimer = knockbackDuration;

        IsKnockbackActive = true;
        currentSpeed = 0f;

        character.Velocity = new Vector2(directionX * horizontalKnockback, -upwardKnockback);
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
