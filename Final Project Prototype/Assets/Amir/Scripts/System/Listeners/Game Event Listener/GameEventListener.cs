using UnityEngine;
using UnityEngine.Events;

public class GameEventListener : MonoBehaviour
{
    #region Fields
    [SerializeField] private GameEvent @event;
    [SerializeField] private UnityEvent handle;
    #endregion Fields

    #region Methods
    public void InvokeInfo() { handle.Invoke(); }

    private void OnDisable()
    { @event.UnRegister(this); }

    private void OnEnable()
    { @event.Register(this); }
    #endregion Methods
}