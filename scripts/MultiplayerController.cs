using System;
using System.Linq;
using Godot;

public partial class MultiplayerController : Control
{
    [Export]
    private int port = 8910;

    [Export]
    private string address = "127.0.0.1";

    private ENetMultiplayerPeer peer;

    public int DisconnectedFromServer { get; private set; }
    private int playersReady = 0;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        Multiplayer.PeerConnected += PeerConnected;
        Multiplayer.PeerDisconnected += PeerDisconnected;
        Multiplayer.ConnectedToServer += ConnectedToServer;
        Multiplayer.ConnectionFailed += ConnectionFailed;
        if (OS.GetCmdlineArgs().Contains("--server"))
        {
            hostGame();
        }
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
        RpcId(
            1,
            "sendPlayerInformaion",
            GetNode<LineEdit>("LineEdit").Text,
            Multiplayer.GetUniqueId()
        );
    }

    //runs when a player disconnects from the server and it only runs on all peers
    // id of the player that disconnected
    private void PeerDisconnected(long id)
    {
        GD.Print("Player " + id.ToString() + " disconnected");
        GameManager.Players.Remove(GameManager.Players.First(x => x.Id == id));
        var players = GetTree().GetNodesInGroup("Player");
        foreach (var p in players)
        {
            if (p.Name == id.ToString())
            {
                p.QueueFree();
            }
        }
    }

    //runs when a player connects to the server and it only runs on all peers
    // id of the player that connected
    private void PeerConnected(long id)
    {
        GD.Print("Player " + id.ToString() + " connected");
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta) { }

    private void hostGame()
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

    public void _on_host_button_down()
    {
        hostGame();
        sendPlayerInformaion(GetNode<LineEdit>("LineEdit").Text, 1);
    }

    public void _on_join_button_down()
    {
        peer = new ENetMultiplayerPeer();
        peer.CreateClient(address, port);
        peer.Host.Compress(ENetConnection.CompressionMode.RangeCoder);
        Multiplayer.MultiplayerPeer = peer;
        GD.Print("Connecting to server...");
        GetNode<Button>("Join").Disabled = true;
        GetNode<Button>("Join").Text = "Connected to server";
    }

    //startgmae
    public void _on_start_game_button_down()
    {
        Rpc("startGame");
    }

    [Rpc(
        MultiplayerApi.RpcMode.AnyPeer,
        CallLocal = true,
        TransferMode = MultiplayerPeer.TransferModeEnum.Reliable
    )]
    public void startGame()
    {
        playersReady++;
        if (playersReady == 2)
        {
            Rpc("allPlayersReady"); // Notify all clients to proceed
            playersReady = 0; // Reset the counter for the next game session
        }
        else
        {
            GetNode<Button>("StartGame").Text = "Waiting for another player...";
            GetNode<Button>("StartGame").Disabled = true;
        }
    }

    [Rpc(MultiplayerApi.RpcMode.AnyPeer, CallLocal = true)]
    public void allPlayersReady() // New RPC
    {
        foreach (var player in GameManager.Players)
        {
            GD.Print(player.Name + " is playing with id: " + player.Id);
        }
        var scene = ResourceLoader
            .Load<PackedScene>("res://scenes/TestScene.tscn")
            .Instantiate<Node2D>();
        GetTree().Root.AddChild(scene);
        this.Hide();
        // Reset game over flag
        GameManager.GameOver = false;
    }

    [Rpc(MultiplayerApi.RpcMode.AnyPeer)]
    private void sendPlayerInformaion(string name, int id)
    {
        PlayerInfo playerInfo = new PlayerInfo() { Name = name, Id = id };
        var fromList = GameManager.Players.FirstOrDefault(x => x.Id == id);
        if (fromList == null)
        {
            GameManager.Players.Add(playerInfo);
        }

        if (Multiplayer.IsServer())
        {
            foreach (var player in GameManager.Players)
            {
                Rpc("sendPlayerInformaion", player.Name, player.Id);
            }
        }
    }
}
