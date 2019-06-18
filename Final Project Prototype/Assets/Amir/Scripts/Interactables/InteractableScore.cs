using UnityEngine;

public class InteractableScore : MonoBehaviour
{
    #region Fields
    [Header("Socre")]
    //[NamedArrayAttribute(new string[] { "None", "Damaged", "Working",  "Shielded" })]
    [SerializeField] private float[] scores = new float[4];
    #endregion Fields

    #region Methods
    public float IneractedSocre(int index)
    { return scores[index]; }
    #endregion Methods
}