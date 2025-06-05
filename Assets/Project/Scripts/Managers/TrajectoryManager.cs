using System.Collections;
using UnityEngine;

public class TrajectoryManager : MonoBehaviour
{
    public delegate void OnThrowEnded(bool hit);
    public delegate void OnAIShoot(bool hit);

    public OnThrowEnded onThrowEnded;
    public OnAIShoot onAIShoot;

    public static TrajectoryManager Instance;

    [SerializeField] float duration = 2.0f;

    PG currentShootingPlayer;
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

        if (shootingPlayer.playerType == PlayerType.AI){ ((AI)shootingPlayer).AddShootEvent(this); }
        shootingPlayer.onPlayerShoot += OnShoot;

        currentShootingPlayer = shootingPlayer;

        throwingPlayer.onPlayerThrow += OnThrow;
        currentCoinTrowed = Instantiate(coin.gameObject, trajectory.Start, Quaternion.identity).GetComponent<Coin>();
        TurnManager.Instance.TurnPhase = TurnPhase.PassiveThrow;
        throwingPlayer.CanTrow = true;
        shootingPlayer.CanShoot = true;
        isHit = false;
    }

    void OnThrow(PG p)
    {
        p.onPlayerThrow -= OnThrow;
        p.CanTrow = false;
        AudioManager.Instance.PlayThrowSound();
        StartCoroutine(MoveOnTrajectory(trajectory.Spline));
        TurnManager.Instance.TurnPhase = TurnPhase.ActiveQuickTimeEvent;
    }

    void OnShoot(PG p)
    {
        p.onPlayerShoot -= OnShoot;

        if (isShining)
        {
            if (p.playerType == PlayerType.PLAYER)
            {
                AudioManager.Instance.PlayHitSound();
                currentCoinTrowed.Hit();
                isHit = true;
            }
        }
        else if (p.playerType == PlayerType.PLAYER)
        {
            AudioManager.Instance.PlayMissSound();
        }

        p.CanShoot = false;

        if (p.playerType == PlayerType.AI)
        {
            ((AI)p).RemoveShootEvent(this);
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
        Destroy(currentCoinTrowed.gameObject);
        isShining = false;
        onThrowEnded?.Invoke(isHit);
    }

    private void CheckForShining(float amount)
    {
        if ((amount >= currentCoinTrowed.shineStart && !isShining && amount <= currentCoinTrowed.ShineEnd) && !currentCoinTrowed.IsOxydated)
        {
            isShining = true;
            Debug.Log("Shining!!!");
            currentCoinTrowed.Shine(true);

            if (currentShootingPlayer.playerType == PlayerType.AI)
            {
                int rnd = Random.Range(1, 101);
                bool aiHit = false;
                Debug.Log("AI Shooting Outcome " + rnd.ToString() + "/" + currentCoinTrowed.AIHitProbability);

                if (rnd <= currentCoinTrowed.AIHitProbability)
                {
                    aiHit = true;
                    isHit = true;
                }
                else
                {
                    isHit = false;
                }

                onAIShoot?.Invoke(aiHit);
            }
        }

        if (amount >= currentCoinTrowed.ShineEnd && isShining)
        {
            isShining = false;
            Debug.Log("Shining Stopped");
            currentCoinTrowed.Shine(false);
        }
    }
}
