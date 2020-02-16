using GamepadInput;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerInfo", menuName = "SO/Variable/PlayerInfo", order = 0)]
public class PlayerInfo : ScriptableObject
{
    #region Fields
    [SerializeField] private GamePad.Index controller;
    [SerializeField] private Color outline;
    public int myNumber;
    #endregion Fields

    #region Properties
    public GamePad.Index Controller { get => controller; set => controller = value; }
    public Color Outline { get => outline; set => outline = value; }
    #endregion Properties
}