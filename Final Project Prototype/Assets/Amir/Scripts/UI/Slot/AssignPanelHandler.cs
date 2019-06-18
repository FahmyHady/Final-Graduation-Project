using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AssignPanelHandler : MonoBehaviour
{
    #region Fields
    [SerializeField] private TextMeshProUGUI assigneText;
    [SerializeField] private Image BGImg;
    [SerializeField] private Image pxImg;
    #endregion Fields

    #region Methods
    public void SetControllerName(string str)
    { assigneText.text = str; }

    public void SetPanelColor(Color pColor)
    {
        BGImg.color = pColor;
        pxImg.color = pColor;
    }

    private void Start() { }

    private void Update() { }
    #endregion Methods
}