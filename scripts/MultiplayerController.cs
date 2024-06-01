using System;
using Godot;

public partial class MultiplayerController : Control
{
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

    public void _on_host_button_down() { }

    public void _on_join_button_down() { }

    //startgmae
    public void _on_start_game_button_down() { }
}
