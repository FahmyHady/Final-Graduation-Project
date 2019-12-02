using System.Collections;
using UnityEngine;

public class Throw : MonoBehaviour
{
#region Fields
    public bool beingThrown;
    private int i;
    private int numberOfDivisions;
    private Transform objectToThrow;
    private Vector3[] points;
    [SerializeField]
    [Range(0.1f, 3)]
    private float rateOfThrow;
    private Rigidbody rb;
    private Vector3 Tangent;
    [SerializeField]
    private float tangentHeight;
    private Vector3 targetLocation;
    private BaseCharacter ThrownChar;
    private PlayerStateInfo ThrownCharInfo;

#endregion Fields

#region Methods


    public void ThrowSomething(Transform objectToThrowTransfrom, Vector3 targetLocationVector)
    {
        targetLocation = targetLocationVector;
        objectToThrow = objectToThrowTransfrom;

        ThrowBegan();
    }
    Vector3 CalculateCubicBezierPoint(float t, Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3)
    {
        float u = 1 - t;
        float tt = t * t;
        float uu = u * u;
        float uuu = uu * u;
        float ttt = tt * t;

        Vector3 p = uuu * p0;
        p += 3 * uu * t * p1;
        p += 3 * u * tt * p2;
        p += ttt * p3;
        return p;
    }
    Vector3[] CalculatePoints(Vector3 P0, Vector3 ControlP1, Vector3 ControlP2, Vector3 P3, int numberOfDivisions)
    {
        Vector3[] tempPoints = new Vector3[90];

        for (int i = 0; i < numberOfDivisions; i++)
        {
            tempPoints[i] = CalculateCubicBezierPoint(i * 1.0f / numberOfDivisions, P0, ControlP1, ControlP2, P3);
        }
        return tempPoints;
    }
    private void CalculateArc()
    {
        Tangent.x = (((targetLocation.x - objectToThrow.position.x)) / 2) + objectToThrow.position.x;
        Tangent.y = objectToThrow.position.y+ tangentHeight;
        Tangent.z = ((targetLocation.z - objectToThrow.position.z) / 2) + objectToThrow.position.z;
        numberOfDivisions = (int)(rateOfThrow * 60);
        points = CalculatePoints(objectToThrow.position, Tangent, Tangent, targetLocation, numberOfDivisions);
    }

    private void FixedUpdate()
    { if (beingThrown) { MoveOnCurve(); } }

    private void MoveOnCurve()
    {
        if (i < points.Length)
        {
            i++;
            objectToThrow.transform.LookAt(targetLocation);
            objectToThrow.root.position = points[i - 1];
        }
        else { StartCoroutine(ThrowFinished(ThrownChar)); }
    }

    private void ThrowBegan()
    {
        ThrownChar = objectToThrow.gameObject.GetComponentInParent<BaseCharacter>();
        rb = ThrownChar.rb;
        ThrownCharInfo = ThrownChar.myStateInfo;
        rb.detectCollisions = false;
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        rb.isKinematic = true;
        beingThrown = true;
        ThrownCharInfo.IsControllerInAir = beingThrown;
        CalculateArc();
    }

    private IEnumerator ThrowFinished(BaseCharacter character)
    {
        i = 0;
        beingThrown = false;
        character.rb.detectCollisions = true;
        rb.isKinematic = false;

        if (transform.position == Cloud.myLocation)
        {
            character.animator.Play("Landing");
            character.myStateInfo.IsControllerInAir = beingThrown;
            character.myStateInfo.IsControllerDisable = true;
            objectToThrow.transform.rotation = new Quaternion(0, objectToThrow.transform.rotation.y, 0, objectToThrow.transform.rotation.w);

            yield return new WaitUntil(() => character.animator.GetCurrentAnimatorStateInfo(0).IsName("Idle"));
            character.myStateInfo.IsControllerDisable = beingThrown;
        }
    }
#endregion Methods
}