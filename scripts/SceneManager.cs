using Godot;

public partial class SceneManager : Node2D
{
    [Export]
    private PackedScene playerScene;

    public override void _Ready()
    {
        int playerCount = GameManager.Players.Count;
        int index = 0;

        for (int i = 0; i < playerCount; i++)
        {
            var player = GameManager.Players[i];

            Player playerInstance = playerScene.Instantiate<Player>();
            playerInstance.Name = player.Id.ToString();
            playerInstance.SetUpPlayer(player.Name);
            AddChild(playerInstance);

            // Camera setup for each player
            Camera2D camera = new Camera2D();
            camera.Name = "Camera";
            playerInstance.AddChild(camera);

            // Ensure the camera is current for the correct player
            playerInstance.SetCamera(); // Call the RPC method to set the camera

            // Position player using spawn points
            foreach (Node2D spawnPoint in GetTree().GetNodesInGroup("PlayerSpawnPoints"))
            {
                if (int.Parse(spawnPoint.Name) == index)
                {
                    playerInstance.GlobalPosition = spawnPoint.GlobalPosition;
                }
            }
            index++;
        }
    }
}
