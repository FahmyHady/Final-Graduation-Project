using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class InteractedPlace : MonoBehaviour
{
    #region Fields
    [SerializeField] private GameEvent @eventEnter;
    [SerializeField] private GameEvent @eventExit;
    [SerializeField] private SphereCollider collider;
    private bool isLocked;
    [SerializeField] [TagSelector] private string playerTag;
    #endregion Fields

    #region Methods
    private void OnTriggerEnter(Collider other)
    {
        
        if (other.gameObject.CompareTag(playerTag) && !isLocked)
        {
            other.GetComponent<PlayerInteractor>().IsPlaced = true;
            isLocked = true;
            @eventEnter?.Raise();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag(playerTag) && other.GetComponent<PlayerInteractor>().IsPlaced)
        {
            other.GetComponent<PlayerInteractor>().IsPlaced = false;
            isLocked = false;
            @eventExit?.Raise();
        }
    }

    

    private void Start()
    {
        collider.isTrigger = true;
    }
    #endregion Methods
}