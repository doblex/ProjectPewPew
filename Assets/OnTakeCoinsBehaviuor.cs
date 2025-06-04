using UnityEngine;

public class OnTakeCoinsBehaviuor : StateMachineBehaviour
{
    [SerializeField] float takeCoinsDelay;

    AnimationController animationController;

    float timer = 0;
    bool effectPlayed = false;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
    {
        animationController = animator.gameObject.GetComponentInParent<AnimationController>();
        timer = 0;
        effectPlayed = false;
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (timer >= takeCoinsDelay && !effectPlayed)
        {
            animationController.TakeCoins();
            effectPlayed = true;
        }

        timer += Time.deltaTime;
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animationController.EndGrabAnimation();
    }
}
