using UnityEngine;

public class OnShootingBehaviuor : StateMachineBehaviour
{
    [SerializeField] float shootingTime;
    [SerializeField] float holsteringTime;

    AI AI;
    float timer = 0;
    bool effectPlayed = false;
    bool holstered = false;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        AI = animator.gameObject.GetComponentInParent<AI>();
        if (AI.Weapon != null)
        {
            timer = 0;
            effectPlayed = false;
            holstered = false;
            AI.Weapon.SetActive(true);
        }


    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (timer >= shootingTime && !effectPlayed)
        {
            AI.Effect.Play();
            effectPlayed = true;
        }

        if (timer >= holsteringTime && !holstered)
        {
            AI.Weapon.SetActive(false);
            holstered = true;
        }

        timer += Time.deltaTime;
    }


    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    //override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    //override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}
