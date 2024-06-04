using Godot;

public partial class FinishLine : Area2D
{
    public override void _Ready()
    {
        BodyEntered += OnBodyEntered;
    }

    private void OnBodyEntered(Node body)
    {
        if (body is Player player)
        {
            // Stop player movement
            GameManager.GameOver = true;

            // Log the winner
            GD.Print("Player " + player.Name + " has crossed the finish line!");
        }
    }
}
