using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using System.Linq;

public class RoleAssignHandler : MonoBehaviour
{
    #region Fields
    public static RoleAssignHandler Instance;
    [SerializeField] private bool hasManyUses;
    public List<PlayerStateInfo> infos;
    public List<PlayerInfo> players;
    public GameObject aphroditeItems;
    public GameObject aresItems;
    public GameObject zeusItems;
    #endregion Fields

    #region Properties
    public bool HasManyUses { get => hasManyUses; set => hasManyUses = value; }
    public List<PlayerStateInfo> Infos { get => infos; set => infos = value; }
    public List<PlayerInfo> Players { get => players; set => players = value; }
    #endregion Properties

    #region Methods

    private void Awake()
    {
        Instance = this;
    }

    public void Assign()
    {
        //   int numberOfReadyPlayers = PlayerPrefs.GetInt("NumberOfReadyPlayers");

        for (int i = 0; i < GameManager.Instance.controllerKeyandSpriteKey.Count; i++)
        {
            int numb = PlayerPrefs.GetInt("pickedChar/SpriteIndex" + i);
            Infos[GameManager.Instance.controllerKeyandSpriteKey.Values.ElementAt(i)].Player = Players[GameManager.Instance.controllerKeyandSpriteKey.Keys.ElementAt(i)];
        }
        List<PlayerStateInfo> playersToRemove = infos.Where(i => i.Player == null).ToList();
        for (int i = 0; i < playersToRemove.Count; i++)
        {
            infos.Remove(playersToRemove[i]);
            DestroyItems(playersToRemove[i].gameObject.name);
            Destroy(playersToRemove[i].gameObject);
        }
        GameplayLevelManager.instance.FindItemsNumbers();
    }
    void DestroyItems(string whoseItemsToDestroy)
    {
        switch (whoseItemsToDestroy)
        {
            case "Ares":
                Destroy(aresItems);
                break;
            case "Aphrodite":
                Destroy(aphroditeItems);
                break;
            case "Zeus":
                Destroy(zeusItems);
                break;
        }
    }

    #endregion Methods
}
