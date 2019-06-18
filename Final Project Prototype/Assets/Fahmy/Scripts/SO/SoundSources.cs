using UnityEngine;

[CreateAssetMenu(fileName = "AudioSources", menuName = "SO/ObjectAudios")]
public class SoundSources : ScriptableObject
{
    #region Fields
    public S_Dic _Audios = new S_Dic();
    #endregion Fields
}