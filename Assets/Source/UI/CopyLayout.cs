using ElementalEngagement.Player;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace ElementalEngagement.UI
{
    [ExecuteAlways]
    [RequireComponent(typeof(RectTransform))]
    public class CopyLayout : MonoBehaviour
    {
        [Tooltip("Maps each player to a win event")]
        public RectTransform source;

        private void Update()
        {
            RectTransform rectTransform = (RectTransform)transform;
            rectTransform.anchoredPosition = source.anchoredPosition;
            rectTransform.anchorMax = source.anchorMax;
            rectTransform.anchorMin = source.anchorMin;
            rectTransform.offsetMin = source.offsetMin;
            rectTransform.offsetMax = source.offsetMax;


            for (int i = 0; i < transform.childCount; i++)
            {
                RectTransform child = (RectTransform)transform.GetChild(i);
                RectTransform childToCopy = (RectTransform)source.GetChild(i);

                child.anchoredPosition = child.anchoredPosition;
                child.anchorMax = childToCopy.anchorMax;
                child.anchorMin = childToCopy.anchorMin;
                child.offsetMin = childToCopy.offsetMin;
                child.offsetMax = childToCopy.offsetMax;
            }
        }
    }
}
