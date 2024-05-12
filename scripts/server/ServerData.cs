// Create a simple class to hold server data
public class ServerData
{
    public string server_name { get; set; }
    public string map_name { get; set; }
    public int player_count { get; set; }
    public int ping { get; set; }

    public ServerData(string serverName, string mapName, int playerCount, int pingArg)
    {
        server_name = serverName;
        map_name = mapName;
        player_count = playerCount;
        ping = pingArg;
    }
}
