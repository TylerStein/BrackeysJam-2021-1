using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAudioController : MonoBehaviour
{
    public float stepTimeOffset = 0.25f;
    private float stepTimer = 0f;
    private bool isMoving = false;

    public AudioSource stepSource;
    public AudioSource jumpSource;
    public AudioSource landSource;

    public float stepMinPitch = 0.95f;
    public float stepMaxPitch = 1.05f;

    public void SetIsMoving(bool value) {
        isMoving = value;
        if (!isMoving) stepTimer = stepTimeOffset * 0.5f;
    }
    public void PlayStep() {
        if (stepSource) {
            stepSource.pitch = Random.Range(stepMinPitch, stepMaxPitch);
            stepSource.Play();
        }
    }

    public void PlayJump() {
        if (jumpSource) jumpSource.Play();
    }

    public void PlayLand() {
        if (landSource) landSource.Play();
    }

    public void Update() {
        if (isMoving) {
            stepTimer -= Time.deltaTime;
            if (stepTimer <= 0f) {
                stepTimer = stepTimeOffset;
                PlayStep();
            }
        }
    }
}
