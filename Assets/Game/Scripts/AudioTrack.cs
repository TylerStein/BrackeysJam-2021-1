using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioTrack : MonoBehaviour
{
    public AudioSource source;
    public int fadeDirection = 0;
    public float fadeRate = 1f;

    public void Awake() {
        //
    }

    public void Update() {
        if (fadeDirection != 0) {
            if (fadeDirection > 0 && source.volume >= 1f) fadeDirection = 0;
            else if (fadeDirection < 0 && source.volume <= 0f) fadeDirection = 0;
            float addVolume = (float)fadeDirection * fadeRate * Time.deltaTime;
            float newVolume = Mathf.Clamp(source.volume + addVolume, 0f, 1f);
            source.volume = newVolume;
        }
    }

    public void SetVolume(float volume = 1f) {
        source.volume = volume;
    }

    public void PlayImmediate() {
        fadeDirection = 0;
        source.volume = 1f;
        source.Play();
    }
    public void StopImmediate() {
        fadeDirection = 0;
        source.volume = 0f;
        source.Stop();
    }

    public void FadeIn(float rate = 1f) {
        if (!source.isPlaying) source.Play();
        fadeDirection = 1;
        fadeRate = rate;
    }

    public void FadeOut(float rate = 1f) {
        fadeDirection = -1;
        fadeRate = rate;
    }
}
