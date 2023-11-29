using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class Clouds : MonoBehaviour
{
    [SerializeField][Min(0)] int cloudCount;
    [SerializeField][Min(0)] float maxSpeed = 2f;
    [SerializeField][Min(0)] float minSpeed = 0.3f;
    [SerializeField][Min(0)] float maxScale = 2f;
    [SerializeField][Min(0)] float minScale = 0.3f;
    [SerializeField][Range(0, 360)] float maxAngle = 15f;
    [SerializeField] List<GameObject> cloudPrefabs;


    Bounds bounds;
    List<Transform> clouds = new List<Transform>();
    List<float> cloudSpeeds = new List<float>();

    private void Start()
    {
        BoxCollider collider = GetComponent<BoxCollider>();
        bounds = new Bounds(collider.center, collider.size);

        for (int i = 0; i < cloudCount; i++)
        {
            clouds.Add(Instantiate(cloudPrefabs[Random.Range(0, cloudPrefabs.Count)], transform).transform);
            clouds[i].transform.localPosition = new Vector3(Random.Range(bounds.min.x, bounds.max.x), 
                                                       Random.Range(bounds.min.y, bounds.max.y), 
                                                       Random.Range(bounds.min.z, bounds.max.z));
            clouds[i].transform.localRotation = Quaternion.Euler(90, Random.Range(-maxAngle, maxAngle), 0);
            clouds[i].transform.localScale = Vector3.one * Random.Range(minScale, maxScale);
            cloudSpeeds.Add(Random.Range(minSpeed, maxSpeed));
        }
    }

    private void Update()
    {
        for (int i = 0; i < cloudCount; i++)
        {
            clouds[i].position += cloudSpeeds[i] * clouds[i].right * Time.deltaTime;
            if (clouds[i].localPosition.x > bounds.max.x || clouds[i].localPosition.x < bounds.min.x)
            {
                clouds[i].localPosition = Vector3.Scale(clouds[i].localPosition, new Vector3(-1, 1, 1));
            }
            if (clouds[i].localPosition.y > bounds.max.y || clouds[i].localPosition.y < bounds.min.y)
            {
                clouds[i].localPosition = Vector3.Scale(clouds[i].localPosition, new Vector3(1, -1, 1));
            }
            if (clouds[i].localPosition.z > bounds.max.z || clouds[i].localPosition.z < bounds.min.z)
            {
                clouds[i].localPosition = Vector3.Scale(clouds[i].localPosition, new Vector3(1, 1, -1));
            }
        }
    }
}
