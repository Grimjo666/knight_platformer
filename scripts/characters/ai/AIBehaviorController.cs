using Godot;

public interface IAIBehavior
{
    void Update(double delta);

    Vector2 GetDirection();
    bool IsJumpPressed();
    bool IsJumpHeld();
}


public class AIBehaviorController : IInputProvider
{   
    protected CharacterBody2D character;
    private IAIBehavior current;
    protected AICharacterSensors sensors;

    public AIBehaviorController(CharacterBody2D character)
    {
        this.character = character;
        this.sensors = new AICharacterSensors(character);
        current = new PatrolBehavior(character, 300f, sensors);
        
    }    
    public void SetBehavior(IAIBehavior behavior)
    {
        if (current == behavior) return;
        current = behavior;
    }

    public void Update(double delta)
    {
        current?.Update(delta);
    }

    public Vector2 GetDirection()
        => current?.GetDirection() ?? Vector2.Zero;

    public bool IsJumpPressed()
        => current?.IsJumpPressed() ?? false;

    public bool IsAttackPressed()
        => false; // AI does not attack for now

    public bool IsJumpHeld()
        => current?.IsJumpHeld() ?? false;
}
