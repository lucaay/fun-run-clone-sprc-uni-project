using System.Collections.Generic; // For player list
using Godot;

public partial class LobbyUiManager : Node
{
    [Export] // Allows setting the path in the Godot editor
    private NodePath serverListContainerPath;

    [Export]
    private NodePath serverEntryTemplatePath;
    private VBoxContainer serverListContainer;
    private HBoxContainer serverEntryTemplate;
    private ServerList serverList;

    // Ensure nodes are found after the scene is fully loaded
    public override void _Ready()
    {
        GetTree().CreateTimer(0.1f).Timeout += InitializeUI; // Defer initialization
    }

    private void InitializeUI()
    {
        // get current node path
        var rootNode = GetPath();
        serverListContainer = GetNode<VBoxContainer>(
            Utils.CombinePaths(rootNode, serverListContainerPath)
        );
        serverEntryTemplate = serverListContainer.GetNode<HBoxContainer>(
            Utils.CombinePaths(rootNode, serverEntryTemplatePath)
        );

        serverList = new ServerList(); // Initialize with sample data
        ServerListUiActions.PopulateServerList(
            serverList,
            serverListContainer,
            serverEntryTemplate
        );
    }

    // Handle "Play" button click (Connect to "pressed" signal in the editor)
    private void _on_back_btn_pressed()
    {
        GetTree().ChangeSceneToFile("res://scenes/MainMenu.tscn");
    }

    // Network Event Handlers
    private void OnConnectedToServer()
    {
        GD.Print("Connected to server!");
    }

    private void OnConnectionFailed()
    {
        GD.PrintErr("Failed to connect to server.");
    }

    private void OnServerDisconnected()
    {
        GD.PrintErr("Disconnected from server.");
    }
}
