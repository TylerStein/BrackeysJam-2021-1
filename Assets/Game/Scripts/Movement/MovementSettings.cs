using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Movement Settings", menuName = "MovementSettings", order = 0)]
public class MovementSettings : ScriptableObject
{
    [Header("Abilities")]

    [Tooltip("Allow the player to jump")]
    [SerializeField] public bool canJump = true;

    [Tooltip("Allow the player to apply forces while in the air")]
    [SerializeField] public bool canMoveInAir = true;

    [Tooltip("Stick to moving ground transforms")]
    [SerializeField] public bool stickToGround = true;

    [Tooltip("Provide a vertical jump grace grap")]
    [SerializeField] public bool useJumpCushion = true;

    [Tooltip("Provide an off the ground jump grace period")]
    [SerializeField] public bool useJumpGracePeriod = true;

    [Tooltip("Provide extra jump force when holding the jump button")]
    [SerializeField] public bool useJumpBoostPeriod = true;

    [Header("General")]

    [Tooltip("Clamp all velocity to this magnitude")]
    [SerializeField] public float maxXVelocity = 7f;
    [SerializeField] public float maxYVelocity = 10f;
    [SerializeField] public float maxYDownVelocity = 25f;

    [Header("Ground Movement")]

    [Tooltip("Movement Velocity on Ground (meters/s)")]
    [SerializeField] public float groundMoveVelocity = 10.0f;

    [Tooltip("Dampen ramp to reach ground move speed")]
    [SerializeField] public float groundMoveSmoothing = 0.05f;

    [Tooltip("Dampen ramp to stopping horizontally on the ground")]
    [SerializeField] public float groundStopSmoothing = 0.1f;


    [Header("Air Movement")]

    [Tooltip("Jump Force")]
    [SerializeField] public float jumpForce = 600.0f;

    [Tooltip("Dampen ramp to reach air move speed")]
    [SerializeField] public float airMoveSmoothing = 0.05f;

    [Tooltip("Allow dampening of horizontal air velocity")]
    [SerializeField] public bool dampenAirMovement = false;

    [Tooltip("Dampen ramp to stopping horizontally in the air")]
    [SerializeField] public float airStopSmoothing = 0.07f;

    [Tooltip("Movement Velocity in Air (meters/s)")]
    [SerializeField] public float airMoveVelocity = 2.0f;

    [Header("Accessibility")]

    [Tooltip("How close should the ground be below to allow jumping if cushion is enabled")]
    [SerializeField] public float jumpCushionDistance = 0.25f;

    [Tooltip("How much time after losing ground contact should a jump be possible for")]
    [SerializeField] public float jumpGracePeriod = 0.15f;

    [Tooltip("How long should extra jump force be applied while holding the jump button")]
    [SerializeField] public float jumpBoostPeriod = 0.15f;

    [Tooltip("The max velocity to uphold while in boost mode")]
    [SerializeField] public float jumpBoostMaxVelocity = 20f;

    [Header("Contact Layers")]

    [Tooltip("Define Ground layer mask for contacts to count as IsGrounded")]
    [SerializeField] public LayerMask blockingLayer;

}
