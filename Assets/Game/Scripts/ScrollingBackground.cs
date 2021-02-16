using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollingBackground : MonoBehaviour
{
    // Start is called before the first frame update
    public List<Transform> objects;
    public Camera mainCamera;

    public Vector2 scrollSpeed = new Vector2(1f, 0f);

    public bool autoXDistance = true;
    public float xDistance = 15f;

    public GameManager gameManager;

    void Start()
    {
        if (!gameManager) gameManager = FindObjectOfType<GameManager>();

        if (objects.Count < 3) throw new UnityException("ScrollingBackground requires at least 3 objects");
        if (autoXDistance) {
            xDistance = Mathf.Abs(objects[0].position.x - objects[1].position.x);
        }
        if (!mainCamera) mainCamera = Camera.main;

        objects.Sort((t1, t2) => t1.position.x >= t2.position.x ? 1 : -1);
    }

    // Update is called once per frame
    void Update()
    {
        if (gameManager.IsPaused) return;

        foreach (Transform t in objects) {
            t.Translate(scrollSpeed * Time.deltaTime);
        }

        float leftX = mainCamera.transform.position.x - xDistance * 1.5f;
        float rightX = mainCamera.transform.position.x + xDistance * 1.5f;

        if (objects[0].position.x < leftX) {
            // move to right side
            objects[0].position = objects[objects.Count - 1].position + new Vector3(xDistance, 0f, 0f);
            Transform moveTarget = objects[0];
            objects.RemoveAt(0);
            objects.Add(moveTarget);
        }

        if (objects[objects.Count - 1].position.x > rightX) {
            // move to left side
            objects[objects.Count - 1].position = objects[0].position - new Vector3(xDistance, 0f, 0f);
            Transform moveTarget = objects[objects.Count - 1];
            objects.RemoveAt(objects.Count - 1);
            objects.Insert(0, moveTarget);
        }
    }
}
