using ElementalEngagement.Player;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

namespace ElementalEngagement.UI
{
    public class MinimapIcon : MonoBehaviour
    {
        [SerializeField] private float _iconSize = 10f;
        public float iconSize => _iconSize * transform.localScale.x;
        public Sprite icon => GetComponent<Allegiance>()?.faction switch
        {
            Faction.Unaligned => unalignedIcon,
            Faction.PlayerOne => playerOneIcon,
            Faction.PlayerTwo => playerTwoIcon,
            _ => unalignedIcon,
        };
        public bool scaleWithMap = true;
        public int layer = 0;
        public Sprite unalignedIcon;
        public Sprite playerOneIcon;
        public Sprite playerTwoIcon;

        public static readonly HashSet<MinimapIcon> minimapIcons = new HashSet<MinimapIcon>();

        // Start is called before the first frame update
        private void Start()
        {
            minimapIcons.Add(this);
        }

        // Update is called once per frame
        private void OnDestroy()
        {
            minimapIcons.Remove(this);
        }
    }
}