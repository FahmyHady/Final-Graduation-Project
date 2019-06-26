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
    float bounsTime;
    #endregion Fields

    #region Properties
    public static RoundManager Instance { get => instance; private set => instance = value; }
    public Canvas MainCanvas { get => mainCanvas; }
    public float GetScore { get => parent.Score + children.Sum(i => i.Score); }
    #endregion Properties

    #region Methods
    public void SetInteractable() {
        //interactablesNum++;
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
        if (interactablesNum == 0)
            RoundEnd(true);
        else
        {
            bounsTime = 0.0f;
            RoundEnd(false);
        }
    }
    public Color GetChildOutline(Character character) {
        Color color =default;
        switch (character)
        {
            case Character.Zeus:
                color = children[2].Player.Outline;
                break;
            case Character.Aris:
                color = children[1].Player.Outline;
                break;
            case Character.Aphrodite:
                color = children[0].Player.Outline;
                break;
        }
        return color;
    }
    public void FixItem() {
        interactablesNum--;
        if (interactablesNum < 0)
        {
            interactablesNum = 0;
        }
        else if (interactablesNum == 0) {
            bounsTime = (clock.RemainingTime / 6);
        }
    }
    private void RoundEnd(bool isDone)
    {
        GameManager.Instance.TotalScore += GetScore + bounsTime;
        GameManager.Instance.RoundEnd(isDone);
    }

    private void Start()
    {
        List<PlayerInfo> players = GameManager.Instance.Players;
        parent.Player = players[0];
        parent.CurrentStamina = parent.MaxStamina;
        for (int i = 0; i < children.Count; i++)
        {
            children[i].Player = players[i + 1];
            children[i].CurrentStamina = children[i].MaxStamina;
        }
        interactablesNum = GameManager.Instance.MaxInteractableFixed;
        clock.StartClock();
        placeManager.StartRandom();
    }
    #endregion Methods
}