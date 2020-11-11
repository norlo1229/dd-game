using Assets.src.TileMap;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.src.actions
{
    public abstract class GameAction
    {
        protected Dictionary<Vector2Int, TileData> TileList;

        protected virtual void GetTilesInRange(Vector2Int position, int distance)
        {
            TileList = MovementUtility.GetMoveableTiles(position, distance);
        }
    }
}
