using System.Collections.Generic; // For player list
using Godot;

public partial class LobbyUiManager : Node
{
    [Export]
    private VBoxContainer serverListPanel;

    [Export]
    private VBoxContainer playerListPanel;

    [Export]
    private HBoxContainer playerServerInfo;

    [Export] // Allows setting the path in the Godot editor
    private VBoxContainer serverListContainer;

    [Export]
    private HBoxContainer serverEntryTemplate;

    private ServerList serverList;

    // Ensure nodes are found after the scene is fully loaded
    public override void _Ready()
    {
        playerListPanel.Visible = false; // Hide player list panel initially
        serverListPanel.Visible = true; // Show server list panel initially
        GetTree().CreateTimer(0.1f).Timeout += InitializeUI; // Defer initialization
    }

    private void InitializeUI()
    {
        serverList = new ServerList(); // Initialize with sample data
        PopulateServerList();
    }

    public void PopulateServerList()
    {
        GD.Print("Populating server list. Server count:", serverList.Servers.Count); // Debugging

        // Safety check
        if (serverListContainer == null || serverEntryTemplate == null)
        {
            GD.PrintErr(
                "serverListContainer or serverEntryTemplate is null. Check your scene tree."
            );
            return;
        }

        // Clear existing entries (except the template) efficiently
        for (int i = serverListContainer.GetChildCount() - 1; i >= 0; i--)
        {
            var child = serverListContainer.GetChild(i);
            if (child != serverEntryTemplate)
                child.QueueFree();
        }

        // Add new entries
        foreach (var serverData in serverList.Servers) // Directly use the serverData variable here
        {
            AddServerEntry(serverData); // Pass the serverData to the AddServerEntry method
        }
    }

    private void AddServerEntry(ServerData serverData)
    {
        var serverEntry = serverEntryTemplate.Duplicate() as HBoxContainer;
        serverEntry.Visible = true;
        var serverNameLabel = serverEntry.GetNode<Label>("server_name_label");
        serverNameLabel.Text = serverData.server_name;

        var mapNameLabel = serverEntry.GetNode<Label>("map_name_label");
        mapNameLabel.Text = serverData.map_name;

        var playerCountLabel = serverEntry.GetNode<Label>("player_count_label");
        playerCountLabel.Text = serverData.player_count.ToString();

        var pingLabel = serverEntry.GetNode<Label>("ping_label");
        pingLabel.Text = $"{serverData.ping} ms";

        var connectButton = serverEntry.GetNode<Button>("connect");
        connectButton.Disabled = false;
        connectButton.Pressed += () => _on_connect_btn_pressed(serverData);

        serverListContainer.AddChild(serverEntry);
    }

    // Handle "Back" button click
    private void _on_back_btn_pressed()
    {
        if (serverListPanel.Visible == true)
        {
            GetTree().ChangeSceneToFile("res://scenes/MainMenu.tscn");
        }
        else
        {
            serverListPanel.Visible = true;
            playerListPanel.Visible = false;
        }
    }

    // Handle "connect" button click
    private void _on_connect_btn_pressed(ServerData serverData)
    {
        serverListPanel.Visible = false;
        playerListPanel.Visible = true;
        playerServerInfo.GetNode<Label>("server_name_label").Text =
            $"Server: {serverData.server_name}";
        playerServerInfo.GetNode<Label>("map_name_label").Text = $"Map: {serverData.map_name}";
        playerServerInfo.GetNode<Label>("player_count_label").Text =
            $"Players: {serverData.player_count}";
        playerServerInfo.GetNode<Label>("ping_label").Text = $"Ping: {serverData.ping} ms";
    }
}
