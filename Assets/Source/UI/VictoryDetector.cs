using ElementalEngagement.Player;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace ElementalEngagement.UI
{
    public class VictoryDetector : MonoBehaviour
    {
        [Tooltip("Maps each player to a win event")]
        [SerializeField] private List<PlayerToEvent> playersToWinEvents;


        private void Start()
        {
            DefeatManager.onPlayerLost.AddListener(OnPLayerLost);
        }

        /// <summary>
        /// Called when a player loses. Calls the appropriate victory event. 
        /// </summary>
        /// <param name="loser"> The loser. </param>
        private void OnPLayerLost(Faction loser)
        {
            if (DefeatManager.survivingFactions.Count != 1)
            {
                return;
            }

            // Get wining player win event.
            playersToWinEvents.FirstOrDefault(ptwe => ptwe.faction == DefeatManager.survivingFactions[0])?.onWon?.Invoke();
            Time.timeScale = 0;
        }

        /// <summary>
        /// Serializable wrapper.
        /// </summary>
        [System.Serializable]
        private class PlayerToEvent
        {
            public Faction faction;
            public UnityEvent onWon;
        }
    }
}
