using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAnimator : MonoBehaviour
{
    public Animator animator;

    public string keyWalking= "walking";
    public bool useWalking = true;

    public string keyJump = "jump";
    public bool useJump = false;

    public string keyGrounded = "grounded";
    public bool useGrounded = false;

    public string keyBlocked = "blocked";
    public bool useBlocked = false;

    public string keyRising = "rising";
    public bool useRising = false;

    public string keyDead = "dead";
    public bool useDead = false;
    public void SetWalking(bool walking) {
        if (useWalking) animator.SetBool(keyWalking, walking);
    }

    public void TriggerJump() {
        if (useJump) animator.SetTrigger(keyJump);
    }

    public void SetGrounded(bool grounded) {
        if (useGrounded) animator.SetBool(keyGrounded, grounded);
    }

    public void SetBlocked(bool blocked) {
        if (useBlocked) animator.SetBool(keyBlocked, blocked);
    }

    public void SetRising(bool rising) {
        if (useRising) animator.SetBool(keyRising, rising);
    }

    public void SetDead(bool dead) {
        if (useDead) animator.SetBool(keyDead, dead);
    }
}
