using Godot;

public partial class Server : Node
{
    public override void _Ready()
    {
        NetworkManager.instance.startService += CreateServer;
    }
    private void CreateServer(int port)
    {
        ENetMultiplayerPeer peer = new ENetMultiplayerPeer();
        peer.CreateServer(port);
        if (peer.GetConnectionStatus() == MultiplayerPeer.ConnectionStatus.Disconnected)
        {
            OS.Alert("error creating server");
            return;
        }
        Multiplayer.MultiplayerPeer = peer;
        GD.Print("Server created successfully");
    }
}
