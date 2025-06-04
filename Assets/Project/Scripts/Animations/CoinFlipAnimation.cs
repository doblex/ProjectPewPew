using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class CoinFlipAnimation : MonoBehaviour
{
    [SerializeField] GameObject coin;
    [SerializeField] float force = 5f;
    [SerializeField] string coinTag = "Coin";

    bool isCoinThrown = false;
    Action Action;

    public void ThrowCoin(Action action) 
    {
        Action = action;
        coin.SetActive(true);
        isCoinThrown = true;
        Rigidbody rb = coin.GetComponent<Rigidbody>();
        rb.AddForce(Vector3.up * force, ForceMode.Impulse);
        rb.AddTorque(Random.insideUnitSphere, ForceMode.Impulse);
    }

    private void Start()
    {
        isCoinThrown = false;
        //Invoke(nameof(ThrowCoin), 2f); // Delay the coin throw by 1 second
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag(coinTag) && isCoinThrown)
        {
            // Optionally, you can add logic here to handle what happens when the coin lands on the table
            coin.SetActive(false);
            Debug.Log("Coin landed on the table!");
            Action?.Invoke();
        }
    }

}
