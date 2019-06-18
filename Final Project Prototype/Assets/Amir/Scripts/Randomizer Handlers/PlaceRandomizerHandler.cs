using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class PlaceRandomizerHandler : MonoBehaviour, IRandomize
{
    #region Fields
    private Transform child;
    [SerializeField] private Color color;
    [SerializeField] private bool hasManyUses;
    private int index;
    private System.Random rand = new System.Random();
    [SerializeField] private Mesh type;
    private List<int> usesIndex = new List<int>();
    [SerializeField] GameEvent @event;
    #endregion Fields

    #region Properties
    public bool HasManyUses { get => hasManyUses; set => hasManyUses = value; }
    #endregion Properties

    #region Methods

    public void CreatePlace() {
        var obj = new GameObject("Place");
        obj.transform.parent = this.transform;
        (obj.AddComponent<RandomPlaceIndecator>()).REvent = @event;
    }
    IEnumerator RanRandPlaceDelay(GameObject @object, float delay) {
        yield return new WaitForSeconds(delay);
        var obj = Instantiate(@object);
        Ranomize(ref obj);
        if (obj == null || obj?.GetComponent<RandomPlaceObj>().Place == null)
        {
            Destroy(obj);
        }
    }

    public void RandPlace(GameObject @object, float delay) {
        StartCoroutine(RanRandPlaceDelay(@object, delay));
    }
    public void Ranomize(ref GameObject @object)
    {
        Transform randTransform = GetLocation();
        if (randTransform == null)
            return;
        @object.transform.position = randTransform.position;
        @object.transform.rotation = randTransform.rotation;
        @object.transform.localScale = randTransform.localScale;
        @object.GetComponentInChildren<RandomPlaceObj>().Place = randTransform.GetComponent<RandomPlaceIndecator>();
    }

    public void ResetHandler()
    { usesIndex.Clear(); }
    public void RelesseKey(int value) {
        if (hasManyUses && usesIndex.Contains(value))
            usesIndex.Remove(value);
    }
    private Transform GetLocation()
    { 
        
        if (this.transform.childCount > 0 )
        {
            var li = this.transform.GetComponentsInChildren<RandomPlaceIndecator>().Where(i => i.IsFree).Select(i => i).ToList();
            if (li != null && li?.Count > 0)
            {
                index = (Mathf.FloorToInt((Random.Range(0, li.Count )) * (Random.value))) % (li.Count);
                //usesIndex.Add(index);
            }
            //do
            //{
            //    index = (Mathf.FloorToInt((Random.Range(0, this.transform.childCount - 1) + rand.Next(0, this.transform.childCount - 1)) * (Random.value + rand.Next()))) % (this.transform.childCount);
            //} while ((hasManyUses) && !(this.transform.GetChild(index).GetComponent<RandomPlaceIndecator>()).IsFree);
            return (li != null && li?.Count > 0) ? li[index].transform : null;
        }
        return null;
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = color;
        for (int i = 0; i < this.transform.childCount; i++)
        {
            child = this.transform.GetChild(i);
            if (child.GetComponent<RandomPlaceIndecator>() == null)
            {
                DestroyImmediate(child.gameObject);
            }
            else
            {
                Gizmos.DrawMesh(type, child.position, child.rotation, child.localScale);
            }
        }
    }
    #endregion Methods
}