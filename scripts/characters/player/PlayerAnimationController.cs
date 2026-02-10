using Godot;
using Godot.Collections;

class PlayerAnimationController: BaseAnimationController {

    public PlayerAnimationController(Player player, BaseMovementController movementController) 
        : base(player, movementController) {
    }


    protected override void OnStateChanged(BaseCharacterState stateName) {
        base.OnStateChanged(stateName);
        switch (stateName) {
            case BaseCharacterState.TurnedAround:
                this.state.Travel("TurnAround");
                break;
        }
    }
    
}

    // foreach (Godot.Collections.Dictionary prop in animationTree.GetPropertyList())
    // {
    //     if (!prop.ContainsKey("name"))
    //         continue;

    //     string name = prop["name"].AsString();
    //     GD.Print(name);
    // }
