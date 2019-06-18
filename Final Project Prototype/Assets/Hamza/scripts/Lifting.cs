using System.Collections;
using UnityEngine;

public class Lifting : MonoBehaviour
{
    #region Fields
    public GameObject myParticle;
    public GameObject myParticle2;
    public Transform particleSpawnLoc;
    public float rate = 0.02f;
    public float yPos = 1.5f;
    private Vector3 endPosDown;
    private Vector3 endPosUP;
    private bool floating;
    private bool floatup;
    private ParticleSystem particleSystem2;
    private Vector3 startPos;
    Vector3 particleOffset=new Vector3(1.64f,0,-0.87f);
    #endregion Fields

    #region Methods
    public void Floating()
    {
        Instantiate(myParticle, particleSpawnLoc.position, Quaternion.identity);
        floatup = true;
        floating = true;
        StartCoroutine(Disappear());
    }

    private IEnumerator Disappear()
    {
        yield return new WaitForSeconds(3);
        Instantiate(myParticle2, particleSpawnLoc.position, Quaternion.identity);
        Destroy(gameObject, particleSystem2.main.duration - 1.5f);
    }

    private void floatingdown()
    {
        transform.position = Vector3.Lerp(transform.position, endPosDown, rate);
        if (Mathf.Abs(transform.position.y - endPosDown.y) < 0.1f) { floatup = true; }
    }

    private void floatingup()
    {
        transform.position = Vector3.Lerp(transform.position, endPosUP, rate);
        if (Mathf.Abs(transform.position.y - endPosUP.y) < 0.4f) { floatup = false; }
    }

    private void Start()
    {
        floatup = false;
        endPosUP = transform.position + transform.up * yPos;
        endPosDown = transform.position + transform.up * yPos / 1.2f;
        particleSystem2 = myParticle2.GetComponent<ParticleSystem>();
    }
    private void Update()
    {
        if (floating)
        {
            if (floatup) { floatingup(); }
            else { floatingdown(); }
        }
    }
    #endregion Methods
}