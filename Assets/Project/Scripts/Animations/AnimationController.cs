using UnityEngine;

public class AnimationController : MonoBehaviour
{
    public delegate void OnEndGrabAnimation();

    public OnEndGrabAnimation onEndGrabAnimation;

    [SerializeField] GameObject coins;

    [SerializeField] GameObject weapon;
    [SerializeField] ParticleSystem effect;

    bool isHit = false;

    public GameObject Weapon { get => weapon; }
    public ParticleSystem Effect { get => effect; }
    public bool IsHit { get => isHit; set => isHit = value; }

    public void Throw()
    {
        AI ai = GetComponent<AI>();
        ai.Throw();
    }

    public void TakeCoins() 
    {
        coins.SetActive(false);
    }

    public void EndGrabAnimation()
    {
        onEndGrabAnimation?.Invoke();
    }
}
