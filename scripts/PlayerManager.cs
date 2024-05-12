using Godot;

public partial class PlayerManager : Node
{
    public static PlayerManager Instance { get; private set; } // Singleton access point
    public Player CurrentPlayer { get; set; }

    public override void _Ready()
    {
        Instance = this;
    }
}
