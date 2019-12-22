using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class TimeHoldinghandler : MonoBehaviour
{
    #region Fields
    internal System.Action timeEnd;
    private float duration;
    private float elapsedTime;
    private List<PlayerStateInfo> infos = new List<PlayerStateInfo>();
    private bool isStarted;
    [SerializeField] private Image timeRing;
    #endregion Fields

    #region Properties
    public bool IsStarted { get => isStarted; private set => isStarted = value; }
    #endregion Properties

    #region Methods

    public void StartTime(float _duration, PlayerStateInfo info)
    {
        if (_duration < 0.0f || info == null) return;
        if (!infos.Contains(info)) infos.Add(info);
        duration = _duration;
        Run();
    }

    public void StopTime(PlayerStateInfo info)
    {
        if (infos.Contains(info)) infos.Remove(info);
        if (infos.Count == 0)
        {
            IsStarted = false;
            HandleRingUI(0.0f);
            elapsedTime = 0.0f;
        }
    }

    private void HandleRingUI(float amount)
    { timeRing.fillAmount = amount; }

    private void Run()
    {
        //timeRing.color = Color.green;
        IsStarted = true;
    }

    private void StopTime()
    {
        IsStarted = false;
        HandleRingUI(0.0f);
        elapsedTime = 0.0f;
        infos.Clear();
    }

    private void Update()
    {
        if (IsStarted)
        {
            HandleRingUI(elapsedTime / duration);
            elapsedTime += infos.Sum(i => i.FixRate) * Time.deltaTime;
            if (elapsedTime >= duration)
            {
                timeEnd?.Invoke();
                StopTime();
              //  Debug.Log("Done");
            }
        }
    }

    #endregion Methods
}