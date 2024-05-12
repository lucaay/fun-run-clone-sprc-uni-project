using System.Collections.Generic;

public class ServerList
{
    private List<ServerData> _serverData = new();

    // Constructor (Initialize with some sample data)
    public ServerList()
    {
        _serverData.Add(
            new ServerData(
                "My Awesome Server",
                "map1",
                12,
                50,
                new List<Player> { new("Player1", false), new("Player2", false) }
            )
        );
        _serverData.Add(
            new ServerData(
                "Beginner's Haven",
                "map2",
                5,
                100,
                new List<Player> { new("Player3", false) }
            )
        );
        _serverData.Add(
            new ServerData("Pro Only!", "map3", 2, 25, new List<Player> { new("Player4", false) })
        );
    }

    // Property to access the server data list
    public List<ServerData> Servers
    {
        get { return _serverData; }
    }

    // Method to add a new server (I'll get this from networking)
    public void AddServer(ServerData newServer)
    {
        _serverData.Add(newServer);
    }

    // Method to update an existing server (If data changes)
    public void UpdateServer(int index, ServerData updatedServer)
    {
        if (index >= 0 && index < _serverData.Count)
        {
            _serverData[index] = updatedServer;
        }
    }

    // Method to remove a server
    public void RemoveServer(int index)
    {
        if (index >= 0 && index < _serverData.Count)
        {
            _serverData.RemoveAt(index);
        }
    }
}
