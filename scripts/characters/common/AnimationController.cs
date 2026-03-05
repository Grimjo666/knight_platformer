using Godot;

public class AnimationController {
	private AnimationTree animationTree;
	protected AnimationNodeStateMachinePlayback state;
	private CharacterBody2D character;
	private MovementController movementController;
	bool isLocked;


	public AnimationController(CharacterBody2D character, MovementController movementController) {
		this.character = character;
		animationTree = character.GetNode<AnimationTree>("AnimationTree");
		animationTree.Active = true;
		this.movementController = movementController;

		state = (AnimationNodeStateMachinePlayback)animationTree.Get("parameters/playback");

		}

	public void Update() {
		animationTree.Set("parameters/Run/TimeScale/scale", movementController.SpeedRatio);
	}

	public virtual void OnStateChanged(string stateName) {
		
		if (isLocked)
			return;

		if (stateName == "Died")
		{	
			state.Travel(stateName);
			isLocked = true;
		}
		
		state.Travel(stateName);

	}
	
}
 
