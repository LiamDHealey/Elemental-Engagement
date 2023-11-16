using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ElementalEngagement.UI
{
    [RequireComponent(typeof(RectTransform))]
    public class Minimap : MonoBehaviour
    {
        public Bounds worldspaceBounds;


        private RectTransform rectTransform => (RectTransform)transform;
        private readonly List<Image> icons = new List<Image>();

        // Update is called once per frame
        void Update()
        {
            int i = 0;
            foreach (MinimapIcon icon in MinimapIcon.minimapIcons)
            {
                if (!worldspaceBounds.Contains(icon.transform.position))
                    continue;

                while (icons.Count <= i)
                {
                    GameObject iconObject = new GameObject("MinimapIcon", typeof(RectTransform), typeof(CanvasRenderer), typeof(Image));
                    iconObject.transform.SetParent(transform);
                    icons.Add(iconObject.GetComponent<Image>());
                }


                RectTransform iconTransform = (RectTransform)icons[i].transform;


                Vector3 boundsRelativePosition = (icon.transform.position - worldspaceBounds.center);
                iconTransform.anchoredPosition = new Vector2((boundsRelativePosition.x / worldspaceBounds.extents.x) * rectTransform.rect.size.x,
                                                             (boundsRelativePosition.z / worldspaceBounds.extents.z) * rectTransform.rect.size.y);
                iconTransform.sizeDelta = new Vector2(icon.iconSize, icon.iconSize);
                icons[i].sprite = icon.icon;

                i++;
            }

            while (icons.Count > i + 1)
            {
                Destroy(icons[i + 1].gameObject);
                icons.RemoveAt(i + 1);
            }
        }
    }

}