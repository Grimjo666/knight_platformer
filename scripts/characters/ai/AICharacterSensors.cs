using Godot;

public class AICharacterSensors
{
    private RayCast2D floorCheck, wallCheck, stageCheck;
    private Area2D detectArea;

    public AICharacterSensors(CharacterBody2D character)
    {   
        this.floorCheck = character.GetNode<RayCast2D>("RotationRoot/Raycasts/FloorCheck");
        this.wallCheck = character.GetNode<RayCast2D>("RotationRoot/Raycasts/WallCheck");
        this.stageCheck = character.GetNode<RayCast2D>("RotationRoot/Raycasts/StageCheck");
        this.detectArea = character.GetNode<Area2D>("DetectArea");
        
    }

    public bool IsOnFloor() => floorCheck.IsColliding();
    public bool IsFacingWall() => wallCheck.IsColliding();
    public bool IsFacingStage() => stageCheck.IsColliding();

}