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

            // Continuous rightward movement
            velocity.X = Speed; // Always move to the right

            // Gravity
            if (!IsOnFloor())
            {
                velocity.Y += gravity * (float)delta;
            }

            // Jump (using Spacebar)
            if (Input.IsActionJustPressed("ui_accept")) // "ui_accept" is usually mapped to Spacebar
            {
                if (IsOnFloor())
                {
                    velocity.Y = JumpVelocity;
                }
            }

            Velocity = velocity;
            MoveAndSlide();
        }
    }

    public void SetUpPlayer(string name)
    {
        GetNode<Label>("Label").Text = name;
    }

    [Rpc(MultiplayerApi.RpcMode.AnyPeer)]
    public void SetCamera()
    {
        var camera = GetNode<Camera2D>("Camera");
    }
}
