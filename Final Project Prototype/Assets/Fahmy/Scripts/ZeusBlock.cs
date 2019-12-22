using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZeusBlock : MonoBehaviour
{
    public GameObject effectPrefab;
    public void Vanish()
    {
        Instantiate(effectPrefab, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

}
