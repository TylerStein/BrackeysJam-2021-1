using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider2D))]
public class ObstacleButtonController : MonoBehaviour
{
    public Animator AnimationPlayer;
    public AnimationClip ForwardClip;
    public AnimationClip ReverseClip;

    public bool isHeld = false;
    public bool isObstacleOpen = false;

    public LayerMask TriggerLayerMask;


    //public void PlayForward() {
    //    if (AnimationPlayer.isPlaying == false) {

    //        AnimationPlayer.PlayQueued(ForwardClip.name);
    //    }
    //}

    //public void PlayReverse() {
    //    if (AnimationPlayer.isPlaying == false) {
    //        AnimationPlayer.PlayQueued(ReverseClip.name);
    //    }
    //}

    public void Update() {
        //if (!AnimationPlayer) {
            if (isHeld && !isObstacleOpen) {
                AnimationPlayer.SetBool("active", true);
                isObstacleOpen = true;
            } else if (!isHeld && isObstacleOpen) {
                AnimationPlayer.SetBool("active", false);
                isObstacleOpen = false;
            }
        //}
    }

    public void OnTriggerEnter2D(Collider2D collision) {
        if (TestLayer(collision.gameObject.layer)) {
            isHeld = true;
        }
    }
    public void OnTriggerExit2D(Collider2D collision) {
        if (TestLayer(collision.gameObject.layer)) {
            isHeld = false;
        }
    }

    private bool TestLayer(int otherLayer) {
        return TriggerLayerMask == (TriggerLayerMask.value | (1 << otherLayer));
    }
}
