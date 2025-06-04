using UnityEngine;

public class OnShootingBehaviuor : StateMachineBehaviour
{
    [SerializeField] float shootingTime;
    [SerializeField] float holsteringTime;

    AnimationController animationController;
    float timer = 0;
    bool effectPlayed = false;
    bool holstered = false;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animationController = animator.gameObject.GetComponentInParent<AnimationController>();
        if (animationController.Weapon != null)
        {
            timer = 0;
            effectPlayed = false;
            holstered = false;
            animationController.Weapon.SetActive(true);
        }
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (timer >= shootingTime && !effectPlayed)
        {
            animationController.Effect.Play();
            effectPlayed = true;
        }

        if (timer >= holsteringTime && !holstered)
        {
            animationController.Weapon.SetActive(false);
            holstered = true;
        }

        timer += Time.deltaTime;
    }
}
