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
		GetTree().ChangeSceneToFile("res://scenes/core/game_root.tscn");
	}
}
