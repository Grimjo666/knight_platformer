using Godot;

public interface IInputProvider
{
    Vector2 GetDirection();
    bool IsJumpPressed();
}
