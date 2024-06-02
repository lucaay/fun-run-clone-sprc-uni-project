using System;
using Godot;

public partial class Player : CharacterBody2D
{
    public const float Speed = 300.0f;
    public const float JumpVelocity = -400.0f;

    private AnimatedSprite2D _animatedSprite;

    // Get the gravity from the project settings to be synced with RigidBody nodes.
    public float gravity = ProjectSettings.GetSetting("physics/2d/default_gravity").AsSingle();

    public override void _Ready()
    {
        GetNode<MultiplayerSynchronizer>("MultiplayerSynchronizer")
            .SetMultiplayerAuthority(int.Parse(Name));

        // Get a reference to the AnimatedSprite2D node
        _animatedSprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");

        // Play the animation (replace "default" with the name of your animation)
        _animatedSprite.Play("default");
    }

    public override void _PhysicsProcess(double delta)
    {
        if (
            GetNode<MultiplayerSynchronizer>("MultiplayerSynchronizer").GetMultiplayerAuthority()
            == Multiplayer.GetUniqueId()
        )
        {
            Vector2 velocity = Velocity;

            // Add the gravity.
            if (!IsOnFloor())
                velocity.Y += gravity * (float)delta;

            // Handle Jump.
            if (Input.IsActionJustPressed("ui_accept") && IsOnFloor())
                velocity.Y = JumpVelocity;

            // Get the input direction and handle the movement/deceleration.
            // As good practice, you should replace UI actions with custom gameplay actions.
            Vector2 direction = Input.GetVector("ui_left", "ui_right", "ui_up", "ui_down");
            if (direction != Vector2.Zero)
            {
                velocity.X = direction.X * Speed;
            }
            else
            {
                velocity.X = Mathf.MoveToward(Velocity.X, 0, Speed);
            }

            Velocity = velocity;
            MoveAndSlide();
        }
    }

    public void SetUpPlayer(string name)
    {
        GetNode<Label>("Label").Text = name;
    }
}
