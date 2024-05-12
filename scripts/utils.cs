using Godot;

public static class Utils // Static class for utility functions
{
    public static NodePath CombinePaths(NodePath path1, NodePath path2) // Public static method
    {
        // if path2 has / at the beginning, remove it
        var path2Str = path2.ToString();
        if (path2Str.StartsWith("/"))
        {
            path2Str = path2Str.Substring(1);
        }

        return new NodePath($"{path1}/{path2Str}");
    }
}
