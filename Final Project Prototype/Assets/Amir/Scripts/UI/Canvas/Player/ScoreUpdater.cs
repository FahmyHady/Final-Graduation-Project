using UnityEngine;
using UnityEngine.UI;

public class ScoreUpdater : MonoBehaviour
{
    #region Fields
    [SerializeField] private Text myText;
    [SerializeField] private PlayerStateInfo stateInfo;
    #endregion Fields

    #region Methods
    // End Testing
    public void UpdateText() { myText.text = stateInfo.PlayerInfo(); }

    // Just For Testing
    private void Start()
    {
        myText.color = stateInfo.gameObject.transform.GetComponentInChildren<MeshRenderer>().material.color;
        UpdateText();
    }
    #endregion Methods
}