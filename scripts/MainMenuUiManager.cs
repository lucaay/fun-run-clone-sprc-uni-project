using Godot;

public partial class MainMenuUiManager : Node
{
    // Called when the node enters the scene tree for the first time.
    public override void _Ready() { }

    // Handle "Play" button click (Connect to "pressed" signal in the editor)
    private void _on_play_btn_pressed()
    {
        GetTree().ChangeSceneToFile("res://scenes/LobbyMenu.tscn");
    }

    // Handle "Exit" button click (Connect to "pressed" signal in the editor)
    private void _on_exit_btn_pressed()
    {
        // Quit the game
        GetTree().Quit();
    }
}
