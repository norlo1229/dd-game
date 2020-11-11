public interface IGameAction
{
    ActionState ActionState { get; set; }
    void Activate(Character invoker, GameActionData options);
    void Cancel();
    void Update();
}

public enum ActionState
{
    Active,
    Completed
}