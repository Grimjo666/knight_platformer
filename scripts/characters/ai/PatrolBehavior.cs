using System;
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
    private bool jumpRequested;
    private const double TurnPause = 1f;
    private float baseRange;
    private float rangeRandomness = 60f;


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
                UpdateWalking();
                break;

            case PatrolState.WaitingBeforeTurn:
                waitTimer -= delta;
                if (waitTimer <= 0)
                {
                    dir *= -1;
                    RecalculatePatrolRange();
                    sensors.FlipRaycastDirection();
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


    private void UpdateWalking()
    {
        float x = character.GlobalPosition.X;
;

        // граница патруля — только если идём в неё
        if ((x <= minX && dir < 0) || (x >= maxX && dir > 0))
        {   
            GD.Print($"Reached patrol boundary at {x:F2}, reversing direction. New range: [{minX:F2}, {maxX:F2}]");
            StartTurnPause();
            return;
        }

        // стена
        if (sensors.IsFacingWall())
        {
            GD.Print("Facing wall, reversing direction.");
            StartTurnPause();
            return;
        }

        // обрыв — только если стоим на земле
        if (character.IsOnFloor() && !sensors.IsOnFloor())
        {
            GD.Print("Facing ledge, reversing direction.");
            StartTurnPause();
            return;
        }

        // ступенька
        if (sensors.IsFacingStage())
        {
            GD.Print("Facing step, requesting jump.");
            jumpRequested = true;
        }
    }

    private void StartTurnPause()
    {
        state = PatrolState.WaitingBeforeTurn;
        waitTimer = TurnPause;
    }


    public Vector2 GetDirection()
    {
        return state == PatrolState.Walking
            ? new Vector2(dir, 0)
            : Vector2.Zero;
    }

    public bool IsJumpPressed()
    {
        if (jumpRequested)
        {
            jumpRequested = false;
            return true;
        }
        return false;
    }
}
