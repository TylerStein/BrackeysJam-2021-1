using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class GameOptions
{
    public float volume = 1f;
}

[System.Serializable]
public class OptionsEvent : UnityEvent<GameOptions> { }

public class OptionsManager : MonoBehaviour
{
    public string optionString_volume = "opt_volume";

    public GameOptions options = new GameOptions();
    public OptionsEvent changeEvent = new OptionsEvent();
    public bool loadOnStart = true;
    public void Start() {
        if (loadOnStart) Load();
    }

    public void ChangeOption_Volume(float value) {
        float newValue = Mathf.Clamp(options.volume + value, 0f, 1f);
        if (newValue != options.volume) {
            options.volume = newValue;
            Save();
            changeEvent.Invoke(options);
        }
    }

    public void Load() {
        options = new GameOptions() {
            volume = PlayerPrefs.GetFloat(optionString_volume, 1.0f)
        };
        changeEvent.Invoke(options);
    }

    public void Save() {
        PlayerPrefs.SetFloat(optionString_volume, options.volume);
    }
}
