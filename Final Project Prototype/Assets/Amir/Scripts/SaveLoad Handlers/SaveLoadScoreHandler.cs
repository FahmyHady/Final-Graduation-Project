using System.Collections.Generic;
using System.Linq;

[System.Serializable]
public struct ScoreTeam
{
    #region Fields
    public string name;
    public float score;
    #endregion Fields
}

[System.Serializable]
public class SaveLoadScoreHandler
{
    #region Fields
    private List<ScoreTeam> teams;
    #endregion Fields

    #region Methods
    public void Add(ScoreTeam team)
    {
        if (teams == null)
            teams = new List<ScoreTeam>();
        teams.Add(team);
        teams = teams.OrderByDescending(i => i.score).ToList();
        if (teams.Count > GameManager.Instance.MaxLeaderBoardTeams)
            teams = teams.GetRange(0, GameManager.Instance.MaxLeaderBoardTeams);
    }

    public List<ScoreTeam> GetData()
    { return teams; }

    public void SetData(List<ScoreTeam> scores)
    { teams = scores; }
    #endregion Methods
}
