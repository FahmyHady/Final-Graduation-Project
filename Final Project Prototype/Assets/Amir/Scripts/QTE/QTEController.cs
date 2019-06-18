using GamepadInput;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;

public class QTEController : MonoBehaviour
{
    #region Fields
    internal System.Action patternDone;
    internal System.Action patternMatched;
    internal System.Action patternMissed;
    internal System.Action timeEnd;
    private StringBuilder builder;
    private Color defColor;
    private float duration;
    private float elapsedTime;
    private GamepadState gamepad;
    private int index;
    private bool isFalsePress;
    private bool isStarted;
    private bool isTruePress;
    [SerializeField] private GamePadKeyDictionary keys;
    private string[] keysAsString;
    private int keysSize;
    private Color nullColor;
    private string pattern;
    private int patternLength;
    private int playerID;
    [SerializeField] private Image QTEKey;
    private string str;
    [SerializeField] private Image timeRing;
    #endregion Fields

    #region Properties
    public bool IsStarted { get => isStarted; private set => isStarted = value; }
    #endregion Properties

    #region Methods
    public void StartQTE(int _patternLength, float _duration, int _ID)
    {
        if (_patternLength < 1)
            return;
        patternLength = _patternLength;
        duration = _duration;
        playerID = _ID;
        GeneratePattern();
        Run();
    }

    public void StartQTE(string _pattern, float _duration, int _ID)
    {
        if (_pattern == string.Empty)
            return;
        pattern = _pattern;
        duration = _duration;
        playerID = _ID;
        if (!ValidatePattern())
        {
            Debug.LogError("Invalid Pattern Does Not Match Keys");
            return;
        }
        Run();
    }

    public void StopQTE()
    {
        IsStarted = false;
        HandleRingUI(0.0f);
        HandelQTEKey(string.Empty);
        elapsedTime = 0.0f;
        index = 0;
    }

    private void ConvertKeysToString()
    { keysAsString = keys.Select(i => i.Key.ToString()).ToArray(); }

    private void GeneratePattern()
    {
        if (keys.Count < 1) return;
        ConvertKeysToString();
        keysSize = keysAsString.Length;
        builder.Clear();
        for (int i = 0; i < patternLength; i++)
        {
            str = keysAsString[Random.Range(0, keysSize)];
            builder.Append(str);
        }
        pattern = builder.ToString();
    }

    private void HandelQTEKey(string key)
    {
        if (key == string.Empty) { QTEKey.color = nullColor; }
        else
        {
            QTEKey.color = Color.green;
            QTEKey.sprite = keys[(GamePad.ButtonPad)System.Enum.Parse(typeof(GamePad.ButtonPad), key)];
            QTEKey.color = defColor;
        }
    }

    private void HandleRingUI(float amount)
    { timeRing.fillAmount = amount; }

    private void Run()
    {
        timeRing.color = Color.red;
        IsStarted = true;
        HandelQTEKey($"{pattern[index]}");
    }

    private void Start()
    {
        builder = new StringBuilder();
        nullColor = new Color(255, 255, 255, 0);
        defColor = new Color(1, 1, 1, 1);
        HandelQTEKey(string.Empty);
    }

    private void Update()
    {
        if (IsStarted)
        {
            HandleRingUI(elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            if (elapsedTime >= duration)
            {
                timeEnd?.Invoke();
                StopQTE();
                //Debug.Log("Done");
                return;
            }
            gamepad = GamePad.GetState((GamePad.Index)playerID);
            if (gamepad.AnyKeyPressedDownAXYB)
            {
                isTruePress = false;
                isFalsePress = false;
                if (gamepad.ADown && pattern[index] == 'A') { isTruePress = true; } else if (gamepad.ADown && pattern[index] != 'A') { isFalsePress = true; }
                if (gamepad.XDown && pattern[index] == 'X') { isTruePress = true; } else if (gamepad.XDown && pattern[index] != 'X') { isFalsePress = true; }
                if (gamepad.YDown && pattern[index] == 'Y') { isTruePress = true; } else if (gamepad.YDown && pattern[index] != 'Y') { isFalsePress = true; }
                if (gamepad.BDown && pattern[index] == 'B') { isTruePress = true; } else if (gamepad.BDown && pattern[index] != 'B') { isFalsePress = true; }
                if (isFalsePress)
                {
                    patternMissed?.Invoke();
                    StopQTE();
                    return;
                }
                else if (isTruePress)
                {
                    patternMatched?.Invoke();
                    index++;
                }
            }
            if (index >= pattern.Length)
            {
                patternDone?.Invoke();
                StopQTE();
            }
            else { HandelQTEKey($"{pattern[index]}"); }
        }
    }
    private bool ValidatePattern()
    {
        ConvertKeysToString();
        string s = pattern;
        foreach (string strKey in keysAsString) { s = Regex.Replace(s, $"[{strKey}]", string.Empty); }
        return s.Length == 0;
    }
    #endregion Methods
}