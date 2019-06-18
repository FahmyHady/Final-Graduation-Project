using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    #region Fields
    static public float count = 0;
    #endregion Fields

    #region Methods
    private void Update()
    {
        if (count == 4) { SceneManager.LoadScene(2); }
    }
    #endregion Methods
}