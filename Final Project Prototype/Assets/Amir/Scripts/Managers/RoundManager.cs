using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RoundManager : MonoBehaviour
{
    #region Fields
    private static RoundManager instance;
    [SerializeField] private List<PlayerStateInfo> children;
    [SerializeField] private PlayerStateInfo parent;
    [SerializeField] private ClockManager clock;
    [SerializeField] private RandomPlaceManager placeManager;
    [SerializeField] Canvas mainCanvas;
    int interactablesNum = 0;
    #endregion Fields

    #region Properties
    public static RoundManager Instance { get => instance; private set => instance = value; }
    public Canvas MainCanvas { get => mainCanvas; }

    #endregion Properties

    #region Methods
    public void SetInteractable() {
        interactablesNum++;
    }
    public List<PlayerStateInfo> GetChildren()
    { return children; }

    public PlayerStateInfo GetParent()
    { return parent; }

    public List<int> GetRoundTime()
    {
        return GameManager.Instance.GetRoundTime();
    }

    public List<PlayerStateInfo> GetPlayers()
    { return new List<PlayerStateInfo>(children) { parent }; }

    public PlayerStateInfo GetRandomChild()
    { return children[Random.Range(0, children.Count)]; }

    private void Awake()
    { if (Instance == null) Instance = this; }

    public void TimeEnd() {
        RoundEnd(false);
    }
    public void FixItem() {
        interactablesNum--;
        if (interactablesNum == 0) {
            clock.StartClock();
            RoundEnd(true);
        }
    }
    private void RoundEnd(bool isDone)
    {
        GameManager.Instance.TotalScore += parent.Score + children.Sum(i => i.Score) + (clock.RemainingTime / 6);
        GameManager.Instance.RoundEnd(isDone);
    }

    private void Start()
    {
        placeManager.StartRandom();
        List<PlayerInfo> players = GameManager.Instance.Players;
        clock.StartClock();
        parent.Player = players[0];
        parent.CurrentStamina = parent.MaxStamina;
        for (int i = 0; i < children.Count; i++)
        {
            children[i].Player = players[i + 1];
            children[i].CurrentStamina = children[i].MaxStamina;
        }
    }

    private void Update()
    {
    }

    #endregion Methods
}