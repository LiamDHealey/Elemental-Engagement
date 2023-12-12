using ElementalEngagement.Favor;
using ElementalEngagement.Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace ElementalEngagement.UI
{
    public class FavorMilestones : MonoBehaviour
    {
        public UnityEvent onFriendlyArmySpawned;
        public UnityEvent onHostileArmySpawned;
        public UnityEvent onMaxFireFavor;
        public UnityEvent onMaxWaterFavor;
        public UnityEvent onMaxEarthFavor;
        public UnityEvent onHalfHumanFavor;

        private bool atHalf = false;

        private Allegiance allegiance;

        private void Start()
        {
            allegiance = GetComponentInParent<Allegiance>();

            FavorManager.onFavorChanged.AddListener(DetectArmySpawn);
            FavorManager.onFavorChanged.AddListener(DetectMaxFireFavor);
            FavorManager.onFavorChanged.AddListener(DetectMaxWaterFavor);
            FavorManager.onFavorChanged.AddListener(DetectMaxEarthFavor);
        }

        private void DetectArmySpawn(Faction faction, MinorGod god)
        {
            if (god != MinorGod.Human)
                return;

            if (FavorManager.factionToFavor[(faction, god)] >= 1)
            {
                if (allegiance.faction == faction)
                {
                    onFriendlyArmySpawned?.Invoke();
                }
                else
                {
                    onHostileArmySpawned?.Invoke();
                }
                FavorManager.onFavorChanged.RemoveListener(DetectArmySpawn);
            }

            else if (FavorManager.factionToFavor[(faction, god)] >= .5 && !atHalf)
            {
                if(allegiance.faction == faction)
                {
                    onHalfHumanFavor?.Invoke();
                    atHalf = true;
                }
            }
        }

        private void DetectMaxFireFavor(Faction faction, MinorGod god)
        {
            if (god != MinorGod.Fire)
                return;
            else if (allegiance.faction != faction)
                return;
            else
            {
                if (FavorManager.factionToFavor[(faction, god)] >= 1)
                {
                    onMaxFireFavor?.Invoke();
                    FavorManager.onFavorChanged.RemoveListener(DetectMaxFireFavor);
                }
            }
        }

        private void DetectMaxWaterFavor(Faction faction, MinorGod god)
        {
            if (god != MinorGod.Water)
                return;
            else if (allegiance.faction != faction)
                return;
            else
            {
                if (FavorManager.factionToFavor[(faction, god)] >= 1)
                {
                    onMaxWaterFavor?.Invoke();
                    FavorManager.onFavorChanged.RemoveListener(DetectMaxWaterFavor);
                }
            }
        }

        private void DetectMaxEarthFavor(Faction faction, MinorGod god)
        {
            if (god != MinorGod.Earth)
                return;
            else if (allegiance.faction != faction)
                return;
            else
            {
                if (FavorManager.factionToFavor[(faction, god)] >= 1)
                {
                    onMaxEarthFavor?.Invoke();
                    FavorManager.onFavorChanged.RemoveListener(DetectMaxEarthFavor);
                }
            }
        }
    }
}