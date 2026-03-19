using Godot;

public class AnimationController {
	private AnimationTree animationTree;
	protected AnimationNodeStateMachinePlayback state;
	private MovementController movementController;
	bool isLocked;


	public AnimationController(CharacterBody2D character, MovementController movementController) {
		animationTree = character.GetNode<AnimationTree>("AnimationTree");
		animationTree.Active = true;
		this.movementController = movementController;

		state = (AnimationNodeStateMachinePlayback)animationTree.Get("parameters/playback");

		}

	public void Update() {
		animationTree.Set("parameters/Run/TimeScale/scale", movementController.SpeedRatio);
	}


	public void OnMovementState(MovementState state)
	{
		if (isLocked) return;

		animationTree.Set("parameters/conditions/run", state == MovementState.Run);
		animationTree.Set("parameters/conditions/jump", state == MovementState.Jump);
		animationTree.Set("parameters/conditions/turnAround", state == MovementState.TurnAround);
		animationTree.Set("parameters/conditions/idle", state == MovementState.Idle);
		
	}

	public void OnAttackState(AttackState attackState)
	{
		if (isLocked) return;

		state.Travel(attackState.ToString());
	}

	public void OnHealthState(HealthState healthState)
	{		
		if (isLocked) return;

		state.Travel(healthState.ToString());

		if (healthState == HealthState.Died)
		{
			isLocked = true;
		}
	}
	
}
 
