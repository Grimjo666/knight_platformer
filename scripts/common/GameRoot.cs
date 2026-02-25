using Godot;
using System;

public partial class GameRoot : Node
{
	private Node2D _levelContainer;
	private TextureProgressBar healthBar;

	public override void _Ready()
	{
		_levelContainer = GetNode<Node2D>("LevelContainer");
		LoadLevel("res://scenes/levels/forest_level.tscn");
		healthBar = GetNode<TextureProgressBar>("CanvasLayer/HealthBar");
		EventBus.HealthChanged += ChangeHealthBar;
	}

	public void LoadLevel(string path)
	{
		// Убираем предыдущий уровень
		foreach (Node child in _levelContainer.GetChildren())
			child.QueueFree();

		// Загружаем новый
		var scene = GD.Load<PackedScene>(path);
		var instance = scene.Instantiate<Node2D>();
		_levelContainer.AddChild(instance);
	}

	public void ChangeHealthBar(int currentHealth, int maxHealth)
	{
		healthBar.Value = (float)currentHealth / maxHealth * 100f;
	}
}
