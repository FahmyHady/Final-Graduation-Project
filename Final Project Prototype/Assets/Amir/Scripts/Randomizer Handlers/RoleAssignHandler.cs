using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using System.Linq;

public class RoleAssignHandler : MonoBehaviour
{
    #region Fields
    [SerializeField] private bool hasManyUses;
    public List<PlayerStateInfo> infos;
    public List<PlayerInfo> players;
    #endregion Fields

    #region Properties
    public bool HasManyUses { get => hasManyUses; set => hasManyUses = value; }
    public List<PlayerStateInfo> Infos { get => infos; set => infos = value; }
    public List<PlayerInfo> Players { get => players; set => players = value; }
    #endregion Properties

    #region Methods



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
            Destroy(playersToRemove[i].gameObject);
        }

    }


    #endregion Methods
}
