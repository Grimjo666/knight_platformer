using Godot;
using System;

public partial class GameRoot : Node
{
	private Node2D _levelContainer;

	public override void _Ready()
	{
		_levelContainer = GetNode<Node2D>("LevelContainer");
		LoadLevel("res://scenes/levels/forest_level.tscn");
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
}
