using Godot;
using Godot.Collections;

class SkeletonAnimationController: BaseAnimationController {

    public SkeletonAnimationController(Skeleton skeleton, BaseMovementController movementController) 
        : base(skeleton, movementController) {
    }

    protected override void OnStateChanged(BaseCharacterState stateName) {
        base.OnStateChanged(stateName);
        switch (stateName) {
            case BaseCharacterState.Running:
                this.state.Travel("Walk");
                break;
        }
    }
    
}