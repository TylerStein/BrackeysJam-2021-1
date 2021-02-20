using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportFX : MonoBehaviour
{
    public SpriteRenderer sprite;
    public Animator animator;
    public string animatorState;
    public float animationTime = 0.5f;
    [SerializeField] private float _timer = 0f;

    public void Start() {
        sprite.enabled = false;
    }

    public void Update() {
        if (_timer > 0f) {
            _timer -= Time.deltaTime;
            if (_timer <= 0f) {
                _timer = 0f;
                sprite.enabled = false;
            }
        }
    }

    public void PlayAt(Vector3 position) {
        sprite.enabled = true;
        transform.position = position;
        animator.Play(animatorState);
        _timer = animationTime;
    }
}
