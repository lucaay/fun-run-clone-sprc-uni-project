using Godot;

public partial class NetworkManager : Control
{
    [Export]
    public bool isServer = false;

    const int PORT = 25565;

    public delegate void StartService(int port);
    public StartService startService;
    public static NetworkManager instance;



    public override void _Ready()
    {
        instance = this;
        if (isServer)
        {
            AddChild(new Server());
        }
        else
        {
            AddChild(new Client());
        }

        startService?.Invoke(PORT);
    }
}
