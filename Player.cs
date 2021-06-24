using Godot;
using System;

public class Player : KinematicBody2D
{

    //TODO: Introduce stopping power
    private Sprite sprite;

    private bool isSpriteFacingLeft;

    private Vector2 motion;

    [Export]
    private int gravityForce = 10;

    [Export]
    private int intertiaCounterForce = 300;

    [Export]
    public int moveSpeed = 1000;

    [Export]
    public int stoppingPowerForce = 20;

    [Export]
    public int maxLaterialSpeed = 100;

    [Export]
    public int maxVerticalSpeed = 50;

    [Export]
    public int jumpForce = 500;

    public override void _Ready()
    {
        this.sprite = (Sprite)GetNode("Sprite");
    }

    public override void _Process(float delta)
    {
        GD.Print("Motion Y: " + motion.y);
    }
    public override void _PhysicsProcess(float delta)
    {

        bool isMovingLeft = Input.IsActionPressed("move_left");
        bool isMovingRight = Input.IsActionPressed("move_right");

        dealWithChangeOfDirection(isMovingLeft, isMovingRight);
        dealWithInputs(isMovingLeft, isMovingRight, delta);
        dealWithIntertia(isMovingLeft, isMovingRight, delta);
        dealWithGravity(delta);

        motion = MoveAndSlide(motion, Vector2.Up);

    }
    private void dealWithInputs(bool isMovingLeft, bool isMovingRight, float delta)
    {
        // Moves the player left
        if (isMovingLeft && motion.x > -maxLaterialSpeed)
        {
            motion.x = Math.Max(motion.x - (moveSpeed * delta), -maxLaterialSpeed);
            // Stopping force
            if (motion.x > 0)
            {
                motion.x -= (stoppingPowerForce * delta);
            }
        }

        // Moves the player right
        if (isMovingRight && motion.x < maxLaterialSpeed)
        {
            motion.x = Math.Min(motion.x + (moveSpeed * delta), maxLaterialSpeed);
            // Stopping force
            if (motion.x < 0)
            {
                motion.x += (stoppingPowerForce * delta);
            }
        }

        bool isJumping = Input.IsActionJustPressed("jump");
        if (isJumping && IsOnFloor())
        {
            GD.Print("Motion Y: " + motion.y);
            motion.y -= (jumpForce * delta);
        }

    }

    private void dealWithIntertia(bool isMovingLeft, bool isMovingRight, float delta)
    {
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
    }

    private void dealWithChangeOfDirection(bool isMovingLeft, bool isMovingRight)
    {
        // Changes the sprite's direction
        if (isMovingLeft && !isSpriteFacingLeft)
        {
            isSpriteFacingLeft = true;
        }
        else if (isMovingRight && isSpriteFacingLeft)
        {
            isSpriteFacingLeft = false;
        }
        sprite.FlipH = isSpriteFacingLeft;
    }

    private void dealWithGravity(float delta)
    {
        if (IsOnFloor())
        {
            motion.y = 0;
            return;
        }
        motion.y = Math.Min(motion.y + (gravityForce * delta), maxVerticalSpeed);
    }

}
