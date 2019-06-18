using UnityEngine;

public class InteractableModel : MonoBehaviour
{
    #region Fields
    [SerializeField] private MeshFilter filter;

    [Header("Mesh")]
    [NamedArrayAttribute(new string[] { "None", "Damaged", "Working", "Shielded" })]
    [SerializeField] private Mesh[] meshes = new Mesh[4];

    [SerializeField] private GameObject prefab;

    #endregion Fields

    #region Methods
    private void Start()
    {
        Done();
    }
    public void IneractedMaterial(int index)
    {
        filter.mesh = meshes[index];
        spawnParticle();
    }

    private void spawnParticle()
    {

        //particles.transform.position = new Vector3(0, 0, 0);

        prefab?.SetActive(true);
            for (int i = 0; i < prefab.transform.childCount; i++)
            prefab.transform.GetChild(i).gameObject.SetActive(true);
        Invoke(nameof(Done), 2.0f);
        //particles.transform.position+= this.transform.position;
    }
    void Done() {
        for (int i = 0; i < prefab.transform.childCount; i++)
            prefab.transform.GetChild(i).gameObject.SetActive(false);
        prefab?.SetActive(false);
    }
    #endregion Methods
}