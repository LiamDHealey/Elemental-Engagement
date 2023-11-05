using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace ElementalEngagement.Utilities
{
    public class OnUpdate : MonoBehaviour
    {
        public UnityEvent onUpdate;
        // Update is called once per frame
        void Update()
        {
            onUpdate?.Invoke();
        }
    }
}