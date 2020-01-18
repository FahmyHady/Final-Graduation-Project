using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using System.Linq;

public class RoleAssignHandler : MonoBehaviour, IRandomize
{
    #region Fields
    [SerializeField] private bool hasManyUses;
    [SerializeField] private List<PlayerStateInfo> infos;
    [SerializeField] private List<PlayerInfo> players;
    #endregion Fields

    #region Properties
    public bool HasManyUses { get => hasManyUses; set => hasManyUses = value; }
    public List<PlayerStateInfo> Infos { get => infos; set => infos = value; }
    public List<PlayerInfo> Players { get => players; set => players = value; }
    #endregion Properties

    #region Methods



    public void Assign(ref GameObject @object)
    {
        int numberOfReadyPlayers = PlayerPrefs.GetInt("NumberOfReadyPlayers");

        for (int i = 0; i < numberOfReadyPlayers; i++)
        {
            int numb = PlayerPrefs.GetInt("pickedChar/SpriteIndex" + i);
            Infos[i].Player = Players[numb];
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
