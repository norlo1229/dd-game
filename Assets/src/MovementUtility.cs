using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MovementUtility : MonoBehaviour
{
    private static MovementUtility _instance;

    public Tilemap MovementLayer;
    public TileBase MovementTile;
    public TileBase CombatTile;
    public GridLayout Grid;

    public void Awake()
    {
        var mapResults = gameObject.GetComponentsInChildren<Tilemap>().ToList();
        MovementLayer = mapResults.Where(x => x.gameObject.name == "movement-layer").FirstOrDefault();
        Grid = MovementLayer.gameObject.GetComponent<GridLayout>();
    }

    public void Start()
    {
        _instance = gameObject.GetComponent<MovementUtility>();
        _instance.MovementLayer.ClearAllTiles();
    }

    public static Dictionary<Vector2Int, Assets.src.TileMap.TileData> GetMoveableTiles(Vector2Int position, int distance)
    {
        return GetMoveableTiles(position.x, position.y, distance);
    }

    public static Vector2Int GetGridLocation(Vector3 worldLoc)
    {
        var result = _instance.Grid.LocalToCell(worldLoc);

        var cleanResult = new Vector2Int(result.x, result.y);
        return cleanResult;
    }

    public static Dictionary<Vector2Int, Assets.src.TileMap.TileData> GetMoveableTiles(int x, int y, int distance)
    {
        var tileDict = new Dictionary<Vector2Int, Assets.src.TileMap.TileData>();

        var startX = x - distance;
        var startY = y - distance;
        var endX = x + distance;
        var endY = y + distance;


        for (int drawX = startX; drawX <= endX; drawX++)
        {
            for (int drawY = startY; drawY <= endY; drawY++)
            {
                var currentTile = MapUtility.GetTile(drawX, drawY);

                if(currentTile == null)
                {
                    break;
                }

                if (currentTile.IsWalkable)
                {
                    tileDict.Add(new Vector2Int(drawX, drawY), currentTile);
                }
            }
        }

        return tileDict;
    }

    public static void DrawMovementTile(Vector2Int pos)
    {
        _instance.MovementLayer.SetTile(new Vector3Int(pos.x, pos.y,  0), _instance.MovementTile);
    }

    public static void DrawCombatTile(Vector2Int pos)
    {
        _instance.MovementLayer.SetTile(new Vector3Int(pos.x, pos.y, 0), _instance.CombatTile);
    }

    public static bool CanMoveToTile(Vector2Int position)
    {
        var tile = MapUtility.GetTile(position.x, position.y);

        if(tile.IsWalkable && tile.OccupyingCharacter == null)
        {
            return true;
        }

        return false;
    }

    public static void OccupyTile(Character character, Vector2Int position)
    {
        var tile = MapUtility.GetTile(position.x, position.y);
        tile.OccupyTile(character);

        Debug.Log($"tile {position} is occupied by {character.name}");
    }

    public static void ClearTiles()
    {
        _instance.MovementLayer.ClearAllTiles();
    }
}
