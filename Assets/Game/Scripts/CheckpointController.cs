using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointController : MonoBehaviour
{
    [SerializeField] private Transform DefaultSpawn;
    [SerializeField] private Checkpoint LastCheckpoint;


    public Transform GetRespawnTarget() {
        return LastCheckpoint?.Target ?? DefaultSpawn;
    }

    public void SetCheckpoint(Checkpoint checkpoint) {
        LastCheckpoint = checkpoint;
    }
}
