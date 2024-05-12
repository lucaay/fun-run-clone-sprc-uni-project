public class Player
{
    public string name { get; set; }
    public bool ready { get; set; }

    public Player(string playerName, bool readyStatus)
    {
        name = playerName;
        ready = readyStatus;
    }
}
