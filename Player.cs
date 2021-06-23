using Godot;
using System;

public class Player : KinematicBody2D
{

    //TODO: Introduce stopping power

    private Vector2 motion;
    private int gravityForce = 0;

    [Export]
    private int intertiaCounterForce = 300;

    [Export]
    public int moveSpeed = 1000;

    [Export]
    public int stoppingPowerForce = 20;

    [Export]
    public int maxSpeed;

    public override void _Ready()
    {
        // Called every time the node is added to the scene
        // Initialization here.
        GD.Print("The player is ready!");
    }

    public override void _Process(float delta)
    {
        motion.y += gravityForce;
    }

    public override void _PhysicsProcess(float delta)
    {

        bool isMovingLeft = Input.IsActionPressed("move_left");
        bool isMovingRight = Input.IsActionPressed("move_right");

        if (isMovingLeft && motion.x > -maxSpeed)
        {
            motion.x = Math.Max(motion.x - (moveSpeed * delta), -maxSpeed);
            // They were just moving left
            if (motion.x > 0)
            {
                motion.x -= stoppingPowerForce;
            }
        }

        if (isMovingRight && motion.x < maxSpeed)
        {
            motion.x = Math.Min(motion.x + (moveSpeed * delta), maxSpeed);
            if (motion.x < 0)
            {
                motion.x += stoppingPowerForce;
            }
        }

        // Counters Intertia
        if (!isMovingLeft && !isMovingRight)
        {
            if (motion.x > 0)
            {
                motion.x = Math.Max(motion.x - (intertiaCounterForce * delta), 0);
            }
            else if (motion.x < 0)
            {
                motion.x = Math.Min(motion.x + (intertiaCounterForce * delta), 0);
            }
        }

        motion = MoveAndSlide(motion);

    }

}
