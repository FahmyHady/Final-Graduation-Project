using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zone : MonoBehaviour
{
    public int StageToMoveToNumber;
    public Collider turnToWall;
    List<GameObject> itemsEntered = new List<GameObject>();
    private void OnTriggerEnter(Collider other)
    {
        if (!itemsEntered.Contains(other.gameObject) && (other is CapsuleCollider) && (other.gameObject.CompareTag("Parent") || other.gameObject.CompareTag("Child")))
        {
            itemsEntered.Add(other.gameObject);
            if (itemsEntered.Count == GameplayLevelManager.instance.children.Count)
            {
                GameplayLevelManager.instance.MoveToNextCamera(StageToMoveToNumber);
                turnToWall.transform.parent = null;
                turnToWall.isTrigger = false;
                Destroy(gameObject);
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if ((other.gameObject.CompareTag("Parent") || other.gameObject.CompareTag("Child")))
        {
            itemsEntered.Remove(other.gameObject);
        }
    }

}
