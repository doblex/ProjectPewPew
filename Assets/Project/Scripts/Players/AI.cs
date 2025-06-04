using UnityEngine;

public class AI : PG
{
    [SerializeField] Animator animator;
    float coinTimer = 0;

    public Animator Animator { get => animator;}

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
            CanTrow = false;
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
        base.Shoot();
        animator.SetTrigger("trShoot");
    }

    protected override void CheckForShoot()
    {}
}
