using ElementalEngagement.Favor;
using ElementalEngagement.Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace ElementalEngagement.UI
{
    public class HumanArmyDetector : MonoBehaviour
    {
        public Allegiance allegiance;

        public UnityEvent onFriendlyArmySpawned;
        public UnityEvent onHostileArmySpawned;

        private void Start()
        {
            FavorManager.onFavorChanged.AddListener(DetectArmySpawn);

            void DetectArmySpawn(Faction faction, MinorGod god)
            {
                if (god != MinorGod.Human)
                    return;

                if (FavorManager.factionToFavor[(faction, god)] >= 1)
                {
                    (allegiance.faction == faction ? onFriendlyArmySpawned : onHostileArmySpawned)?.Invoke();
                    FavorManager.onFavorChanged.RemoveListener(DetectArmySpawn);
                }
            }
        }
    }
}