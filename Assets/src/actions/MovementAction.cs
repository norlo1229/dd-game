using Assets.src.TileMap;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.src.actions
{
    public class MovementAction : GameAction, IGameAction
    {
        private MovementState MovementState;
        private Character Invoker;
        private Dictionary<Vector2Int, TileData> MoveableTiles;
        private List<Vector2Int> Path;

        float defaultSpeed = .05f;
        float time;

        public ActionState ActionState { get; set; }

        public MovementAction()
        {
            time = defaultSpeed;
            Path = new List<Vector2Int>();
            ActionState = ActionState.Active;
            MovementState = MovementState.GetAvailableTiles;
            MoveableTiles = new Dictionary<Vector2Int, TileData>();
        }

        public void Activate(Character invoker, GameActionData options)
        {
            Invoker = invoker;
            MovementUtility.ClearTiles();
        }

        public void Cancel()
        {
            MoveableTiles.Clear();
            MovementUtility.ClearTiles();
        }

        public void Update()
        {
            switch (MovementState)
            {
                case MovementState.GetAvailableTiles:
                    GetTiles();
                    break;
                case MovementState.DrawTiles:
                    DrawTiles();
                    break;
                case MovementState.AwaitingInput:
                    CalculateMovementPath();
                    break;
                case MovementState.Moving:
                    MoveCharacter();
                    break;
                case MovementState.Cleanup:
                    CleanUp();
                    break;
            }
        }

        protected void GetTiles()
        {
            var scaledMovementValue = Invoker.AvailableMovement / GameSettings.TILE_SIZE;
            MoveableTiles = MovementUtility.GetMoveableTiles(Invoker.Position, scaledMovementValue);

            MovementState = MovementState.DrawTiles;
        }

        private void DrawTiles()
        {
            foreach (Vector2Int pos in MoveableTiles.Keys)
            {
                MovementUtility.DrawMovementTile(pos);
            }

            MovementState = MovementState.AwaitingInput;
        }

        private void CalculateMovementPath()
        {
            if (Input.GetMouseButtonDown(0))
            {        
                var MousePos = Input.mousePosition;
                var viewPos = GameObject.FindObjectOfType<Camera>().ScreenToWorldPoint(MousePos);
                var endPostion = MovementUtility.GetGridLocation(viewPos);

                if (MoveableTiles.ContainsKey(endPostion) && MovementUtility.CanMoveToTile(endPostion))
                {
                    var pathFinder = new Pathfinder(MoveableTiles.Keys.ToList());
                    Path = pathFinder.FindPath(Invoker.Position, endPostion);
                    MovementState = MovementState.Moving;
                }
                else
                {
                    Debug.Log($"cant move to {endPostion}");
                }
            }
        }

        private void MoveCharacter()
        {
            if(time > 0)
            {
                time -= Time.deltaTime;
                return;
            }
            else
            {
                if (Path.Count == 0)
                {
                    MovementState = MovementState.Cleanup;
                    return;
                }
                else
                {
                    Vector2Int nextPos = Path[Path.Count - 1];
                    Invoker.transform.position = new Vector3(nextPos.x, nextPos.y, 0);
                    Path.RemoveAt(Path.Count - 1);
                    time = defaultSpeed;
                    Invoker.AvailableMovement -= GameSettings.TILE_SIZE;
                }
            }
        }

        private void CleanUp()
        {
            MovementUtility.ClearTiles();
            ActionState = ActionState.Completed;
        }
    }
}

public enum MovementState
{
    GetAvailableTiles,
    DrawTiles,
    AwaitingInput,
    Moving,
    Cleanup
}
