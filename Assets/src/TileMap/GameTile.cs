using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GameTile : Tile
{
    public bool IsWalkable;

    [MenuItem("Assets/Create/Custom/Game Tile")]
    public static void CreateGameTile()
    {
        string path = EditorUtility.SaveFilePanelInProject("Save Tile", "New Tile", "Asset", "Save Tile", "Assets");

        if (path == "")
            return;
        AssetDatabase.CreateAsset(ScriptableObject.CreateInstance<GameTile>(), path);
    }
}
