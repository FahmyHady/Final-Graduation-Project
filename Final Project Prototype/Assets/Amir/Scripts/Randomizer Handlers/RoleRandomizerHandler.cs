using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class RoleRandomizerHandler : MonoBehaviour, IRandomize
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

   

    public void Ranomize(ref GameObject @object)
    {
        if (Infos.Count == Players.Count)
        {
            int rand = Random.Range(1, 4);
            for (int i = 0; i < rand; i++) { Shuffle(); }
            for (int i = 0; i < Infos.Count; i++) { Infos[i].Player = Players[i]; }
        }
    }

    public void Shuffle()
    {
        RNGCryptoServiceProvider provider = new RNGCryptoServiceProvider();
        int n = Players.Count;
        while (n > 1)
        {
            byte[] box = new byte[1];
            do provider.GetBytes(box);
            while (!(box[0] < n * (System.Byte.MaxValue / n)));
            int k = (box[0] % n);
            n--;
            var value = Players[k];
            Players[k] = Players[n];
            Players[n] = value;
        }
    }
    #endregion Methods
}