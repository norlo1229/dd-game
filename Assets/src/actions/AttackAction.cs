using Assets.src.actions;
using System.Collections.Generic;

public class AttackAction : GameAction, IGameAction
{
    public ActionState ActionState { get; set; }
    private AttackActionState AttackActionState;
    private List<GameTile> CombatTiles;

    public AttackAction()
    {
        AttackActionState = AttackActionState.GetCombatTiles;
    }

    public void Activate(Character invoker, GameActionData options)
    {
        ActionState = ActionState.Active;
    }

    private void GetTilesInRange()
    {

    }

    public void Cancel()
    {
        throw new System.NotImplementedException();
    }

    public void Update()
    {
        return;
    }
}

public enum AttackActionState
{
    GetCombatTiles,
    DrawCombatTiles,
    PerformAttack,
    Cleanup
}
