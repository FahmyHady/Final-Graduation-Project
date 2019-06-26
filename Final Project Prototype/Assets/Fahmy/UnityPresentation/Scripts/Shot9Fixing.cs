using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shot9Fixing : MonoBehaviour
{
    Vector3 player1StartPos;
    Vector3 player2StartPos;
    Vector3 player3StartPos;
    GameObject item;
    public GameObject itemPrefab;
    public Transform itemSpawnPoint;
    public Child player1;
    public Child player2;
    public PresentationParent player3;
    bool created;
    public bool throwing;
    void Awake()
    {
        if (player1)
        {
            player1StartPos = player1.transform.position;

        }
        if (player2)
        {

            player2StartPos = player2.transform.position;
        }
        if (player3)
        {

            player3StartPos = player3.transform.position;
        }
        created = true;
    }
    private void OnEnable()
    {
        if (player1)
        {
            player1.transform.position = player1StartPos;

        }
        if (player2)
        {

            player2.transform.position = player2StartPos;
        }
        if (player3)
        {

            player3.transform.position = player3StartPos;
        }
        item = Instantiate(itemPrefab, itemSpawnPoint.position, itemSpawnPoint.rotation);
        created = true;

    }
    private void OnDisable()
    {
        if (item)
        {
            Destroy(item);

        }
    }
    void Update()
    {
        if (created)
        {
            if (throwing)
            {
                created = false;
                Invoke("waitToReEnable", 15);
            }
            if (!item && !throwing)
            {
                created = false;
                gameObject.SetActive(false);
                gameObject.SetActive(true);
            }
        }
    }

    void waitToReEnable()
    {
        gameObject.SetActive(false);
        gameObject.SetActive(true);
    }
}
