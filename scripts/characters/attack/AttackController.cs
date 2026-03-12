using System;
using Godot;


public enum AttackState
{
    Idle,
    BaseAttack,
}

public sealed class AttackController
{   

    public event Action<string> OnAttackStateChanged;
    private CharacterBody2D character;
    private HitBox hitBox;
    private IInputProvider input;

    public AttackController(CharacterBody2D character, HitBox hitBox, IInputProvider input)
    {
        this.character = character;
        this.hitBox = hitBox;
        this.input = input;
    }

    public void Update(double delta)
    {
        if (input.IsAttackPressed())
        {
            PerformAttack();
        }
    }

    private void PerformAttack()
    {
        OnAttackStateChanged?.Invoke(AttackState.BaseAttack.ToString());
        hitBox.Enable();
    }

}
