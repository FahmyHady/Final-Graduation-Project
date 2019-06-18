using UnityEngine;
using UnityEngine.UI;

public class StaminaUpdater : MonoBehaviour
{
    #region Fields
    [SerializeField] private Image fillImg;
    [SerializeField] private PlayerStateInfo info;
    [SerializeField] private Slider slider;
    #endregion Fields

    #region Methods
    public void UpdateStamina()
    {
        fillImg.color = info.Player.Outline;
        slider.value = info.CurrentStamina / info.MaxStamina;
    }

    private void Start()
    {
        fillImg.color = info.Player.Outline;
        slider.value = info.CurrentStamina / info.MaxStamina;
    }
    #endregion Methods
}