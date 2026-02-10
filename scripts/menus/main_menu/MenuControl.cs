using Godot;
using System;

public partial class MenuControl : Control
{
	private void _on_quit_btn_pressed()
	{
		GetTree().Quit();
	}

	private void _on_play_btn_pressed()
	{
		GetTree().ChangeSceneToFile("res://scense/levels/forest_level.tscn");
	}
}
