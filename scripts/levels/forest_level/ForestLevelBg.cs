using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class ForestLevelBg : Node2D
{
	private const int DuplicateCount = 4;
	private Sprite2D[] sprites;

	public override void _Ready()
	{

		InitializeSprites();
	}

	private void InitializeSprites()
	{
		var viewportSize = GetViewportRect().Size;
		var originalSprites = GetChildren().OfType<Sprite2D>().ToArray();
		var allSprites = new List<Sprite2D>(originalSprites.Length * (DuplicateCount + 1));

		foreach (var sprite in originalSprites)
		{
			if (sprite.Texture == null)
			{
				continue;
			}

			var textureSize = sprite.Texture.GetSize();
			sprite.Scale = viewportSize / textureSize;
			sprite.Position = Vector2.Zero;
			allSprites.Add(sprite);

			for (int i = 1; i <= DuplicateCount; i++)
			{
				var copy = (Sprite2D)sprite.Duplicate();
				AddChild(copy);
				copy.Scale = sprite.Scale;
				copy.Position = new Vector2(viewportSize.X * i, 0);
				allSprites.Add(copy);
			}
		}

		sprites = allSprites.ToArray();
		Console.WriteLine(sprites.Length);
	}
}
