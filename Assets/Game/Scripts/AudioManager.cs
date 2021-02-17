using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public OptionsManager optionsManager;
    public AudioMixer mixer;

    public string masterVolumeParameter = "MasterVolume";

    void Awake()
    {
        if (!optionsManager) optionsManager = FindObjectOfType<OptionsManager>();
        optionsManager.changeEvent.AddListener(OnChangeOptions);
    }

    // Update is called once per frame
    void OnChangeOptions(GameOptions options)
    {
        float dbVolume = (options.volume * 80f) - 80f;
        mixer.SetFloat(masterVolumeParameter, dbVolume);
    }
}
