using Godot;

using System;

public partial class HitBox : Area2D
{
	[Export] public AttackData AttackData;
	public void Enable() => Monitoring = true;
	public void Disable() => Monitoring = false;
	public BaseCharacter OwnerCharacter;

	public override void _Ready()
	{
		Monitoring = true;
		OwnerCharacter = this.Owner as BaseCharacter;
		OwnerCharacter.RegisterHitbox(this);

	}

	public void _on_area_entered(Area2D area)
	{
		// Проверяем, что это hurtbox и не свой
		if (area is HurtBox hurt)
		{	
			// Игрок бьет врага
			if (OwnerCharacter is Player && area.IsInGroup("enemy_hurtbox"))
			{
				hurt.ReceiveHit(AttackData, OwnerCharacter);
			}
			// Враг бьет игрока
			else if (OwnerCharacter is Enemy && area.IsInGroup("player_hurtbox"))
			{	
				hurt.ReceiveHit(AttackData, OwnerCharacter);
			}
		}
	}
}
