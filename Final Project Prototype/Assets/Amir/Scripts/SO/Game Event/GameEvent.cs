using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GameEvent", menuName = "SO/Events/GameEvent", order = 0)]
public class GameEvent : ScriptableObject
{
    #region Fields
    private List<GameEventListener> listeners = new List<GameEventListener>();
    #endregion Fields

    #region Methods
    public void Register(GameEventListener listener)
    { if (!listeners.Contains(listener)) listeners.Add(listener); }

    // void fun(void){}
    public void Raise()
    {
        for (int i = 0; i < listeners.Count; i++)
        { listeners[i].InvokeInfo(); }
    }

    public void UnRegister(GameEventListener listener)
    { if (listeners.Contains(listener)) listeners.Remove(listener); }    
    #endregion Methods
}