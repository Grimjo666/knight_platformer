using Godot;
using System;

public partial class HitBox : Area2D
{
	[Export] public int Damage = 10;
	[Export] public float Lifetime = 0.15f;

	public override void _Ready()
	{
		GetTree().CreateTimer(Lifetime).Timeout += QueueFree;
	}
}
