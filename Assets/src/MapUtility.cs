using Assets.src.TileMap;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapUtility : MonoBehaviour
{
    private static MapUtility _instance;

    public Tilemap Map;
    private Dictionary<Vector2Int, Assets.src.TileMap.TileData> TileData;

    public void Awake()
    {
        var mapResults = gameObject.GetComponentsInChildren<Tilemap>().ToList();
        Map = mapResults.Where(x => x.gameObject.name == "layer0").FirstOrDefault();
    }

    public void Start()
    {
        _instance = gameObject.GetComponent<MapUtility>();
        _instance.InitializeTileData();
    }

    private void InitializeTileData()
    {
        TileData = new Dictionary<Vector2Int, Assets.src.TileMap.TileData>();

        foreach (Vector3Int pos in Map.cellBounds.allPositionsWithin)
        {
            var localPos = new Vector3Int(pos.x, pos.y, 0);

            if (!Map.HasTile(localPos)) continue;
            GameTile t = Map.GetTile<GameTile>(localPos);

            var tileState = new Assets.src.TileMap.TileData()
            {
                Position = new Vector2Int(localPos.x, localPos.y),
                IsWalkable = t.IsWalkable,
                OccupyingCharacter = null
            };

            TileData.Add(tileState.Position, tileState);
        }
    }

    public static Assets.src.TileMap.TileData GetTile(int x, int y)
    {
        var key = new Vector2Int(x, y);

        if (!_instance.TileData.ContainsKey(key))
        {
            return null;
        }

        var tile = _instance.TileData[key];

        if (tile == null)
        {
            return null;
        }

        return tile;
    }
}
