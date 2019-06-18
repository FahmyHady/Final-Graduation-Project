using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class GameManager : MonoBehaviour
{
    #region Fields
    private static GameManager instance;
    [Range(5, 15)]
    [ReadOnlyWhenPlaying]
    [SerializeField] private int maxLeaderBoardTeams = 5;
    [SerializeField] private int maxRounds;
    [SerializeField] private int maxInteractableFixed;
    int playedRound;
    int[] rounds;
    List<PlayerInfo> players;
    private SaveLoadScoreHandler teams;
    private float totalScore;
    [Header("Round Time")]
    [SerializeField] int minTime;
    [SerializeField] int hTime;
    #endregion Fields

    #region Properties
    public static GameManager Instance { get => instance; set => instance = value; }
    public int MaxLeaderBoardTeams { get => maxLeaderBoardTeams; }
    public float TotalScore { get => totalScore; set => totalScore = value; }
    public List<PlayerInfo> Players { get => players; set => players = value; }
    public int[] Rounds { get => rounds; set => rounds = value; }
    public SaveLoadScoreHandler Teams { get => teams; set => teams = value; }
    public int MaxInteractableFixed { get => maxInteractableFixed; set => maxInteractableFixed = value; }
    #endregion Properties

    #region Methods
    private void Awake()
    {
     
            if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
            Destroy(this.gameObject);
    }
    void FinishGame() {
        SceneManager.LoadSceneAsync(3);
    }
    public void EndGame(string teamName)
    {
        ScoreTeam team = new ScoreTeam();
        team.name = (teamName == string.Empty) ? "Someone Play this Game" : teamName;
        team.score = TotalScore + maxRounds;
        Teams.Add(team);
        SaveAndLoadManager.Save(Teams);
    }
    public void StartRound() {
        SceneManager.LoadSceneAsync(2);
    }

    public void NewGame() {
        totalScore = 0.0f;
        playedRound = 0;
        Rounds = new int[maxRounds];
        SceneManager.LoadSceneAsync(1);
    }
    public void RoundEnd(bool isDone) {
        Rounds[playedRound] = isDone ? 1 : 2;
        playedRound++;
        if (playedRound == maxRounds)
            FinishGame();
        else
            SceneManager.LoadSceneAsync(1);
    }
    // Start is called before the first frame update
    private void Start()
    { Teams = SaveAndLoadManager.Load(); }

    public List<int> GetRoundTime() {
        List<int> vs = new List<int>();
        vs.Add(minTime);
        vs.Add(hTime);
        return vs;
    }
  
    #endregion Methods
 
     
}