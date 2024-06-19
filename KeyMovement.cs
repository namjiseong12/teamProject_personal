using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyMovement : MonoBehaviour
{
    public float rotationSpeed = 50.0f; // 회전 속도
    public float floatSpeed = 0.5f; // 부유 속도
    public float floatHeight = 0.5f; // 부유 높이

    private Vector3 originalPosition; // 원래 위치 저장

    void Start()
    {
        originalPosition = transform.position;
    }

    void Update()
    {
        // 오브젝트 회전
        transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);

        // 위아래 부유 움직임
        float newY = Mathf.Sin(Time.time * floatSpeed) * floatHeight + originalPosition.y;
        transform.position = new Vector3(originalPosition.x, newY, originalPosition.z);
    }
}
