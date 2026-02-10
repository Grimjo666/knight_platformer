using Godot;

public class AICharacterSensors
{
    private RayCast2D floorCheck, wallCheck, stageCheck;
    private Area2D detectArea;
    private Node2D raycasts;
    public AICharacterSensors(CharacterBody2D character)
    {
        this.floorCheck = character.GetNode<RayCast2D>("Raycasts/FloorCheck");
        this.wallCheck = character.GetNode<RayCast2D>("Raycasts/WallCheck");
        this.stageCheck = character.GetNode<RayCast2D>("Raycasts/StageCheck");
        this.detectArea = character.GetNode<Area2D>("DetectArea");
        this.raycasts = character.GetNode<Node2D>("Raycasts");
    }

    public bool IsOnFloor() => floorCheck.IsColliding();
    public bool IsFacingWall() => wallCheck.IsColliding();
    public bool IsFacingStage() => stageCheck.IsColliding();

    public void FlipRaycastDirection()
    {
        raycasts.Scale = new Vector2(-raycasts.Scale.X, raycasts.Scale.Y);
    }
}