using UnityEngine;

public class PlayerIsOnMe : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        PreLevelManager.Manager.ReadyPlayerNum += 1;
        AudioManager.Play(AudioManager.AudioItems.Teleport, "TeleportEnter");
    }

    private void OnTriggerExit(Collider other)
    { PreLevelManager.Manager.ReadyPlayerNum -= 1; }
}