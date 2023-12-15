using ElementalEngagement.Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

namespace ElementalEngagement.Utilities
{
    /// <summary>
    /// Allows control over the color of a sprite via unity events
    /// </summary>
    [RequireComponent(typeof(SpriteRenderer))]
    public class SpriteColor : MonoBehaviour
    {
        public Color player1Color = Color.white;
        public Color player2Color = Color.black;
        public Color unalignedColor = Color.clear;

        public void Start()
        {
            GetComponent<SpriteRenderer>().color = GetComponentInParent<Allegiance>()?.faction switch
            {
                Faction.PlayerOne => player1Color,
                Faction.PlayerTwo => player2Color,
                _ => unalignedColor,
            };
        }
    }

}