using UnityEngine;

public class EarthquakeCurse : MonoBehaviour, ICursed
{
    #region Fields
    [SerializeField] private GameEvent @eventEnd;
    [SerializeField] private GameEvent @eventStart;
    [SerializeField] private float curseTime;
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
    public void StartCurse()
    {
        @eventStart.Raise();
        RoundManager.Instance.GetPlayers().ForEach(i => i.GetComponentInChildren<EarthquakeHandler>().Curse());
        Invoke(nameof(EndCurse), curseTime);
        IsStartCurse = true;
    }

    private void EndCurse()
    {
        RoundManager.Instance.GetPlayers().ForEach(i => i.GetComponentInChildren<EarthquakeHandler>().StopCurse());
        IsStartCurse = false;
        @eventEnd.Raise();
    }
    #endregion Methods
}