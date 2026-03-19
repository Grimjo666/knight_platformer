using Godot;


enum PatrolState
{
    Walking,
    WaitingBeforeTurn
}

public class PatrolBehavior : IAIBehavior
{
    private CharacterBody2D character;
    private AICharacterSensors sensors;

    private float minX, maxX;
    private int dir = 1;
    private PatrolState state = PatrolState.Walking;
    private double waitTimer = 0f;
    private const double TurnPause = 1f;
    private float baseRange;
    private float rangeRandomness = 60f;

    // Прыжок
    private bool jumpRequested;
    private bool jumpPending; // флаг, чтобы не прыгать каждый кадр
    private double jumpHoldTimer = 0;
    private const double JumpHoldDuration = 0.2;

    public PatrolBehavior(CharacterBody2D character, float range, AICharacterSensors sensors)
    {
        this.character = character;
        this.sensors = sensors;
        this.baseRange = range;
        RecalculatePatrolRange();
    }

    public void Update(double delta)
    {
        switch (state)
        {
            case PatrolState.Walking:
                UpdateWalking(delta);
                break;

            case PatrolState.WaitingBeforeTurn:
                waitTimer -= delta;
                if (waitTimer <= 0)
                {
                    dir *= -1;
                    RecalculatePatrolRange();
                    state = PatrolState.Walking;
                }
                break;
        }
    }

    private void RecalculatePatrolRange()
    {
        float center = character.GlobalPosition.X;
        float randomOffset = (float)GD.RandRange(-rangeRandomness, rangeRandomness);
        float finalRange = baseRange + randomOffset;
        minX = center - finalRange;
        maxX = center + finalRange;
    }

    private void UpdateWalking(double delta)
    {
        // Таймер удержания для полупрыжка
        if (jumpHoldTimer > 0)
        {
            jumpHoldTimer -= delta;
        }
        else
        {
            jumpRequested = false;
        }

        float x = character.GlobalPosition.X;

        // Поворот на границах патруля
        if ((x <= minX && dir < 0) || (x >= maxX && dir > 0))
        {
            StartTurnPause();
            return;
        }

        // Стена
        if (sensors.IsFacingWall())
        {
            StartTurnPause();
            return;
        }

        // Обрыв
        if (character.IsOnFloor() && !sensors.IsOnFloor())
        {
            StartTurnPause();
            return;
        }

        // Ступенька — прыгаем один раз
        if (sensors.IsFacingStage() && !jumpPending && character.IsOnFloor())
        {
            jumpRequested = true;
            jumpPending = true;
            jumpHoldTimer = JumpHoldDuration; // старт полупрыжка
        }

        // Сброс флага, когда персонаж отрывается от земли
        if (!character.IsOnFloor())
        {
            jumpPending = false;
        }
    }

    private void StartTurnPause()
    {
        state = PatrolState.WaitingBeforeTurn;
        waitTimer = TurnPause;
    }

    public Vector2 GetDirection()
    {
        return state == PatrolState.Walking ? new Vector2(dir, 0) : Vector2.Zero;
    }

    public bool IsJumpPressed()
    {
        if (jumpRequested)
        {
            return true;
        }
        return false;
    }

    public bool IsJumpHeld()
    {
        return jumpHoldTimer > 0;
    }
}