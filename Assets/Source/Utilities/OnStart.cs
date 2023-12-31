using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace ElementalEngagement.Utilities
{
    public class OnStart : MonoBehaviour
    {
        public UnityEvent onStart;

        void Start()
        {
            onStart?.Invoke();
        }
    }
}