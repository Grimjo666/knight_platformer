using Godot;

[GlobalClass]
public partial class CharacterSettings : Resource
{
	[ExportGroup("Movement")]
	[Export] public float MaxSpeed = 250f;
	[Export] public float Acceleration = 700f;
	[Export] public float Friction = 1200f;
	[Export] public float JumpVelocity = -400f;

	[ExportGroup("Physics")]
	[Export] public float Weight = 1f;

	[ExportGroup("Health")]
	[Export] public float MaxHealth = 100f;
}
