using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class GameOptions
{
    public float music_volume = 1f;
    public float sxf_volume = 0.5f;
}

[System.Serializable]
public class OptionsEvent : UnityEvent<GameOptions> { }

public class OptionsManager : MonoBehaviour
{
    public string optionString_sfxvolume = "opt_sfx_volume";
    public string optionString_musicVolume = "opt_music_volume";

    public GameOptions options = new GameOptions();
    public OptionsEvent changeEvent = new OptionsEvent();
    public bool loadOnStart = true;
    public void Start() {
        if (loadOnStart) Load();
    }

    public void ChangeOption_MusicVolume(float value) {
        float newValue = Mathf.Clamp(options.music_volume + value, 0f, 1f);
        if (newValue != options.music_volume) {
            options.music_volume = newValue;
            Save();
            changeEvent.Invoke(options);
        }
    }
    public void ChangeOption_SxfVolume(float value) {
        float newValue = Mathf.Clamp(options.sxf_volume + value, 0f, 1f);
        if (newValue != options.sxf_volume) {
            options.sxf_volume = newValue;
            Save();
            changeEvent.Invoke(options);
        }
    }

    public void Load() {
        options = new GameOptions() {
            music_volume = PlayerPrefs.GetFloat(optionString_musicVolume, 1.0f),
            sxf_volume = PlayerPrefs.GetFloat(optionString_sfxvolume, 1.0f),
        };
        changeEvent.Invoke(options);
    }

    public void Save() {
        PlayerPrefs.SetFloat(optionString_musicVolume, options.music_volume);
        PlayerPrefs.SetFloat(optionString_sfxvolume, options.sxf_volume);
    }
}
