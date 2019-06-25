using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PreLevelManager : MonoBehaviour
{
    #region Fields
    private float elapsedTime;
    [SerializeField] private List<PortalHandler> portals;
    private int readyPlayerNum;
    [SerializeField] private RoleRandomizerHandler roleHandler;

    [Range(5, 30)]
    [SerializeField] private int startDelay;

    private static PreLevelManager manager;
    [SerializeField] private Text startTimerText;
    int value;
    #endregion Fields

    #region Properties
    public int ReadyPlayerNum { get => readyPlayerNum; set { readyPlayerNum = value; CheckReadyPlayers(); } }

    public static PreLevelManager Manager { get => manager; set => manager = value; }
    #endregion Properties

    #region Methods
    private void Awake()
    {
        if (Manager == null)
            manager = this;
    }
    private void HandlePlayersRole()
    {
        GameObject obj =null;
        roleHandler.Ranomize(ref obj);
        for (int i = 0; i < portals.Count; i++)
        {
            portals[i].SetColor(roleHandler.Infos[i].Player.Outline);
        }
    }

    private void CheckReadyPlayers()
    {
        if (startDelay > 5 && ReadyPlayerNum == portals.Count+1)
        {
            elapsedTime = 0.0f;
            startDelay = 5;
            startTimerText.text = startDelay.ToString();
        }
    }

    private void Start()
    {
        HandlePlayersRole();
        elapsedTime = startDelay;
        startTimerText.text = startDelay.ToString();
    }

    // Update is called once per frame
    private void Update()
    {
        elapsedTime += Time.deltaTime;
        if (elapsedTime >= 1.0f )
        {
            elapsedTime = 0.0f;
            startDelay--;
            startDelay = Mathf.Max(startDelay, 0);
            startTimerText.text = startDelay.ToString();
            if (startDelay == 0)
            {
                GameManager.Instance.Players = roleHandler.Players;
                GameManager.Instance.StartRound();
            }
        }
    }

    #endregion Methods
}