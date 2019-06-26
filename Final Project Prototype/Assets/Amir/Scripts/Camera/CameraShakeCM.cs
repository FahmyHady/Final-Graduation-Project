using Cinemachine;
using UnityEngine;

public class CameraShakeCM : MonoBehaviour
{
    #region Fields
    private float repeatRate = 0.5f;
    [SerializeField] private float ShakeAmplitude = 1.2f;
    [SerializeField] private float ShakeDuration = 0.3f;
    private float ShakeElapsedTime = 0.0f;
    [SerializeField] private float ShakeFrequency = 2.0f;
    [SerializeField] private CinemachineVirtualCamera VirtualCamera;
    private CinemachineBasicMultiChannelPerlin virtualCameraNoise;
    #endregion Fields

    #region Methods

    public void StartShake()
    {
        ShakeElapsedTime = 0.0f;
        virtualCameraNoise.m_AmplitudeGain = ShakeAmplitude;
        virtualCameraNoise.m_FrequencyGain = ShakeFrequency;
        InvokeRepeating(nameof(UpdateParSec), 0, repeatRate);
    }

    private void Start()
    {
        if (VirtualCamera != null)
        {
            virtualCameraNoise = VirtualCamera.GetCinemachineComponent<Cinemachine.CinemachineBasicMultiChannelPerlin>();
            virtualCameraNoise.m_AmplitudeGain = 0f;
        }
    }

    private void UpdateParSec()
    {
        ShakeElapsedTime += repeatRate;
        if (ShakeElapsedTime >= ShakeDuration)
        {
            virtualCameraNoise.m_AmplitudeGain = 0f;
            ShakeElapsedTime = 0f;
            CancelInvoke(nameof(UpdateParSec));
        }
    }

    #endregion Methods
}