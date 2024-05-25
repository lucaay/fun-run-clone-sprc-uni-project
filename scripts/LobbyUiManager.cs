using System.Collections.Generic; // For player list
using System.Net.Sockets;
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
    private VBoxContainer playerListContainer;

    [Export]
    private HBoxContainer playerEntryTemplate;

    [Export] // Allows setting the path in the Godot editor
    private VBoxContainer serverListContainer;

    [Export]
    private HBoxContainer serverEntryTemplate;

    private ServerList serverList;
    private Player localPlayer;

    // Ensure nodes are found after the scene is fully loaded
    public override void _Ready()
    {
        if (PlayerManager.Instance.CurrentPlayer != null)
        {
            var player = PlayerManager.Instance.CurrentPlayer;
            // Use the player data here
            localPlayer = player;

            playerListPanel.Visible = false; // Hide player list panel initially
            serverListPanel.Visible = true; // Show server list panel initially
            GetTree().CreateTimer(0.1f).Timeout += InitializeUI; // Defer initialization
        }
        else
        {
            GetTree().ChangeSceneToFile("res://scenes/MainMenu.tscn");
        }
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

        // Update server info in playerServerInfo
        playerServerInfo.GetNode<Label>("server_name_label").Text =
            $"Server: {serverData.server_name}";
        playerServerInfo.GetNode<Label>("map_name_label").Text = $"Map: {serverData.map_name}";
        playerServerInfo.GetNode<Label>("player_count_label").Text =
            $"Players: {serverData.player_count}";
        playerServerInfo.GetNode<Label>("ping_label").Text = $"Ping: {serverData.ping} ms";

        // Update player list
        PopulatePlayerList(serverData.players);
    }

    public void PopulatePlayerList(List<Player> players)
    {
        GD.Print("Populating player list. Player count:", players.Count); // Debugging

        // Safety check
        if (playerListContainer == null || playerEntryTemplate == null)
        {
            GD.PrintErr(
                "playerListContainer or playerEntryTemplate is null. Check your scene tree."
            );
            return;
        }

        // Clear existing entries (except the template) efficiently

        for (int i = playerListContainer.GetChildCount() - 1; i >= 0; i--)
        {
            var child = playerListContainer.GetChild(i);
            if (child != playerEntryTemplate && child != playerServerInfo)
                child.QueueFree();
        }

        foreach (var player in players)
        {
            if (player != null) // Add null check
            {
                AddPlayerEntry(player, players);
            }
            else
            {
                GD.PrintErr("Encountered a null player entry."); // Log an error
            }
        }
    }

    private void AddPlayerEntry(Player player, List<Player> players)
    {
        var playerEntry = playerEntryTemplate.Duplicate() as HBoxContainer;
        playerEntry.Visible = true;
        var playerNameLabel = playerEntry.GetNode<Label>("player_name_label");
        playerNameLabel.Text = player.name;

        var playerReadyLabel = playerEntry.GetNode<Label>("ready_status_label");
        playerReadyLabel.Text = player.ready ? "Ready" : "Not Ready";

        var readyButton = playerEntry.GetNode<Button>("ready_btn");
        readyButton.Text = player.ready ? "Unready" : "Ready";
        //disable ready button if player is not the local player
        readyButton.Disabled = player.name != localPlayer.name;
        readyButton.Pressed += () => _on_ready_btn_pressed(player, players);

        playerListContainer.AddChild(playerEntry);
    }

    private void _on_ready_btn_pressed(Player player, List<Player> players)
    {
        player.ready = !player.ready; // Toggle ready status
        PopulatePlayerList(players); // Refresh player list
    }
}
