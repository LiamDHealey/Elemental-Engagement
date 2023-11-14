using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DelayStart : MonoBehaviour
{
    void Start()
    {
        StartCoroutine(ActivateRoutine());
    }

    private IEnumerator ActivateRoutine()
    {

        // make a list of all children
        Transform[] ChildrenTransforms = this.gameObject.GetComponentsInChildren<Transform>();
        foreach (Transform t in ChildrenTransforms)
        {
            if (0 == t.childCount)
                t.gameObject.SetActive(false); // disable the children
        }

        yield return new WaitForSeconds(.05f);

        foreach (Transform t in ChildrenTransforms)
        {
            t.gameObject.SetActive(true);  // restore all the disabled objects in the list (even the parent)
        }
    }
}
