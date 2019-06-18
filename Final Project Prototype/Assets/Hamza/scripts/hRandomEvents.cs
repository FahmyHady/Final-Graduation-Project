using UnityEngine;

public class hRandomEvents : MonoBehaviour
{
    #region Fields
    public GameObject[] Child;
    public float infetionLength = 20;
    private int childNum;
    private hplayerMove PlayerMove = null;
    private float timedEvent;
    private float timeStamp1;
    private float timeStamp2;
    #endregion Fields

    #region Methods
    public void Healed()
    {
        Child[childNum].GetComponentInChildren<Renderer>().material.color = new Color(255, 255, 255);
        Child[childNum].tag = "Child";
        Child[childNum].GetComponent<InfectionTrigger>().enabled = false;
        PlayerMove = Child[childNum].GetComponent<hplayerMove>();
        PlayerMove.speed -= 70;
    }

    public void Infected()
    {
        Child[childNum].GetComponentInChildren<Renderer>().material.color = new Color(0, 255, 0);
        Child[childNum].tag = "Infected";
        Child[childNum].GetComponent<InfectionTrigger>().enabled = true;
        PlayerMove = Child[childNum].GetComponent<hplayerMove>();
        PlayerMove.speed += 70;
    }

    private void getChilds()
    {
        childNum = Random.Range(0, Child.Length);
        Debug.Log((Child[childNum].name));
        Infected();
    }

    private void Start()
    {
        timeStamp1 = Time.time + 10;
        timeStamp2 = Time.time + 15;
        timedEvent = Random.Range(timeStamp1, timeStamp2);
        Invoke("getChilds", timedEvent);
        //PlayerMove = GetComponent<hplayerMove>();
    }
    private void Update()
    { if (Child[childNum].tag == "Infected" && Time.time >= timedEvent + infetionLength) { Healed(); } }
    #endregion Methods
}