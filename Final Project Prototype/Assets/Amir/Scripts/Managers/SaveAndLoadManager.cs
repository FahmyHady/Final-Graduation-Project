using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public static class SaveAndLoadManager
{
    #region Methods
    public static SaveLoadScoreHandler Load()
    {
        SaveLoadScoreHandler handler = new SaveLoadScoreHandler();
        if (File.Exists(Application.persistentDataPath + $"/LeaderBoard.Koko"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + $"/LeaderBoard.Koko", FileMode.Open);
            file.Position = 0;
            List<ScoreTeam> teams = (List<ScoreTeam>)bf.Deserialize(file);
            handler.SetData(teams);
            file.Close();
        }
        return handler;
    }

    public static void Save(SaveLoadScoreHandler handler)
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + $"/LeaderBoard.Koko");
        bf.Serialize(file, handler.GetData());
        file.Close();
    }
    #endregion Methods
}