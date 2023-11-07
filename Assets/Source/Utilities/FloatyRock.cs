using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatyRock : MonoBehaviour
{
    [SerializeField][Min(0)] float maxHeight = 15f;
    [SerializeField][Min(0)] float minHeight = 3f;
    [SerializeField][Min(0)] float maxSpeed = 2f;
    [SerializeField][Min(0)] float minSpeed = 0.3f;
    [SerializeField][Min(0)] float maxSpinRate = 45f;
    [SerializeField][Min(0)] float minSpinRate = 15f;



    float startPeriod;
    float startHeight;
    float height;
    float speed;
    float spinRate;

    private void Start()
    {
        startHeight = transform.position.y;
        height = Random.Range(minHeight, maxHeight);
        startPeriod = Random.Range(0, Mathf.PI * 2);
        speed = RandomSign() * Random.Range(maxSpeed, minSpeed);
        spinRate = RandomSign() * Random.Range(maxSpinRate, minSpinRate);
    }

    private void Update()
    {
        transform.position = new Vector3(transform.position.x, startHeight + Mathf.Sin(startPeriod + Time.time * speed) * height/2, transform.position.z);
        transform.rotation *= Quaternion.Euler(0, spinRate * Time.deltaTime, 0);
    }

    private float RandomSign() => Random.value > 0.5 ? -1 : 1;
}
