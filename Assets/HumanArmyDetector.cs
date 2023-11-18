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
        public UnityEvent onFriendlyArmySpawned;
        public UnityEvent onHostileArmySpawned;

        private Allegiance allegiance;

        private void Start()
        {
            allegiance = GetComponentInParent<Allegiance>();

            FavorManager.onFavorChanged.AddListener(DetectArmySpawn);

            void DetectArmySpawn(Faction faction, MinorGod god)
            {
                if (god != MinorGod.Human)
                    return;

                if (FavorManager.factionToFavor[(faction, god)] >= 1)
                {
                    if (allegiance.faction == faction)
                    {
                        Debug.Log(allegiance.faction.ToString() + "Friendly");
                        onFriendlyArmySpawned?.Invoke();
                    }
                    else
                    {
                        Debug.Log(allegiance.faction.ToString() + "Hostile");
                        onHostileArmySpawned?.Invoke();
                    }
                    FavorManager.onFavorChanged.RemoveListener(DetectArmySpawn);
                }
            }
        }
    }
}