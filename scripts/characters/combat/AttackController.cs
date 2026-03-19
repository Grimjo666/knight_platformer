using System;
using Godot;


public enum AttackState
{
	BaseAttack
}

public partial class AttackController: Node
{   

	public event Action<AttackState> AttackStateChanged;
	private CharacterBody2D character;
	private HitBox hitBox;
	private IInputProvider input;

	[Export] public AudioStreamPlayer swordSound;


	public void Init(CharacterBody2D character, HitBox hitBox, IInputProvider input)
	{
		this.character = character;
		this.hitBox = hitBox;
		this.input = input;
		hitBox.Disable();
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
		AttackStateChanged?.Invoke(AttackState.BaseAttack);
	}

	public void AttackStart()
	{
		hitBox.Enable();
		swordSound.Play();
	}

	public void AttackEnd()
	{
		hitBox.Disable();
	}
}
