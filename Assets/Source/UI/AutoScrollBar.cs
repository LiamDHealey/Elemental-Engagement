using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace ElementalEngagement.UI
{
    [RequireComponent(typeof(ScrollRect))]
    public class AutoScrollBar : MonoBehaviour
    {
        public float speed;
        public float padding;

        Scrollbar scrollbar;
        RectTransform rectTransform => (RectTransform)transform;
        RectTransform selectedTransform => (RectTransform)EventSystem.current.currentSelectedGameObject.transform;

        private void Start()
        {
            scrollbar = GetComponent<ScrollRect>().verticalScrollbar;
        }

        // Update is called once per frame
        private void Update()
        {
            if (!UIManager.usingNavigation)
                return;

            if (!selectedTransform.IsChildOf(rectTransform))
                return;

            Rect viewportBounds = rectTransform.rect;
            Bounds selectedBounds = RectTransformUtility.CalculateRelativeRectTransformBounds(rectTransform.parent, selectedTransform);

            if (viewportBounds.min.y > selectedBounds.min.y - padding)
            {
                scrollbar.value = Mathf.Clamp01(scrollbar.value - speed * Time.deltaTime);
            }


            if (viewportBounds.max.y < selectedBounds.max.y + padding)
            {
                scrollbar.value = Mathf.Clamp01(scrollbar.value + speed * Time.deltaTime);
            }
        }
    }

}