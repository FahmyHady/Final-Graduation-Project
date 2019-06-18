using UnityEngine;

public class PortalHandler : MonoBehaviour
{
    #region Fields
    [SerializeField] private Light light;
    [SerializeField] private ParticleSystem ps;
    #endregion Fields

    #region Methods
    public void SetColor(Color color)
    {
        light.color = color;
#pragma warning disable CS0618 // Type or member is obsolete
        ps.startColor = color;
#pragma warning restore CS0618 // Type or member is obsolete
    }
    #endregion Methods
}