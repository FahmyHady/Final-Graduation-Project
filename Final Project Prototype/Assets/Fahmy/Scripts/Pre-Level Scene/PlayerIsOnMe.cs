using UnityEngine;

public class PlayerIsOnMe : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    { PreLevelManager.Manager.ReadyPlayerNum += 1; }

    private void OnTriggerExit(Collider other)
    { PreLevelManager.Manager.ReadyPlayerNum -= 1; }
}