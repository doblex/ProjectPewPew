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
}
