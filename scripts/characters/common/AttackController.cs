using System;
using Godot;

public enum AttackPhase
{
    Idle,
    Windup,
    Active,
    Recovery
}

public sealed class AttackController
{
    public event Action<AttackPhase> AttackPhaseChanged;

    private readonly CharacterBody2D owner;
    private readonly Node2D attackOrigin;
    private readonly PackedScene hitBoxScene;
    private readonly IInputProvider input;
    private AttackPhase phase = AttackPhase.Idle;
    private float timer;

    // настройки одной атаки
    private readonly float windup;
    private readonly float active;
    private readonly float recovery;

    public AttackController(CharacterBody2D owner, PackedScene hitBoxScene, IInputProvider input,
        float windup = 0.08f, float active = 0.12f, float recovery = 0.18f)
    {   
        this.input = input;
        this.owner = owner;
        this.attackOrigin = owner.GetNode<Node2D>("RotationRoot/AttackOrigin");
        this.hitBoxScene = hitBoxScene;
        this.windup = windup;
        this.active = active;
        this.recovery = recovery;
    }

    public bool IsAttacking => phase != AttackPhase.Idle;

    public void Update(double delta)
    {
        if (phase == AttackPhase.Idle)
        {
            if (input.IsAttackPressed()) StartAttack();
            return;
        }

        timer -= (float)delta;
        if (timer > 0f) return;

        switch (phase)
        {
            case AttackPhase.Windup:
                SpawnHitBox();
                SetPhase(AttackPhase.Active, active);
                break;
            case AttackPhase.Active:
                SetPhase(AttackPhase.Recovery, recovery);
                break;
            case AttackPhase.Recovery:
                SetPhase(AttackPhase.Idle, 0f);
                break;
        }
    }

    private void StartAttack()
    {
        SetPhase(AttackPhase.Windup, windup);
    }

    private void SetPhase(AttackPhase newPhase, float newTimer)
    {
        phase = newPhase;
        timer = newTimer;
        AttackPhaseChanged?.Invoke(phase);
    }

    private void SpawnHitBox()
    {
        if (hitBoxScene == null) return;
        var hitBox = hitBoxScene.Instantiate<HitBox>();
        hitBox.GlobalPosition = attackOrigin.GetNode<Node2D>("AttackPoint").GlobalPosition;
        owner.GetTree().CurrentScene.AddChild(hitBox);
    }
}
