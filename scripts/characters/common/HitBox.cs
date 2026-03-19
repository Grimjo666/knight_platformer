using System.Collections.Generic;
using Godot;


public partial class HitBox : Area2D
{
	[Export] public AttackData AttackData;
	private HashSet<HurtBox> targets = new();
	private Dictionary<HurtBox, float> hitCooldowns = new();
	private float hitInterval = 0.3f;
	public void Enable() => Monitoring = true;
	public void Disable() => Monitoring = false;
	public BaseCharacter OwnerCharacter;

	public override void _Ready()
	{
		Monitoring = true;
		OwnerCharacter = this.Owner as BaseCharacter;
		OwnerCharacter.RegisterHitbox(this);

	}

	public override void _PhysicsProcess(double delta)
	{
		ApplyHit(delta);
	}

	private void OnAreaEntered(Area2D area)
	{
		if (area is not HurtBox hurt)
			return;

		if (!IsValidTarget(area))
			return;

		targets.Add(hurt);
	}

	private void OnAreaExited(Area2D area)
	{
		if (area is HurtBox hurt)
		{
			targets.Remove(hurt);
			hitCooldowns.Remove(hurt);
		}
	}

	private bool IsValidTarget(Area2D area)
	{
		if (OwnerCharacter is Player)
			return area.IsInGroup("enemy_hurtbox");

		return area.IsInGroup("player_hurtbox");
	}

	public void ApplyHit(double delta)
	{
		foreach (var hurt in targets)
		{
			if (!hitCooldowns.ContainsKey(hurt))
				hitCooldowns[hurt] = 0;

			hitCooldowns[hurt] -= (float)delta;

			if (hitCooldowns[hurt] <= 0)
			{
				hurt.ReceiveHit(AttackData, OwnerCharacter);
				hitCooldowns[hurt] = hitInterval;
			}
		}
	}
}
