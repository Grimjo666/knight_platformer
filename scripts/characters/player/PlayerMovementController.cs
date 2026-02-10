using System;
using Godot;


public class PlayerMovementController : BaseMovementController
{

    public PlayerMovementController(CharacterBody2D character, IInputProvider input) 
        : base(character, input)
    {
        maxSpeed = 250;
        acceleration = 700f;
        friction = 1200f;
        jumpVelocity = -400f;
    }

}
