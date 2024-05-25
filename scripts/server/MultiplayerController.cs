using System;
using System.Linq;
using System.Net.Sockets;
using Godot;

public partial class MultiplayerController : Control
{
    [Export]
    private int _port = 8910;

    [Export]
    private string _ip = "localhost";

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

        UdpClient = new UdpClient();
        udpClient.Client.Bind(new IPEndPoint(IPAddress.Any, DiscoveryPort));

        GetTree().CreateTimer(5.0f).Timeout += DiscoverServers; // Discover every 5 seconds
        DiscoverServers(); // Initial discovery
    }

    private async void DiscoverServers()
    {
        serverList.Servers.Clear(); // Clear the list for new discoveries

        var broadcastEndpoint = new IPEndPoint(IPAddress.Broadcast, DiscoveryPort);
        var requestData = System.Text.Encoding.ASCII.GetBytes("ServerDiscoveryRequest");
        await udpClient.SendAsync(requestData, requestData.Length, broadcastEndpoint);

        var receiveTask = udpClient.ReceiveAsync();
        while (!receiveTask.IsCompleted)
        {
            await Task.Delay(100); // Add a small delay to prevent high CPU usage
        }

        var result = await receiveTask;
        var responseData = System.Text.Encoding.ASCII.GetString(result.Buffer);

        // Process responseData (e.g., split it into server details)
        var serverDetails = responseData.Split(',');
        var newServer = new ServerData(
            serverDetails[0], // Server name
            serverDetails[1], // Map name
            int.Parse(serverDetails[2]), // Player count
            int.Parse(serverDetails[3]), // Ping
            result.RemoteEndPoint.Address.ToString(), // IP Address
            int.Parse(serverDetails[4]) // Port
        );

        serverList.AddServer(newServer);

        PopulateServerList();
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

    public void ConnectToServer(string ipAddress, int port)
    {
        _peer = new ENetMultiplayerPeer();
        var status = _peer.CreateClient(ipAddress, port);
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
        GDPrintC.Print("Connecting to " + serverData.ipAddress + ":" + serverData.port);
        ConnectToServer(serverData.ipAddress, serverData.port);
    }
}
