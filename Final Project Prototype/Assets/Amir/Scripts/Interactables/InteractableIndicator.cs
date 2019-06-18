using UnityEngine;
using UnityEngine.UI;

public class InteractableIndicator : MonoBehaviour
{
    #region Fields
    private bool currentState;
    private Vector3 m_cameraOffsetForward;
    private Vector3 m_cameraOffsetRight;
    private Vector3 m_cameraOffsetUp;
    [Space]
    [Range(0, 100)]
    [SerializeField] private float m_edgeBuffer;
    private RectTransform m_icon;
    private Image m_iconImage;
    private bool m_outOfScreen;
    [SerializeField] private Sprite m_targetIconOffScreen;
    [SerializeField] private Sprite m_targetIconOnScreen;
    [SerializeField] private Vector3 m_targetIconScale;
    private Camera mainCamera;
    [SerializeField] private Canvas mainCanvas;
    private Color offScreen;
    private Color onScreen;
    [Space]
    [SerializeField] private bool PointTarget = true;
    [SerializeField] private bool ShowDebugLines;
    bool isStarted;
    #endregion Fields

    #region Methods

    public void DrawDebugLines()
    {
        Vector3 directionFromCamera = transform.position - mainCamera.transform.position;
        Vector3 cameraForwad = mainCamera.transform.forward;
        Vector3 cameraRight = mainCamera.transform.right;
        Vector3 cameraUp = mainCamera.transform.up;
        cameraForwad *= Vector3.Dot(cameraForwad, directionFromCamera);
        cameraRight *= Vector3.Dot(cameraRight, directionFromCamera);
        cameraUp *= Vector3.Dot(cameraUp, directionFromCamera);
        Debug.DrawRay(mainCamera.transform.position, directionFromCamera, Color.magenta);
        Vector3 forwardPlaneCenter = mainCamera.transform.position + cameraForwad;
        Debug.DrawLine(mainCamera.transform.position, forwardPlaneCenter, Color.blue);
        Debug.DrawLine(forwardPlaneCenter, forwardPlaneCenter + cameraUp, Color.green);
        Debug.DrawLine(forwardPlaneCenter, forwardPlaneCenter + cameraRight, Color.red);
    }

    public Vector3 Vector3Maxamize(Vector3 vector)
    {
        Vector3 returnVector = vector;
        float max = 0;
        max = vector.x > max ? vector.x : max;
        max = vector.y > max ? vector.y : max;
        max = vector.z > max ? vector.z : max;
        returnVector /= max;
        return returnVector;
    }

    private void InstainateTargetIcon()
    {
        m_icon = new GameObject().AddComponent<RectTransform>();
        m_icon.transform.SetParent(mainCanvas.transform);
        m_icon.localScale = m_targetIconScale;
        m_icon.name = name + ": OTI icon";
        m_iconImage = m_icon.gameObject.AddComponent<Image>();
        m_iconImage.sprite = m_targetIconOnScreen;
        m_iconImage.color = onScreen;
    }

    private void Start()
    {
        onScreen = new Color(1, 1, 1, 0.2f);
        offScreen = new Color(1, 1, 1, 0.9f);
        mainCamera = Camera.main;
       // Debug.Assert((mainCanvas != null), "There needs to be a Canvas object in the scene for the OTI to display");
    }
    public void SetCanvas(Canvas _Canvas) {
        mainCanvas = _Canvas;
        InstainateTargetIcon();
        isStarted = true;
    }
    private void Update()
    {
        if (isStarted)
        {
            if (ShowDebugLines)
                DrawDebugLines();
            UpdateTargetIconPosition();
        }
    }

    private void UpdateTargetIconPosition()
    {
        Vector3 newPos = transform.position;
        newPos = mainCamera.WorldToViewportPoint(newPos);
        if (newPos.x > 1 || newPos.y > 1 || newPos.x < 0 || newPos.y < 0)
            m_outOfScreen = true;
        else
            m_outOfScreen = false;
        if (newPos.z < 0)
        {
            newPos.x = 1f - newPos.x;
            newPos.y = 1f - newPos.y;
            newPos.z = 1;
            newPos = Vector3Maxamize(newPos);
        }
        newPos = mainCamera.ViewportToScreenPoint(newPos);
        newPos.x = Mathf.Clamp(newPos.x, m_edgeBuffer, Screen.width - m_edgeBuffer);
        newPos.y = Mathf.Clamp(newPos.y, m_edgeBuffer, Screen.height - m_edgeBuffer);
        if (m_icon != null)
        {
            m_icon.transform.position = newPos;
            if (currentState != m_outOfScreen)
            {
                currentState = m_outOfScreen;
                if (m_outOfScreen)
                {
                    m_iconImage.sprite = m_targetIconOffScreen;
                    m_iconImage.color = offScreen;
                    m_iconImage.transform.localScale = m_targetIconScale;

                    if (PointTarget)
                    {
                        var targetPosLocal = mainCamera.transform.InverseTransformPoint(transform.position);
                        var targetAngle = -Mathf.Atan2(targetPosLocal.x, targetPosLocal.y) * Mathf.Rad2Deg - 90;
                        m_icon.transform.eulerAngles = new Vector3(0, 0, targetAngle);
                    }
                }
                else
                {
                    m_icon.transform.eulerAngles = new Vector3(0, 0, 0);
                    m_iconImage.sprite = m_targetIconOnScreen;
                    m_iconImage.color = onScreen;
                    m_iconImage.transform.localScale = m_targetIconScale * 0.9f;
                }
            }
        }
    }

    #endregion Methods
}