using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollingBackground : MonoBehaviour
{
    // Start is called before the first frame update
    public List<Transform> objects;
    public Camera mainCamera;

    public float xScrollSpeed = 0.2f;
    public Vector3 trackScreenPoint = Vector3.zero;

    public bool autoXDistance = true;
    public float xDistance = 15f;

    public bool snapYToScreen = true;
    public float yOffset = 9f;

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

        Vector3 screenWorld = mainCamera.ScreenToWorldPoint(trackScreenPoint);
        foreach (Transform t in objects) {
            Vector3 tPosition = t.position;

            if (snapYToScreen) {
                tPosition.y = screenWorld.y + yOffset;
            }

            tPosition.x += xScrollSpeed * Time.deltaTime;
            t.position = tPosition;
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
