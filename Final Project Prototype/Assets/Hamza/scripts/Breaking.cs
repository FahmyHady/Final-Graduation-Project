using UnityEngine;

public class Breaking : MonoBehaviour

{
    #region Fields
    public GameObject myParticle;
    #endregion Fields

    #region Methods
    public void Explode()
    {
        GameObject ExplosionChild = Instantiate(myParticle, transform.position, Quaternion.identity);
        Destroy(this.gameObject, .25f);
        Destroy(ExplosionChild, 2.5f);
    }
    #endregion Methods
}