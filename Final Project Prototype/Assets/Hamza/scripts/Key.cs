using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : MonoBehaviour
{
    #region Fields
    public GameObject Door;
    GameObject doorChild;
    public GameObject lightingStrike;
    //public float Doorspeed = 1.0f;

    public Transform strDoorPos, endDoorPos;
    public Transform startPos, endPos;
    public bool repeated = false;
    public float speed = 1.0f;
    public float doorspeed = 1.0f;
    public float duration = 3.0f;
    bool interacted = false;
    float startTime, totalDistance, totalDistance2;

    #endregion Fields

    #region Methods

    IEnumerator Start()
    {
        startTime = Time.time;
        totalDistance = Vector3.Distance(startPos.position, endPos.position);
        totalDistance2 = Vector3.Distance(strDoorPos.position, endDoorPos.position);
        while(repeated)
        {
            yield return repeatLerp(startPos.position, endPos.position, duration);

            yield return repeatLerp(endPos.position, startPos.position, duration);
            repeated = false;
        }
    }

    public void vanish()
    {
        Destroy(Instantiate(lightingStrike, transform.position, transform.rotation), 3);
        Destroy(Instantiate(lightingStrike, Door.transform.position, Door.transform.rotation), 3);
        interacted = true;
        //Door.SetActive(false);
    }

    private void Update()
    {
        if(!repeated&&interacted)
        {
            float currentDuration = (Time.time - startTime) * speed;
            float journeyFraction = currentDuration / totalDistance;
            //-------Door Lerping Rate------
            float currentDuration2 = (Time.time - startTime) * doorspeed;
            float journeyFraction2 = currentDuration2 / totalDistance;
            this.transform.position = Vector3.Lerp(startPos.position, endPos.position, journeyFraction);
            Door.transform.position = Vector3.Lerp(strDoorPos.position, endDoorPos.position, journeyFraction2);
        }
    }

    public IEnumerator repeatLerp(Vector3 a, Vector3 b, float time)
    {
        float i = 0.0f;
        float rate = (1.0f / time) * speed;
       
        while (i < 1.0f)
        {
            i += Time.deltaTime * rate;
            this.transform.position = Vector3.Lerp(a, b, i);
            //Door.transform.position = Vector3.Lerp(a, b, i);
            yield return null;
        }
    }
    #endregion Methods
}