using System.Collections;
using UnityEngine;

public class Coin : MonoBehaviour
{
    [Header("Coin properties")]
    [SerializeField] CoinType type;
    [SerializeField] int points = 1;
    [SerializeField, Range(0, 1)] public float shineStart;
    [SerializeField, Range(0.1f, 1)] float duration;

    [Header("Shining & Hit Effects")]
    [SerializeField] GameObject shiningEffect;
    [SerializeField] SkinnedMeshRenderer skinnedMesh;
    [SerializeField] float multiplier = 1.5f;

    [Header("AI Options")]
    [SerializeField] float launchDelay = 0;

    public float LaunchDelay { get => launchDelay; }
    public float ShineEnd { get => shineStart + duration; }
    public CoinType Type { get => type; }
    public int Points { get => points; }

    public void Start()
    {
        Shine(false);
    }

    public void Shine(bool isShining)
    {
        if (shiningEffect != null)
        {
            shiningEffect.SetActive(!isShining);
        }
    }

    public void Hit()
    {
        StartCoroutine(BlendShapeAnimate());
    }

    IEnumerator BlendShapeAnimate() 
    {
        float value = 0f;

        while (value < 100f)
        {
            skinnedMesh.SetBlendShapeWeight(0, value);
            value += 1 * multiplier;
            yield return null;
        }
    }
}
