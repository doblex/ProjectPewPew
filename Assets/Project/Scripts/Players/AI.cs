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
        trajectoryManager.onAIShoot += OnShoot;
    }

    public void RemoveShootEvent(TrajectoryManager trajectoryManager) 
    {
        trajectoryManager.onAIShoot -= OnShoot;
    }

    public void OnShoot(bool isHit) 
    {
        GetComponent<AnimationController>().IsHit = isHit;
        Shoot();
    }

    public override void Shoot()
    {
        animator.SetTrigger("trShoot");
        base.Shoot();
    }

    protected override void CheckForShoot()
    {}
}
