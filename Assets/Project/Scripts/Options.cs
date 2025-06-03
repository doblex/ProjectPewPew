using UnityEngine;

public class Options : MonoBehaviour
{
    public void AdjustVolume(float newVolume)
    {
        AudioListener.volume = newVolume;
    }
}
