using Godot;

[GlobalClass]
public partial class CharacterSettings : Resource
{
	[ExportGroup("Movement")]
	[Export] public float MaxSpeed = 250f;
	[Export] public float Acceleration = 700f;
	[Export] public float Friction = 1200f;
	[Export] public float JumpVelocity = -400f;

	[ExportGroup("Knockback")]
	[Export] public float KnockbackHorizontal = 160f;
	[Export] public float KnockbackUpward = 180f;
	[Export] public float KnockbackDuration = 0.5f;
	[Export] public float KnockbackDamping = 500f;
}
