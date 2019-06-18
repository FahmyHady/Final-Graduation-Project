using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class LeaderBoardHandler : MonoBehaviour
{
    [SerializeField] InputField field;
    [SerializeField] Text LeaderBoard;
    void Start()
    {
        
    }
    public void Done() {
        SetTeamName();
        LeaderBoard.text = string.Empty;
        var teams = GameManager.Instance.Teams.GetData();
        foreach (var item in teams)
        {
            LeaderBoard.text += $"{item.name} - {item.score} \n";
        }

    }
    void SetTeamName() {
        GameManager.Instance.EndGame(field.text);
    }
    public void NewGame() {
        SceneManager.LoadSceneAsync(0); 
    }
}
