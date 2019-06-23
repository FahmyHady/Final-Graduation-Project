using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class RandomPlaceManager : MonoBehaviour
{
    [SerializeField] List<PlaceRandom> places;
    [SerializeField] float delay;
    GameObject obj;
    int value;
    void Start()
    {
        
    }
    public void StartRandom() {
        foreach (PlaceRandom place in places) {
            if (place.placeObjs?.Count != 0)
            {
                while (true)
                {
                    value = Random.Range(1, place.placeObjs.Count - 1);
                    obj = Instantiate(place.placeObjs[value]);
                    place.place.Ranomize(ref obj);
                    if (obj.GetComponentInChildren<RandomPlaceObj>().Place == null)
                    {
                        Destroy(obj);
                        break;
                    }
                    obj.GetComponentInChildren<InteractableIndicator>()?.SetCanvas(RoundManager.Instance.MainCanvas);
                }
            }
        }
    }
    public void RandPlace(PlaceRandomizerHandler placeRandom) {
        var placeRand = places.Find(i => i.place = placeRandom);
        if (placeRand.Equals(default(PlaceRandom)))
            return;
        int randWeight = Random.Range(placeRand.minWeight, placeRand.maxWeight);
        var SelectedObj = placeRand.placeObjs.Where(i => i.GetComponentInChildren<RandomPlaceObj>().Amount <= randWeight).Select(i => i).ToList();
        value = Random.Range(0, SelectedObj.Count - 1);
        placeRand.place.RandPlace(SelectedObj[value], delay);
    }
}
[System.Serializable]
public struct PlaceRandom {
    [Header("Place Random")]
    public PlaceRandomizerHandler place;
    public int minWeight;
    public int maxWeight;
    public List<GameObject> placeObjs;
    
}
