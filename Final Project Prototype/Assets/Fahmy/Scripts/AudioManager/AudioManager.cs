using UnityEngine;

public class AudioManager : MonoBehaviour
{
    #region Fields
    public SoundSources Aphrodite;
    public SoundSources Aris;
    public SoundSources BackgroundMusic;
    public SoundSources Event;
    public SoundSources Hera;
    public SoundSources Interactable;
    public SoundSources Zeus;
    public SoundSources MainMenu;
    public SoundSources Skill;
    public SoundSources Skill2;

    public AudioSource S_Aphrodite;
    public AudioSource S_Aris;
    public AudioSource S_BackgroundMusic;
    public AudioSource S_Event;
    public AudioSource S_Skill2;
    //----------------------------------------------
    public AudioSource S_Hera;
    public AudioSource S_Interactable;
    public AudioSource S_MainMenu;
    public AudioSource S_Skill;
    public AudioSource S_Zeus;
    //----------------------------------------------
    private static AudioManager StaticAudioManager;
    //----------------------------------------------
    private AudioClip tempClip;
    #endregion Fields

    #region Enums
    public enum AudioItems
    { Hera, Zeus, Aris, Aphrodite, MainMenu, Interactable, Skill, Skill2, Event, BackgroundMusic }
    #endregion Enums

    #region Methods
    public static void Play(AudioItems whoseAudio, string soundName)
    {
        StaticAudioManager.PlaySound(whoseAudio, soundName);
    }
    void PlaySound(AudioItems whoseAudio, string soundName)
    {
        switch (whoseAudio)
        {
            case AudioItems.Hera:
                CheckSound(Hera, S_Hera, soundName);

                break;

            case AudioItems.Zeus:
                CheckSound(Zeus, S_Zeus, soundName);

                break;

            case AudioItems.Aris:
                CheckSound(Aris, S_Aris, soundName);

                break;

            case AudioItems.Aphrodite:
                CheckSound(Aphrodite, S_Aphrodite, soundName);

                break;

            case AudioItems.MainMenu:
                CheckSound(MainMenu, S_MainMenu, soundName);

                break;

            case AudioItems.Interactable:
                CheckSound(Interactable, S_Interactable, soundName);

                break;

            case AudioItems.Skill:
                CheckSound(Skill, S_Skill, soundName);

                break;

            case AudioItems.Event:
                CheckSound(Event, S_Event, soundName);
                break;

            case AudioItems.BackgroundMusic:
                CheckSound(BackgroundMusic, S_BackgroundMusic, soundName);
                break;

            case AudioItems.Skill2:
                CheckSound(Skill2, S_Skill2, soundName);
                break;
        }

    }

    private void CheckSound(SoundSources who, AudioSource audioSource, string soundName)
    {
        if (who._Audios.TryGetValue(soundName, out tempClip))
        {
            audioSource.clip = tempClip;
            audioSource.Play();
        }
    }

    private void Awake()
    {
        if (!StaticAudioManager) { StaticAudioManager = this; }
    }

    #endregion Methods
}