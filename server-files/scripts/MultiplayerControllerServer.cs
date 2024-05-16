using System;
using System.Linq;
using Godot;

public partial class MultiplayerControllerServer : Control
{
    [Export]
    private int _port = 8910;

    [Export]
    private string _ip = "127.0.0.1";

    [Export]
    private int _maxPlayerCount = 2;

    private ENetMultiplayerPeer _peer;

    public override void _Ready()
    {
        GDPrintS.Print("<<< START SERVER >>>");
        Multiplayer.PeerConnected += PlayerConnected;
        Multiplayer.PeerDisconnected += PlayerDisconnected;

        HostGame();
        this.Hide();
    }

    private void HostGame()
    {
        _peer = new ENetMultiplayerPeer();
        var status = _peer.CreateServer(_port, _maxPlayerCount);
        if (status != Error.Ok)
        {
            GDPrintS.PrintErr("Server could not be created:");
            GDPrintS.PrintErr($"Port: {_port}");
            return;
        }

        _peer.Host.Compress(ENetConnection.CompressionMode.RangeCoder);
        Multiplayer.MultiplayerPeer = _peer;
        GDPrintS.Print("Server started SUCCESSFULLY.");
        GDPrintS.Print("Waiting for players to connect ...");
    }

    private void PlayerConnected(long id)
    {
        GDPrintS.Print($"Player <{id}> connected.");
    }

    private void PlayerDisconnected(long id)
    {
        GDPrintS.Print($"Player <{id}> disconected.");
    }
}
