using Godot;

public static class ServerListUiActions // Static class for utility functions
{
    public static void PopulateServerList(
        ServerList serverList,
        VBoxContainer serverListContainer,
        HBoxContainer serverEntryTemplate
    )
    {
        GD.Print("Populating server list. Server count:", serverList.Servers.Count); // Debugging
        if (serverListContainer != null && serverEntryTemplate != null)
        {
            foreach (Node child in serverListContainer.GetChildren())
            {
                if (child != serverEntryTemplate)
                    child.QueueFree();
            }

            foreach (ServerData serverData in serverList.Servers)
            {
                AddServerEntry(serverData, serverEntryTemplate, serverListContainer);
            }
        }
        else
        {
            GD.PrintErr(
                "serverListContainer or serverEntryTemplate is null. Check your scene tree."
            );
        }
    }

    private static void AddServerEntry(
        ServerData serverData,
        HBoxContainer serverEntryTemplate,
        VBoxContainer serverListContainer
    )
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

        serverListContainer.AddChild(serverEntry);
    }
}
