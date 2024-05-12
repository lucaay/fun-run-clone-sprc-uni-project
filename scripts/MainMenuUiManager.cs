using System.Collections.Generic;
using Godot;

public partial class MainMenuUiManager : Node
{
    [Export]
    private Button playBtn;

    [Export]
    private LineEdit lineEdit;

    [Export]
    private Label errorLabel;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready() { }

    // when starting to type in the LineEdit node remove the text from the error label
    private void _on_lineEdit_text_changed()
    {
        errorLabel.Text = "";
    }

    // Handle "Play" button click (Connect to "pressed" signal in the editor)
    private void _on_play_btn_pressed()
    {
        // Get the player name from the LineEdit node
        var playerName = lineEdit.Text;

        // Check if the player name is empty or less than 4 characters
        if (string.IsNullOrEmpty(playerName) || playerName.Length < 4)
        {
            // Show an error message
            errorLabel.Text = "Player name >4 please.";
            return;
        }

        // Create a new player object
        var player = new Player(playerName, false);
        PlayerManager.Instance.CurrentPlayer = player;

        // Change the scene to the lobby
        GetTree().ChangeSceneToFile("res://scenes/LobbyMenu.tscn");
    }

    // Handle "Exit" button click (Connect to "pressed" signal in the editor)
    private void _on_exit_btn_pressed()
    {
        // Quit the game
        GetTree().Quit();
    }
}
