#define  TryToInstantiateBehindMe
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Collider))]
public class SlowArea : BaseSkill
{
    BaseCharacter otherplayer;
  static  Transform parent;
    [SerializeField]
    [Range(0.4f, 0.9f)]
    float slowEffect;
    [SerializeField]
    [Range(1, 5)]
    float activeDuration;
    [SerializeField]
    [Range(0.1f,1)]
    float offSetMiniSmoke;

    Vector3 raycastCheckOrigin;
    Vector3 raycastCheckOrigin2;
    Vector3 smokeBounds;
    float timeStamp;
    Dictionary<Collider, BaseCharacter> playersInArea = new Dictionary<Collider, BaseCharacter>();
    float numberOfPlayerSlowed;
    Collider myCollider;
    LayerMask layerMask=1<<10;
    LayerMask playerLayerMask = 1 << 9;
    private RaycastHit ray;

    private void Awake()
    {
        myCollider = GetComponent<Collider>();

        if (transform.parent)
        {
            smokeBounds = new Vector3(myCollider.bounds.extents.x, myCollider.bounds.extents.y, myCollider.bounds.extents.z);
            parent = transform.parent;
        }

    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 9 && !playersInArea.ContainsKey(other) && other.transform != parent )
        {
           
            playersInArea.Add(other, other.attachedRigidbody?.gameObject.GetComponent<BaseCharacter>());
            if (!playersInArea[other].isSlowed)
            {
                playersInArea[other]?.Slowed(100, slowEffect);
                numberOfPlayerSlowed += 1;
            }
        
        }

    }
    private void OnTriggerExit(Collider other)
    {

        if (playersInArea.ContainsKey(other))
        {
            playersInArea[other].Slowed(0, slowEffect);
            playersInArea.Remove(other);
            numberOfPlayerSlowed -= 1;
        }
     
    }

    private void OnDestroy()
    {
        CheckIfPlayersAreStillSlowedBeforeDisableOrDestroy();
    }
    private void OnDisable()
    {
        CheckIfPlayersAreStillSlowedBeforeDisableOrDestroy();
    }
    private void OnEnable()
    {
        timeStamp = Time.time + activeDuration; //Calculates how long the skill is active to disable it after set duration
    }
    private void Update()
    {
        if (parent && transform.IsChildOf(parent))
        {
#if TryToInstantiateBehindMe
            SpawnSlowArea();
#endif
        }
        if (Time.time >= timeStamp)
        {
            ResetSkill();
        }
    }
    private void ResetSkill()
    {
        if (parent && transform.IsChildOf(parent))
        {
            gameObject.SetActive(false);

        }
        else
        {
            Destroy(gameObject);
            playersInArea.Remove(myCollider);
            numberOfPlayerSlowed -= 1;
        }
    }

    void CheckIfPlayersAreStillSlowedBeforeDisableOrDestroy()
    {
        if (numberOfPlayerSlowed != 0)
        {
            foreach (KeyValuePair<Collider, BaseCharacter> item in playersInArea)
            {
                item.Value.Slowed(0, slowEffect);
                item.Value.isSlowed = false;
            }
            playersInArea.Clear();
        }
    }
#if TryToInstantiateBehindMe
    void SpawnSlowArea()
    {
        raycastCheckOrigin = transform.position - (transform.forward * smokeBounds.z) + (transform.right* smokeBounds.x) - transform.forward * offSetMiniSmoke;
        raycastCheckOrigin2 =( transform.position - (transform.forward * smokeBounds.z ) + (-transform.right * smokeBounds.x )) - transform.forward * offSetMiniSmoke;


        if (!Physics.CheckCapsule( raycastCheckOrigin2,  raycastCheckOrigin , 0.2f, layerMask.value))
        {

            Instantiate(gameObject, (raycastCheckOrigin-transform.forward* offSetMiniSmoke-transform.right * smokeBounds.x) , transform.rotation);

        }

    }
    private void OnDrawGizmos()
    {
        if (parent)
        {
           // DebugExtension.DrawCapsule(raycastCheckOrigin, raycastCheckOrigin - transform.forward * offSetMiniSmoke, 0.2f);
              DebugExtension.DrawCapsule(raycastCheckOrigin2 , raycastCheckOrigin , 0.2f);
        }

    }
#endif
}
