using UnityEngine;

[RequireComponent(typeof(QTEController))]
[RequireComponent(typeof(TimeHoldinghandler))]
[RequireComponent(typeof(InteractableScore))]
[RequireComponent(typeof(SphereCollider))]
[ExecuteInEditMode]
public class Interactable : MonoBehaviour
{
    #region Fields
    [SerializeField] private bool conditionalInteractionPlace;
    [SerializeField] private float destroyDelay;
    [SerializeField] private GameEvent fixedEvent;
    private bool hasInteracted = false;
    private int holdedPlaces;

    [ConditionalHide(nameof(isTimed), true)]
    [SerializeField] private float holdTime;

    private int interactedPlaces;

    [ConditionalHide(nameof(conditionalInteractionPlace), true)]
    [SerializeField] private int interactionPlacesNum;

    [SerializeField] private Transform interactionTransform;
    private bool isInteracting = true;
    [SerializeField] [HideInInspector] private bool isPattern = false;
    [SerializeField] private bool isTimed = true;

    [Range(1, 4)]
    [SerializeField] private int maxInteractors = 1;

    [SerializeField] private InteractableModel model;

    [ConditionalHide(nameof(isPattern), true)]
    [SerializeField] private PatternDifficulty pattern;

    [SerializeField] private RandomPlaceObj placeObj;
    [SerializeField] [TagSelector] private string playerTag;
    [SerializeField] private InteractableScore score;
    [SerializeField] private SphereCollider sphCollider;
    [SerializeField] private InteractableType type;
    #endregion Fields

    #region Properties
    public bool HasInteracted
    {
        get
        {
            if (conditionalInteractionPlace)
            {
                return isInteracting;
            }
            else
            {
                return hasInteracted;
            }
        }
        set => hasInteracted = value;
    }
    public float HoldTime { get => holdTime; }
    public bool IsTimed { get => isTimed; }
    public int Pattern { get => (int)pattern; }
    public float Score { get => score.IneractedSocre((int)Type); }
    public InteractableType Type { get => type; set { type = value; HandleModelMaterial(); } }
    #endregion Properties

    #region Methods

    public void FixedItemDone()
    {
        fixedEvent.Raise();
        AudioManager.Play(AudioManager.AudioItems.Interactable, "Fixed");

        Destroy(this.transform.parent.gameObject, destroyDelay);
    }

    public void HoldPlace()
    {
        holdedPlaces = Mathf.Clamp(holdedPlaces + 1, 0, maxInteractors);
        if (holdedPlaces == maxInteractors) hasInteracted = true;
    }

    public virtual void Interact()
    { model.IneractedMaterial((int)Type); }

    public void InteractedPlaceEnter()
    {
        interactedPlaces++; CheckInteractedPlaces();
    }

    public void InteractedPlaceExit()
    {
        interactedPlaces--; CheckInteractedPlaces();
    }

    public void RelesePlace()
    {
        holdedPlaces = Mathf.Clamp(holdedPlaces - 1, 0, maxInteractors);
        if (holdedPlaces < maxInteractors) hasInteracted = false;
    }

    private void CheckInteractedPlaces()
    {
        if (interactedPlaces == interactionPlacesNum) { isInteracting = false; }
        else { isInteracting = true; }
    }

    private void HandleModelMaterial()
    { model.IneractedMaterial((int)Type); }

    private void OnDestroy()
    {
        placeObj.Place = null;
    }

    private void OnTriggerEnter(Collider other)
    { if (other.gameObject.CompareTag(playerTag)) { other.GetComponent<PlayerInteractor>().Enter(this); } }

    private void OnTriggerExit(Collider other)
    { if (other.gameObject.CompareTag(playerTag)) { other.GetComponent<PlayerInteractor>().Exit(); } }

    private void OnValidate()
    {
        isPattern = !IsTimed;
        HandleModelMaterial();
        if (!IsTimed) holdTime = 0.0f;
        else pattern = PatternDifficulty.Easy;
        isInteracting = conditionalInteractionPlace;
        if (!conditionalInteractionPlace)
            interactedPlaces = 0;
    }

    private void Start()
    {
        //sphCollider = this.GetComponent<SphereCollider>();
        AudioManager.Play(AudioManager.AudioItems.Interactable, "Spawn");
        sphCollider.isTrigger = true;
        interactionTransform = this.transform;
        RoundManager.Instance?.SetInteractable();
        placeObj = GetComponent<RandomPlaceObj>();
    }

    #endregion Methods
}