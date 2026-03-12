using Godot;

public partial class HurtBox : Area2D
{
	public BaseCharacter OwnerCharacter;

	public void Enable() => Monitorable = true;
	public void Disable() => Monitorable = false;

	public override void _Ready()
	{
		Monitorable = true;
		OwnerCharacter = this.Owner as BaseCharacter;
		OwnerCharacter.RegisterHurtbox(this);
	}

	public void ReceiveHit(AttackData attack, BaseCharacter attacker)
	{
		OwnerCharacter?.ReceiveHit(attack, attacker);
	}
}
