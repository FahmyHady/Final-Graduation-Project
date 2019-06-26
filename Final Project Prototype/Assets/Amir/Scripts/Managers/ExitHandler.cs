using UnityEngine;

public class ExitHandler : MonoBehaviour
{
    public void Quit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
            GameManager.Instance.Exit();
#endif
    }
}