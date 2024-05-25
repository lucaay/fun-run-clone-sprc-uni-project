using System;
using System.Linq;
using Godot;

public partial class MultiplayerController : Control
{
    [Export]
    private int _port = 8910;

    [Export]
    private string _ip = "127.0.0.1";

    private ENetMultiplayerPeer _peer;
    private int _playerId;

    public override void _Ready() 
    {
        Multiplayer.PeerConnected += PlayerConnected;
        Multiplayer.PeerDisconnected += PlayerDisconnected;
        Multiplayer.ConnectedToServer += ConnectionSuccessful;
        Multiplayer.ConnectionFailed += ConnectionFailed;

        // Connect to the "join_server" signal in the parent node (LobbyUiManager)
        GetParent().Connect("join_server", new Callable(this, "OnJoinServer"));
    }

    private void ConnectionFailed()
    {
        GDPrintC.Print("Connection FAILED.");
        GDPrintC.Print("Could not connect to server.");
    }

    private void ConnectionSuccessful()
    {
        GDPrintC.Print("Connection SUCCESSFULL.");

        _playerId = Multiplayer.GetUniqueId();

        GDPrintC.Print(_playerId, "Sending player information to server.");
        GDPrintC.Print(_playerId, $"Id: {_playerId}");

        RpcId(1, "SendPlayerInformation", _playerId);
    }

    private void PlayerConnected(long id)
    {
        GDPrintC.Print(_playerId, $"Player <{id}> connected.");
    }

    private void PlayerDisconnected(long id)
    {
        GDPrintC.Print(_playerId, $"Player <${id}> disconnected.");
    }

    public void ConnectToServer()
    {
        _peer = new ENetMultiplayerPeer();
        var status = _peer.CreateClient(_ip, _port);
        if (status != Error.Ok)
        {
            GDPrintC.PrintErr("Creating client FAILED.");
            return;
        }

        _peer.Host.Compress(ENetConnection.CompressionMode.RangeCoder);
        Multiplayer.MultiplayerPeer = _peer;
    }

    // New method to handle the "join_server" signal
    public void OnJoinServer(ServerData serverData)
    {
        GDPrintC.Print("Received server data: " + serverData.server_name); // Confirm data is received
        ConnectToServer();
    }
}
