using UnityEngine;

public class AI : PG
{
    [SerializeField] Animator animator;
    [SerializeField] GameObject weapon;
    [SerializeField] ParticleSystem effect;
    float coinTimer = 0;

    public GameObject Weapon { get => weapon; }
    public ParticleSystem Effect { get => effect; }

    public AI ()
    {
        playerType = PlayerType.AI;
    }

    public override int ChooseCoinDifficulty(Coin[] coins)
    {
        int index = Random.Range(0, coins.Length);

        Coin coin = coins[index];
        coinTimer = coin.LaunchDelay;
        return index;
    }

    public override int ChooseCoinTrajectory(Trajectory[] trajectories, out int itemIndex)
    {
        itemIndex = -1;

        return Random.Range(0, trajectories.Length);
    }

    public override int ChooseEnemyTrajectory(Trajectory[] trajectories)
    {
        return Random.Range(0, trajectories.Length);
    }

    protected override void CheckForThrow()
    {
        if (coinTimer <= 0 && CanTrow)
        {
            animator.SetTrigger("trTossCoin");
        }

        coinTimer -= Time.deltaTime;
    }

    public void AddShootEvent(TrajectoryManager trajectoryManager)
    {
        trajectoryManager.onAIShoot += Shoot;
    }

    public void RemoveShootEvent(TrajectoryManager trajectoryManager) 
    {
        trajectoryManager.onAIShoot -= Shoot;
    }

    public override void Shoot()
    {
        weapon.SetActive(true);

        base.Shoot();
        animator.SetTrigger("trShoot");
    }

    protected override void CheckForShoot()
    {}
}
