using System.Collections;
using UnityEngine;

public class Cloud : MonoBehaviour
{
    #region Fields
    public static Vector3 myLocation;
    [SerializeField] private float delayOfThrowBack;
    // [SerializeField] private Collider[] myBounds;
    [SerializeField] Transform[] LandingPoints;
    private Throw myThrowSkill;
    GameObject particleEffect;
    #endregion Fields

    #region Methods
    private void Start()
    {
        particleEffect = transform.GetChild(0).gameObject;
        myLocation = transform.position;
        myThrowSkill = GetComponent<Throw>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer != 12)
        {
            particleEffect.SetActive(true);
            StartCoroutine(ThrowBack(other.transform));
            AudioManager.Play(AudioManager.AudioItems.Skill, "FallInCloud");

        }
    }

    private Vector3 RandomPointOnPlatfrom()
    {
        Vector3 targetPoint;
        int pointToThrowTo = Random.Range(0, LandingPoints.Length);
        targetPoint = LandingPoints[pointToThrowTo].position;
        return targetPoint;
    }
    private IEnumerator ThrowBack(Transform objectToThrow)
    {
        objectToThrow.root.gameObject.SetActive(false);
        yield return new WaitForSeconds(delayOfThrowBack);
        objectToThrow.root.gameObject.SetActive(true);
        objectToThrow.gameObject.GetComponentInChildren<VolcanoCurseHandler>().StopCurse();
        myThrowSkill.ThrowSomething(objectToThrow, RandomPointOnPlatfrom());
    }
    #endregion Methods

}