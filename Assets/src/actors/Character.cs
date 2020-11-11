using System;
using UnityEngine;

public class Character : GameActor
{
    public event EventHandler<EventArgs> EndTurn;

    public CharacterData CharacterData;
    public Vector2Int Position => new Vector2Int((int)transform.position.x, (int)transform.position.y);
    public int AvailableMovement;
    public bool ActionAvailable { get; private set; }
    public bool BonusActionAvailable { get; private set; }
    public int Initiative;

    public IGameAction CurrentAction;

    public void OnEndTurn(object sender, EventArgs args)
    {
        EndTurn?.Invoke(sender, args);
    }

    public void Start()
    {
        AvailableMovement = CharacterData.MovementSpeed;
        ActionAvailable = true;
        BonusActionAvailable = true;
    }

    public bool CanMove()
    {
        if(AvailableMovement > 0)
        {
            return true;
        }

        return false;
    }
}
