using Godot;

public interface IInputProvider
{
    Vector2 GetDirection();
    bool IsJumpPressed();

    bool IsAttackPressed();
    bool IsJumpHeld();

}
