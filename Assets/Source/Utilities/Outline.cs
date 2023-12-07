using ElementalEngagement.Player;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

namespace ElementalEngagement.Utilities
{
    /// <summary>
    /// Allows control over the outline of a sprite
    /// </summary>
    [RequireComponent(typeof(SpriteRenderer))]
    public class Outline : MonoBehaviour
    {
        private void Awake()
        {
            var allegiance = GetComponentInParent<Allegiance>();
            allegiance?.onFactionChanged
                .First(fe => fe.faction == Faction.PlayerOne)
                .onSelected.AddListener(() => SetColor(Color.white));
            allegiance?.onFactionChanged
                .First(fe => fe.faction == Faction.PlayerTwo)
                .onSelected.AddListener(() => SetColor(Color.black));
            allegiance?.onFactionChanged
                .First(fe => fe.faction == Faction.Unaligned)
                .onSelected.AddListener(() => SetColor(Color.clear));
        }

        public void SetColor(string color)
        {
            string[] rgba = color.Split(", ");

            GetComponent<SpriteRenderer>().material?.SetColor("_OutlineColor", new Color(float.Parse(rgba[0]), float.Parse(rgba[1]), float.Parse(rgba[2]), float.Parse(rgba[3])));
        }
        public void SetColor(Color color)
        {
            GetComponent<SpriteRenderer>().material.SetColor("_OutlineColor", color);
        }
    }

}