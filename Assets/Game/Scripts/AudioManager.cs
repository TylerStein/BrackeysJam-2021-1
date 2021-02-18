using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public OptionsManager optionsManager;
    public AudioMixer mixer;
    public string masterVolumeParameter = "MasterVolume";

    public List<AudioTrack> audioTracks;

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

    void FadeInTrack(int index, bool solo = false, float rate = 1f) {
        if (index < 0 || index > audioTracks.Count) throw new UnityException("Invalid Track FadeIn Index");
        if (solo) {
            for (int i = 0; i < audioTracks.Count; i++) {
                if (index == i) audioTracks[i].FadeIn(rate);
                else audioTracks[i].FadeOut(rate);
            }
        } else {
            audioTracks[index].FadeIn(rate);
        }
    }

    void FadeOutTrack(int index, float rate = 1f) {
        if (index < 0 || index > audioTracks.Count) throw new UnityException("Invalid Track FadeOut Index");
        audioTracks[index].FadeOut(rate);
    }
}
