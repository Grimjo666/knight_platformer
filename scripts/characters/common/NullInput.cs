using Godot;

public class NullInputProvider : IInputProvider
{
    public Vector2 GetDirection() => Vector2.Zero;

    public bool IsJumpPressed() => false;

    public bool IsAttackPressed() => false;
}

public static class NullInput
{
    public static readonly IInputProvider Input = new NullInputProvider();
}