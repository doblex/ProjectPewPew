using UnityEngine;
using static UnityEditor.Progress;


public class Player : PG
{
    [SerializeField] KeyCode ThrowKey;
    [SerializeField] KeyCode ShootKey;

    public Player()
    {
        playerType = PlayerType.PLAYER;
    }

    public override int ChooseCoinDifficulty(Coin[] coins)
    {
        int index = -1;
        index = 0;
        return index;
    }

    public override int ChooseCoinTrajectory(Trajectory[] trajectories, out int itemIndex)
    {
        int index = -1;
        itemIndex = -1;
        index = 0;
        return index;
    }

    public override int ChooseEnemyTrajectory(Trajectory[] trajectories)
    {
        int index = -1;
        index = 0;
        return index;
    }

    protected override void CheckForThrow()
    {
        if (Input.GetKeyDown(ThrowKey) && CanTrow)
        {
            Throw();
        }
    }

    protected override void CheckForShoot()
    {
        if (Input.GetKeyDown(ShootKey) && CanShoot)
        {
            Shoot();
        }
    }
}
