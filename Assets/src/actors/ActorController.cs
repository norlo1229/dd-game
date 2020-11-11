using Assets.src.actions;
using UnityEngine;

public class ActorController : MonoBehaviour
{
    private static ActorController _instance;
    public Character Target;
    public bool InAction;

    private void Start()
    {
        InAction = false;
        _instance = GetComponent<ActorController>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Target == null)
        {
            return;
        }

        ProcessInput();
        ActionUpdate();
    }

    private void ProcessInput()
    {
        if (Input.GetKeyUp(KeyCode.M) && !InAction && Target.CanMove())
        {
            PushAction(new MovementAction());
        }

        if (Input.GetKeyUp(KeyCode.E) && Target.CurrentAction == null)
        {
            Target.OnEndTurn(Target, null);
        }

        if (Input.GetKeyUp(KeyCode.Escape))
        {
            PopAction();
        }
    }

    private void ActionUpdate()
    {
        if (Target.CurrentAction != null)
        {

            if (Target.CurrentAction.ActionState == ActionState.Completed)
            {
                PopAction();
            }
            else
            {
                Target.CurrentAction.Update();
            }
        }
    }

    private void PushAction(IGameAction gameAction)
    {
        PushAction(gameAction, null);
    }

    private void PushAction(IGameAction gameAction, GameActionData args)
    {
        if (InAction)
        {
            return;
        }

        InAction = true;
        Target.CurrentAction = gameAction;
        Target.CurrentAction.Activate(Target, args);
    }

    public void PopAction()
    {
        if(Target.CurrentAction == null)
        {
            return;
        }

        if(Target.CurrentAction.ActionState == ActionState.Active)
        {
            Target.CurrentAction.Cancel();
        }

        InAction = false;
        Target.CurrentAction = null;
    }

    public static void SetTarget(Character target)
    {
        _instance.Target = target;
    }
}
