using GamepadInput;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UINavigationHandler : MonoBehaviour
{
    #region Fields
    private Vector2 axis;
    private AxisEventData currentAxis;
    private GameObject currentButton;
    private float timeBetweenInputs = 0.1f;
    private float timer = 0;
    [SerializeField] private GameObject defaultButtonObj;
    [SerializeField] private GameObject backButtonObj;
    [SerializeField] private bool useAcceptBackBtn;
    [SerializeField] private bool useTabMovement;
    [ConditionalHide(nameof(useAcceptBackBtn), true)]
    [SerializeField] private GamePad.Button acceptBtn;
    [ConditionalHide(nameof(useAcceptBackBtn), true)]
    [SerializeField] private GamePad.Button backBtn;
    #endregion Fields

    #region Methods

    public void Back()
    {
        ExecuteEvents.Execute(backButtonObj, currentAxis, ExecuteEvents.submitHandler);
    }

    public void Submit()
    {
        currentButton = EventSystem.current.currentSelectedGameObject;
        ExecuteEvents.Execute(currentButton, currentAxis, ExecuteEvents.submitHandler);
    }

    private void ExecuteBack()
    {
        if (GamePad.GetButtonDown(backBtn, GamePad.Index.Any) && useAcceptBackBtn)
        {
            Back();
        }
    }

    private void ExecuteSubmit()
    {
        if (GamePad.GetButtonDown(acceptBtn, GamePad.Index.Any) && useAcceptBackBtn)
        {
            Submit();
        }
    }
    void TabMovement() {
        currentButton = EventSystem.current.currentSelectedGameObject;
        if (Input.GetKeyDown(KeyCode.Tab)) {
            currentAxis.moveDir = MoveDirection.Down;
            ExecuteEvents.Execute(currentButton, currentAxis, ExecuteEvents.moveHandler);
        }
    }
    private void Movement()
    {
        axis = GamePad.GetAxis(GamePad.Axis.LeftStick, GamePad.Index.Any);
        if (timer == 0)
        {
            currentButton = EventSystem.current.currentSelectedGameObject;
            if (axis.y > 0)
            {
                currentAxis.moveDir = MoveDirection.Up;
                ExecuteEvents.Execute(currentButton, currentAxis, ExecuteEvents.moveHandler);
                timer = timeBetweenInputs;
            }
            else if (axis.y < 0)
            {
                currentAxis.moveDir = MoveDirection.Down;
                ExecuteEvents.Execute(currentButton, currentAxis, ExecuteEvents.moveHandler);
                timer = timeBetweenInputs;
            }
            else if (axis.x < 0)
            {
                currentAxis.moveDir = MoveDirection.Right;
                ExecuteEvents.Execute(currentButton, currentAxis, ExecuteEvents.moveHandler);
                timer = timeBetweenInputs;
            }
            else if (axis.x > 0)
            {
                currentAxis.moveDir = MoveDirection.Left;
                ExecuteEvents.Execute(currentButton, currentAxis, ExecuteEvents.moveHandler);
                timer = timeBetweenInputs;
            }
        }
        if (timer > 0) { timer -= Time.deltaTime; } else { timer = 0; }
    }

    private void OnEnable()
    {
        if (defaultButtonObj != null)
        {
            EventSystem.current.SetSelectedGameObject(defaultButtonObj);
            currentButton = EventSystem.current.currentSelectedGameObject;
            currentAxis = new AxisEventData(EventSystem.current);
            currentAxis.moveDir = MoveDirection.None;
            ExecuteEvents.Execute(currentButton, currentAxis, ExecuteEvents.moveHandler);
        }
    }
    private void OnValidate()
    { if (acceptBtn == backBtn) { backBtn++; } }

    private void Update()
    {
        if (defaultButtonObj != null)
        {
            if (useTabMovement)
                TabMovement();
            else
                Movement();
            ExecuteSubmit();
        }
        if (backButtonObj != null)
            ExecuteBack();
    }
    #endregion Methods
}