using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ClockValues", menuName = "SO/Variable/Clock Values", order = 0)]
public class ClockValues : ScriptableObject
{
    public int hours;
    public int minute;
}
