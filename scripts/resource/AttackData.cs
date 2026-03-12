using Godot;

[GlobalClass]
public partial class AttackData : Resource
{
	[Export] public float Damage = 20;
	[Export] public float Cooldown = 0.4f;
	[Export] public float KnockbackPower = 20f;
	[Export] public float KnockbackDuration = 0.3f;
}
