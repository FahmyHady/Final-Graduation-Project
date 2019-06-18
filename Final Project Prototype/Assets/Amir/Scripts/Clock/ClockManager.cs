using System.Collections.Generic;
using UnityEngine;

public class ClockManager : MonoBehaviour
{
    #region Fields
    public ClockValues clock;
    private float elapsedTime;
    private bool isStarted;
    [SerializeField] private GameEvent timeEnd;
    [SerializeField] GameEvent timePass;
    #endregion Fields

    #region Methods
    public string GetTime()
    { return $"{clock.hours.ToString("00")}:{clock.minute.ToString("00")}"; } 
    public void StartClock()
    {
        List<int> time = RoundManager.Instance.GetRoundTime();
        clock.minute = time[0];
        clock.hours = time[1];
        isStarted = true;
        timePass.Raise();
    }

    public void StopClock()
    { isStarted = false; }

   

    private void Start()
    {
       
        elapsedTime = 0;
        isStarted = false;
    }

    // Update is called once per frame
    private void Update()
    {
        if (isStarted)
        {
            if (!((clock.minute == 0) && (clock.hours == 0)))
            {
                elapsedTime += Time.deltaTime;
                if (elapsedTime >= 1f)
                {
                    clock.minute--;
                    if (clock.minute <= 0)
                    {
                        clock.minute = 59;
                        clock.hours--;
                        if (clock.hours < 0)
                        {
                            clock.hours = 0;
                            clock.minute = 0;
                        }
                    }
                    elapsedTime = 0.0f;
                    timePass.Raise();
                }
            }
            else
            {
                //end of roud event raised
                timeEnd.Raise();
                isStarted = false;
            }
        }
    }
    public int RemainingTime { get => (clock.hours * 60) + clock.minute; }
    #endregion Methods
}