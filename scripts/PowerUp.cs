using Godot;

public partial class PowerUp : Area2D
{
    public override void _Ready()
    {
        BodyEntered += OnBodyEntered;
    }

    private void OnBodyEntered(Node body)
    {
        if (body is Player player)
        {
            // Apply power-up effect (e.g., increase speed, grant temporary invincibility)
            GD.Print("Power-up collected by " + player.Name);

            // Remove the power-up from the scene
            QueueFree();
        }
    }
}
