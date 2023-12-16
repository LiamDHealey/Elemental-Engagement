using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DelayStart : MonoBehaviour
{
    [Tooltip("How Long to wait before activating this objects children")]
    public float timeToWait = 0.001f;

    Transform[] children;
    void Start()
    {
        StartCoroutine(ActivateRoutine());
    }

    private IEnumerator ActivateRoutine()
    {
        // make a list of all children
        children = this.gameObject.GetComponentsInChildren<Transform>();
        foreach (Transform t in children)
        {
            if (0 == t.childCount)
                t.gameObject.SetActive(false); // disable the children
        }

        yield return new WaitForSeconds(timeToWait);

        foreach (Transform t in children)
        {
            t.gameObject.SetActive(true);  // restore all the disabled objects in the list (even the parent)
        }
    }
}
