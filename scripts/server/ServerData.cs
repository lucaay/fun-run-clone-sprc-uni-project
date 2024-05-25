// Create a simple class to hold server data
using System.Collections.Generic;

public class ServerData
{
    public string server_name { get; set; }
    public string map_name { get; set; }
    public int player_count { get; set; }

    public int ping { get; set; }
    public string ipAddress { get; set; }
    public int port { get; set; }

    public List<Player> players { get; set; } // List to hold multiple players

    public ServerData(
        string serverName,
        string mapName,
        int playerCount,
        int pingArg,
        List<Player> playersList = null // Optional player list in the constructor
    )
    {
        server_name = serverName;
        map_name = mapName;
        player_count = playerCount;
        ping = pingArg;
        players = playersList ?? new List<Player>(); // Initialize if null
    }
}
