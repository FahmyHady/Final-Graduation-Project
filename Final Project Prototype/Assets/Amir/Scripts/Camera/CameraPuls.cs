using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class CameraPuls : MonoBehaviour
{
    [SerializeField] private PostProcessProfile profile;
    [SerializeField] private PostProcessVolume volume;
    [SerializeField] private float timeFaded;
    [SerializeField] private float loopRate = 0.1f;
    private float elpTime;
    private float lerpStepValee = 0.01f;
    private float lerpStep = 1f;
    float startLerp = 0.7f;
    float endLerp = 0.95f;
    public void StartPlue()
    {
        //volume.profile = profile;
        //elpTime = 0.0f;
        //InvokeRepeating(nameof(UpdatePerSec), 0, loopRate);
    }

    private void UpdatePerSec()
    {
        elpTime += loopRate;
        lerpStep += loopRate;
        if (elpTime >= timeFaded)
        {
            elpTime = 0.0f;
            lerpStep *= -1;
            var t = startLerp;
            startLerp = endLerp;
            endLerp = t;
            lerpStepValee = 0;
        }
        else
        {
            lerpStepValee += lerpStep * 0.1f;
        }
        volume.weight += lerpStepValee;
    }
}