using Godot;

public partial class Client : Node
{
    public override void _Ready()
    {
        NetworkManager.instance.startService += CreateClient;
    }
    private void CreateClient(int port)
    {
        ENetMultiplayerPeer peer = new ENetMultiplayerPeer();
        peer.CreateClient("localhost", port);
        if (peer.GetConnectionStatus() == MultiplayerPeer.ConnectionStatus.Disconnected)
        {
            OS.Alert("error creating client");
            return;
        }
        Multiplayer.MultiplayerPeer = peer;
        GD.Print("Client created successfully");
    }
}
