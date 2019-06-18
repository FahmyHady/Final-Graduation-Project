using UnityEngine;

public class VolcanoCurseHandler : MonoBehaviour, ICurseHandler
{
    #region Fields
    [SerializeField] private GameEvent @event;
    [SerializeField] private GameObject fireEff;
    [SerializeField] private PlayerStateInfo info;
    #endregion Fields

    #region Methods
    public void Curse()
    {
        info.IsControllerBurned = true;
        fireEff.SetActive(true);
    }

    public void StopCurse()
    {
        info.IsControllerBurned = false;
        fireEff.SetActive(false);
        @event.Raise();
    }

    private void Start()
    { info = this.GetComponentInParent<PlayerStateInfo>(); }
    #endregion Methods
}