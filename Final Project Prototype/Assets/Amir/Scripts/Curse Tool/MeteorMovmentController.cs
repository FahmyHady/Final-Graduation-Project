using UnityEngine;

public class MeteorMovmentController : MonoBehaviour
{
    #region Fields
    [SerializeField] private GameEvent @event;
    [SerializeField] private float destroyTime;
    private Vector3 dir;
    [SerializeField] private GameObject explosion;
    [SerializeField] private GameObject indicator;
    private bool isFire;
    [SerializeField] private Transform model;
    [SerializeField] private float moveSpeed;
    [SerializeField] private ParticleSystem particle;
    private GameObject place;
    [SerializeField] private float rotSpeed;
    private bool shouldRiseEvent;
    [SerializeField] private Vector3 target;
    Rigidbody rb;
    #endregion Fields

    #region Methods

    public void Fire(Vector3 _target)
    {
        target = _target + new Vector3();
        isFire = true;
        place = Instantiate(indicator, new Vector3(_target.x, _target.y, _target.z), Quaternion.identity);
        shouldRiseEvent = true;
    }

    private void Destroy()
    {
        if (model != null)
            Destroy(model?.gameObject);
        if (!particle.isStopped)
            particle.Stop();
        //  explosion.SetActive(true);
    }

    private void OnDestroy()
    { if (shouldRiseEvent) @event.Raise(); }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Child" && model != null)
        {
            shouldRiseEvent = !other.gameObject.GetComponentInParent<PlayerStateInfo>().IsControllerBurned;
            (other.transform.gameObject.GetComponentInChildren<VolcanoCurseHandler>() as ICurseHandler)?.Curse();
        }
        Destroy();
        Instantiate(explosion, transform.position, Quaternion.identity);
        rb.detectCollisions = false;
        Destroy(this.gameObject, destroyTime);
        Destroy(place?.gameObject);
        model = null;
       AudioManager.Play(AudioManager.AudioItems.Event, "MeteorCrash");
    }

    private void Update()
    {
        if (!isFire) return;
        dir = target - this.transform.position;
        if (dir.magnitude > 0.2f) this.transform.Translate(dir.normalized * moveSpeed);
        else
        {
            Destroy();
            Destroy(place?.gameObject);
            Destroy(this.gameObject, destroyTime);
        }

        if (model != null)
            model?.Rotate(Vector3.one * Time.deltaTime * rotSpeed);
    }
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        AudioManager.Play(AudioManager.AudioItems.Event, "MeteorFalling");

    }
    #endregion Methods
}