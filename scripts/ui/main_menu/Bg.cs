using Godot;
using System.Linq;


public partial class Bg : Node2D
{
	private const float BaseParallaxSpeed = 100f;
	private readonly float[] LayerSpeeds = { 0f, 0.5f, 1f, 1.5f, 2f };

	public override void _Ready()
	{
		InitializeSprites();
	}
	
	private void InitializeSprites()
	{
		var parallaxNodes = GetChildren().OfType<Parallax2D>();
		int i = 0;
		var viewportSize = GetViewportRect().Size;


		foreach (var parallax in parallaxNodes)
		{
			var sprite = parallax.GetChildren().OfType<Sprite2D>().FirstOrDefault();
			var textureSize = sprite.Texture.GetSize();
			sprite.Scale = viewportSize / textureSize;
			sprite.Position = Vector2.Zero;

			parallax.RepeatSize = viewportSize;
			if (i < LayerSpeeds.Length)
			{
				parallax.Autoscroll = new Vector2(BaseParallaxSpeed * LayerSpeeds[i], 0);
			}
			i++;
		}
	}

	public override void _Process(double delta)
	{
		
	}
}
