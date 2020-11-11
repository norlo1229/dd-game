using UnityEngine;

namespace Assets.src.actors
{
    public class AIControllerBase : MonoBehaviour
    {
        Character target;

        public void Awake()
        {
            target = GetComponent<Character>();

            if (target == null)
            {
                gameObject.SetActive(false);
            }
        }

        public void Execute()
        {
            target.OnEndTurn(target, null);
        }
    }
}
