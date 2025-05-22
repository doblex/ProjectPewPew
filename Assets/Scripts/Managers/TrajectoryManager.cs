using System.Collections;
using UnityEngine;

public class TrajectoryManager : MonoBehaviour
{
    public delegate void OnThrowEnded(bool hit);
    public OnThrowEnded onThrowEnded;

    public static TrajectoryManager Instance;

    [SerializeField] float duration = 2.0f;

    Coin currentCoinTrowed;
    Trajectory trajectory;

    bool isShining = false;
    bool isHit = false;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    public void SpawnCoin(PG shootingPlayer, PG throwingPlayer, Trajectory trajectory, Coin coin)
    {
        this.trajectory = trajectory;
        shootingPlayer.onPlayerShoot += OnShoot;
        throwingPlayer.onPlayerThrow += OnThrow;
        currentCoinTrowed = Instantiate(coin.gameObject, trajectory._startPoint, Quaternion.identity).GetComponent<Coin>();

        throwingPlayer.CanTrow = true;
        isHit = false;
    }

    void OnThrow(PG p)
    {
        p.onPlayerThrow -= OnThrow;
        p.CanTrow = false;

        StartCoroutine(MoveOnTrajectory(trajectory.Spline));
    }

    void OnShoot(PG p)
    {
        p.onPlayerShoot -= OnShoot;
        if (isShining)
        {
            currentCoinTrowed.Hit();
            isHit = true;
        }
    }

    private IEnumerator MoveOnTrajectory(Spline spline)
    {
        float currentTime = 0;
        Vector3 coinStartPosition = currentCoinTrowed.transform.position;
        
        while (currentTime < duration)
        {
            currentTime += Time.deltaTime;

            float amount = Mathf.Clamp01(currentTime / duration);
            CheckForShining(amount);

            currentCoinTrowed.transform.position = spline.CalculatePositionCustomStart(amount, coinStartPosition);

            yield return null;
        }

        currentCoinTrowed.transform.position = trajectory._endPoint;
        onThrowEnded?.Invoke(isHit);
    }

    private void CheckForShining(float amount)
    {
        if (amount >= currentCoinTrowed.shineStart && !isShining && amount <= currentCoinTrowed.ShineEnd)
        {
            isShining = true;
            currentCoinTrowed.Shine(true);
        }

        if (amount >= currentCoinTrowed.ShineEnd)
        {
            isShining = false;
            currentCoinTrowed.Shine(false);
        }
    }
}
