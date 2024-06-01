using System;
using Godot;

public partial class MultiplayerController : Control
{
    [Export]
    private int port = 8910;

    [Export]
    private string address = "127.0.0.1";

    private ENetMultiplayerPeer peer;

    public int DisconnectedFromServer { get; private set; }

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        Multiplayer.PeerConnected += PeerConnected;
        Multiplayer.PeerDisconnected += PeerDisconnected;
        Multiplayer.ConnectedToServer += ConnectedToServer;
        Multiplayer.ConnectionFailed += ConnectionFailed;
    }

    //runs when the connection to the server fails and it only runs on the client
    private void ConnectionFailed()
    {
        GD.Print("Connection failed");
    }

    //runs when the connection to the server is established and it only runs on the client
    private void ConnectedToServer()
    {
        GD.Print("Connected to server");
    }

    //runs when a player disconnects from the server and it only runs on all peers
    // id of the player that disconnected
    private void PeerDisconnected(long id)
    {
        GD.Print("Player " + id.ToString() + " disconnected");
    }

    //runs when a player connects to the server and it only runs on all peers
    // id of the player that connected
    private void PeerConnected(long id)
    {
        GD.Print("Player " + id.ToString() + " connected");
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta) { }

    public void _on_host_button_down()
    {
        peer = new ENetMultiplayerPeer();
        var error = peer.CreateServer(port, 2);
        if (error != Error.Ok)
        {
            GD.Print("Error cannot host: " + error.ToString());
            return;
        }
        peer.Host.Compress(ENetConnection.CompressionMode.RangeCoder);

        Multiplayer.MultiplayerPeer = peer;
        GD.Print("Waiting for players...");
    }

    public void _on_join_button_down()
    {
        peer = new ENetMultiplayerPeer();
        peer.CreateClient(address, port);
        peer.Host.Compress(ENetConnection.CompressionMode.RangeCoder);
        Multiplayer.MultiplayerPeer = peer;
        GD.Print("Connecting to server...");
    }

    //startgmae
    public void _on_start_game_button_down()
    {
        var scene = ResourceLoader
            .Load<PackedScene>("res://scenes/TestScene.tscn")
            .Instantiate<Node2D>();
        GetTree().Root.AddChild(scene);
        this.Hide();
    }
}
