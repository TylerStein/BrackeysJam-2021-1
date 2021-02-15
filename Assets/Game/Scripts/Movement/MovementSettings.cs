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

    [Header("General")]

    [Tooltip("Clamp all velocity to this magnitude")]
    [SerializeField] public float maxXVelocity = 3.5f;
    [SerializeField] public float maxYVelocity = 3.5f;

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

    [Header("Contact Distances")]

    [Tooltip("How close should the ground be below to consider touching (meters)")]
    [SerializeField] public float minGroundDistance = 0.01f;

    [Tooltip("How close should the ceiling be above to consider touching (meters")]
    [SerializeField] public float minCeilingDistance = 0.01f;

    [Tooltip("How close should a wall be beside the player to consider touching (meters)")]
    [SerializeField] public float minWallDistance = 0.01f;


    [Header("Contact Layers")]

    [Tooltip("Define Ground layer mask for contacts to count as IsGrounded")]
    [SerializeField] public LayerMask blockingLayer;

}
