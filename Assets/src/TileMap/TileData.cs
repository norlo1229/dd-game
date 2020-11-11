using UnityEngine;

namespace Assets.src.TileMap
{
    public class TileData
    {
        public Vector2Int Position { get; set; }
        public bool IsWalkable { get; set; }
        public Character OccupyingCharacter { get; set; }

        public bool OccupyTile(Character character)
        {
            if (IsOccupied())
            {
                return false;
            }

            OccupyingCharacter = character;
            return true;
        }

        public bool IsOccupied()
        {
            if(OccupyingCharacter != null)
            {
                return true;
            }

            return false;
        }

        public void LeaveTile()
        {
            OccupyingCharacter = null;
        }
    }
}
