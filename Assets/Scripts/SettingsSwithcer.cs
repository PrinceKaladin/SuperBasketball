using UnityEngine;
using UnityEngine.UI;

public class SettingsSwitcher : MonoBehaviour
{
    [Header("Audio")]
    public AudioSource musicSource;
    public AudioSource sfxSource;

    [Header("UI Images")]
    public Image musicImage;
    public Image sfxImage;


    [Header("Colors")]
    public Color enabledColor = new Color(1f, 1f, 1f, 1f);     // обычный
    public Color disabledColor = new Color(1f, 1f, 1f, 0.4f); // полупрозрачный

    [Header("States")]
    public bool musicOn = true;
    public bool sfxOn = true;


    void Start()
    {
        ApplyMusicState();
        ApplySfxState();
    }

    // --------------------
    // SWITCHERS
    // --------------------

    public void SwitchMusic()
    {
        musicOn = !musicOn;
        ApplyMusicState();
    }

    public void SwitchSfx()
    {
        sfxOn = !sfxOn;
        ApplySfxState();
    }

 
    // --------------------
    // APPLY STATES
    // --------------------

    void ApplyMusicState()
    {
        musicSource.mute = !musicOn;
        musicImage.color = musicOn ? enabledColor : disabledColor;
    }

    void ApplySfxState()
    {
        sfxSource.mute = !sfxOn;
        sfxImage.color = sfxOn ? enabledColor : disabledColor;
    }


}
