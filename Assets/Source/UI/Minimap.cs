using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
        private Vector2 backgroundSize;
        private void Start()
        {
            backgroundSize = new Vector2(GetComponent<Image>().sprite.texture.width, GetComponent<Image>().sprite.texture.height);
        }

        // Update is called once per frame
        void Update()
        {
            int i = 0;
            foreach (MinimapIcon icon in MinimapIcon.minimapIcons.OrderBy(i => i.layer))
            {
                if (!worldspaceBounds.Contains(icon.transform.position))
                    continue;

                while (icons.Count <= i)
                {
                    GameObject iconObject = new GameObject("MinimapIcon", typeof(RectTransform), typeof(CanvasRenderer), typeof(Image));
                    iconObject.transform.SetParent(transform);
                    icons.Add(iconObject.GetComponent<Image>());
                    icons[icons.Count - 1].maskable = false;
                }


                RectTransform iconTransform = (RectTransform)icons[i].transform;


                Vector3 boundsRelativePosition = (icon.transform.position - worldspaceBounds.center);
                iconTransform.anchoredPosition = new Vector2((boundsRelativePosition.x / worldspaceBounds.extents.x) * rectTransform.rect.size.x,
                                                             (boundsRelativePosition.z / worldspaceBounds.extents.z) * rectTransform.rect.size.y);
                iconTransform.sizeDelta = new Vector2(icon.iconSize, icon.iconSize);
                if (icon.scaleWithMap)
                {
                    iconTransform.sizeDelta *= rectTransform.rect.size / backgroundSize;
                }
                iconTransform.localScale = Vector3.one;
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