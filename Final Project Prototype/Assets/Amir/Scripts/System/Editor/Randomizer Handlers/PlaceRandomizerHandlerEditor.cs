using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(PlaceRandomizerHandler))]
public class PlaceRandomizerHandlerEditor : Editor
{
    #region Methods
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        PlaceRandomizerHandler myScript = (PlaceRandomizerHandler)target;
        if (GUILayout.Button("Build Object"))
        {
            myScript.CreatePlace();
        }
    }
    #endregion Methods
}