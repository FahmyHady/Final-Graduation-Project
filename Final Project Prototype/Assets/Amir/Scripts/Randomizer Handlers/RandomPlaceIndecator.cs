using UnityEngine;

public class RandomPlaceIndecator : MonoBehaviour
{
    #region Fields
    [SerializeField] private GameEvent @event;
    private bool isFree = true;
    #endregion Fields

    #region Properties
    public bool IsFree { get => isFree; set => isFree = value; }
    public GameEvent REvent { get => @event; set => @event = value; }
    #endregion Properties
}