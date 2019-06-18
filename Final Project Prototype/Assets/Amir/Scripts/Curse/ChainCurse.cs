using UnityEngine;

public class ChainCurse : MonoBehaviour, ICursed
{
    #region Fields
    [SerializeField] private GameEvent @event;
    [SerializeField] private float childDelayTimeForDisable;
    [Tooltip("if true, only one at time that can be active")]
    [ReadOnlyWhenPlaying]
    [SerializeField] private bool isOnceOnly;
    private bool isStartCurse;
    [SerializeField] private int weight;
    #endregion Fields

    #region Properties
    public bool IsOnceOnly { get => isOnceOnly; set => isOnceOnly = value; }
    public bool IsStartCurse { get => isStartCurse; set => isStartCurse = value; }
    public int Weight { get => weight; set => weight = value; }
    #endregion Properties

    #region Methods
    public void EndCurse()
    {
        isStartCurse = false;
        @event.Raise();
    }

    public void StartCurse()
    {
        SetChildDelayTimeForDisable();
        RoundManager.Instance.GetParent().GetComponentInChildren<ChainCurseParentHandler>().Curse();
        RoundManager.Instance.GetRandomChild().GetComponentInChildren<ChainCurseChildHandler>().Curse();
        isStartCurse = true;
    }

    private void SetChildDelayTimeForDisable()
    {
        // Zeft
        foreach (var i in RoundManager.Instance.GetChildren())
        {
            i.GetComponentInChildren<ChainCurseChildHandler>().Delay = childDelayTimeForDisable;//Zeft *Zeft
        }
    }
    #endregion Methods
}