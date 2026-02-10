using Godot;

public abstract class BaseAnimationController {
    private AnimationTree animationTree;
    protected AnimationNodeStateMachinePlayback state;
    private CharacterBody2D character;
    private BaseMovementController movementController;


    protected BaseAnimationController(CharacterBody2D character, BaseMovementController movementController) {
        this.character = character;
        animationTree = character.GetNode<AnimationTree>("AnimationTree");
        animationTree.Active = true;
        this.movementController = movementController;

        state = (AnimationNodeStateMachinePlayback)animationTree.Get("parameters/playback");

        movementController.AnimationRequested += OnStateChanged;
        }

    public void Update() {
        animationTree.Set("parameters/Run/TimeScale/scale", movementController.SpeedRatio);
    }

    protected virtual void OnStateChanged(BaseCharacterState stateName) {
        switch (stateName) {
            case BaseCharacterState.Jumping:
                this.state.Travel("Jump");
                break;
            case BaseCharacterState.Running:
                this.state.Travel("Run");
                break;
            case BaseCharacterState.Idle:
                this.state.Travel("Idle");
                break;
        }
    }
    
}
 