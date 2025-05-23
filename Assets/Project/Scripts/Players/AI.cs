using UnityEngine;

public class AI : PG
{
    float coinTimer = 0;

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

    public override Trajectory ChooseCoinTrajectory(Trajectory[] trajectories, out Item item)
    {
        item = null;
        return trajectories[Random.Range(0, trajectories.Length)];
    }

    public override Trajectory ChooseEnemyTrajectory(Trajectory[] trajectories)
    {
        return trajectories[Random.Range(0, trajectories.Length)];
    }

    protected override void CheckForShoot()
    {
        
    }

    protected override void CheckForThrow()
    {
        if (coinTimer <= 0 && CanTrow)
        {
            Throw();
        }

        coinTimer -= Time.deltaTime;
    }
}
