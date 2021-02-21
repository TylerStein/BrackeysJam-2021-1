using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class SpawnTrigger : MonoBehaviour
{
    public List<EnemyController> enemySpawns;
    public LayerMask spawnLayerMask = new LayerMask();
    public bool didSpawn = false;

    private List<Vector2> spawnPositions = new List<Vector2>();
    private List<int> spawnDirections = new List<int>();
    private void Start() {
        for (int i = 0; i < enemySpawns.Count; i++) {
            spawnPositions.Add(enemySpawns[i].transform.position);
            spawnDirections.Add(enemySpawns[i].moveDirection);
        }
    }

    public void ResetSpawns() {
        for (int i = 0; i < enemySpawns.Count; i++) {
            enemySpawns[i].transform.position = spawnPositions[i];
            enemySpawns[i].moveDirection = spawnDirections[i];
            enemySpawns[i].gameObject.SetActive(false);
        }
    }

    public void OnTriggerStay2D(Collider2D collision) {
        if (TestLayer(collision.gameObject.layer)) {
            for (int i = 0; i < enemySpawns.Count; i++) {
                enemySpawns[i].gameObject.SetActive(true);
            }
        }
    }

    private bool TestLayer(int otherLayer) {
        return spawnLayerMask == (spawnLayerMask.value | (1 << otherLayer));
    }
}
