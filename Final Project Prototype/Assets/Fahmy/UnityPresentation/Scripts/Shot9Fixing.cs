using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shot9Fixing : MonoBehaviour
{
    Vector3 player1StartPos;
    Vector3 player2StartPos;
    GameObject item;
    public GameObject itemPrefab;
    public Transform itemSpawnPoint;
    public Child player1;
    public Child player2;
    bool created;
    void Awake()
    {
        player1StartPos = player1.transform.position;
        player2StartPos = player2.transform.position;
        item = Instantiate(itemPrefab, itemSpawnPoint.position, itemSpawnPoint.rotation);
        created = true;
    }
    private void OnEnable()
    {
        player1.transform.position = player1StartPos;
        player2.transform.position = player2StartPos;
        item = Instantiate(itemPrefab, itemSpawnPoint.position, itemSpawnPoint.rotation);
        created = true;

    }
    void Update()
    {
        if (created)
        {

            if (!item)
            {
                created = false;
                gameObject.SetActive(false);

                gameObject.SetActive(true);
            }
        }
    }
}
