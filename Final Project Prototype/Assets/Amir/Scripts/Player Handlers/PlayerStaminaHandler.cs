using UnityEngine;

public class PlayerStaminaHandler : MonoBehaviour
{
    #region Fields
   [SerializeField] private PlayerStateInfo info;
    private float stamina;
    #endregion Fields

    #region Methods
    public void OnStaminaChange()
    {
        if (info.CurrentStamina < info.MaxStamina && !IsInvoking(nameof(UpdateParSec)))
            RunUpdate();
    }

    private void RunUpdate()
    { InvokeRepeating(nameof(UpdateParSec), 0, 1); }

    private void Start()
    {
        info = this.GetComponent<PlayerStateInfo>();
        RunUpdate();
    }
    private void StopUpdate()
    { CancelInvoke(nameof(UpdateParSec)); }

    private void UpdateParSec()
    {
        if (info.CurrentStamina == info.MaxStamina)
        {
            StopUpdate();
            return;
        }
        stamina = info.CurrentStamina + (info.RegenRate);
        info.CurrentStamina = Mathf.Clamp(stamina, 0, info.MaxStamina);
    }
    #endregion Methods
}