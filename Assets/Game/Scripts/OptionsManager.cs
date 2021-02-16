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

    public void Load() {
        options = new GameOptions() {
            volume = PlayerPrefs.GetFloat(optionString_volume, 1.0f)
        };
    }

    public void Save() {
        PlayerPrefs.SetFloat(optionString_volume, options.volume);
    }
}
