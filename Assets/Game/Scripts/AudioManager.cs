using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public OptionsManager optionsManager;
    public AudioMixer mixer;
    public string musicVolumeParameter = "MusicVolume";
    public string sfxVolumeParameter = "SFXVolume";

    public int startTrackIndex = 0;

    public List<AudioTrack> audioTracks;

    void Awake()
    {
        if (!optionsManager) optionsManager = FindObjectOfType<OptionsManager>();
        optionsManager.changeEvent.AddListener(OnChangeOptions);

        if (startTrackIndex >= 0 && startTrackIndex < audioTracks.Count) {
            PlayTrackImmediate(startTrackIndex);
        }
    }

    // force track to play
    public void PlayTrackImmediate(int index) {
        for (int i = 0; i < audioTracks.Count; i++) {
            if (i == index) {
                audioTracks[i].PlayImmediate();
            } else {
                audioTracks[i].StopImmediate();
            }
        }
    }

    // Update is called once per frame
    void OnChangeOptions(GameOptions options)
    {
        float musicVolume = (options.music_volume * 80f) - 80f;
        mixer.SetFloat(musicVolumeParameter, musicVolume);

        float sfxVolume = (options.sxf_volume * 80f) - 80f;
        mixer.SetFloat(sfxVolumeParameter, sfxVolume);
    }

    public void FadeToSoloTrack(int index) {
        if (index < 0 || index > audioTracks.Count) throw new UnityException("Invalid Track FadeIn Index");
        for (int i = 0; i < audioTracks.Count; i++) {
            if (i == index) audioTracks[i].FadeIn();
            else audioTracks[i].FadeOut();
        }
    }

    public void FadeInTrack(int index, bool solo = false, float rate = 1f) {
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

    public void FadeOutTrack(int index, float rate = 1f) {
        if (index < 0 || index > audioTracks.Count) throw new UnityException("Invalid Track FadeOut Index");
        audioTracks[index].FadeOut(rate);
    }
}
