using UnityEngine;

public class EarthquakeHandler : MonoBehaviour, ICurseHandler
{
    #region Fields
    [SerializeField] private GameObject confuseEff;
    [SerializeField] private PlayerStateInfo info;
    #endregion Fields

    #region Methods
    private void Start()
    { info = this.GetComponentInParent<PlayerStateInfo>(); }

    public void Curse()
    {
        info.IsControllerConfused = true;
        confuseEff.SetActive(true);
    }
    public void StopCurse()
    {
        info.IsControllerConfused = false;
        confuseEff.SetActive(false);
    }
    #endregion Methods
}