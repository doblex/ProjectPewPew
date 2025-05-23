using UnityEngine;


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
        //TODO Parte Grafica Monete
        index = 0;
        return index;
    }

    public override Trajectory ChooseCoinTrajectory(Trajectory[] trajectories, out Item item)
    {
        int index = -1;
        item = null;
        //TODO Parte Grafica sia Item che traiettoria
        index = 0;
        return trajectories[index];
    }

    public override Trajectory ChooseEnemyTrajectory(Trajectory[] trajectories)
    {
        int index = -1;
        //TODO Parte Grafica Scelta traiettoria nemica
        index = 0;
        return trajectories[index];
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
