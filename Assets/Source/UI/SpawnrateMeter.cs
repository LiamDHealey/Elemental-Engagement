using ElementalEngagement.Favor;
using ElementalEngagement.Player;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace ElementalEngagement.UI
{

    [RequireComponent(typeof(TMP_Text))]
    public class SpawnrateMeter : MonoBehaviour
    {
        [Tooltip("The format of the text. {Spawnrate} will be replaced with the actual spawnrate.")]
        public string format = "×{Spawnrate} Spawn Rate";


        private Allegiance allegiance;
        private TMP_Text text;

        private void Start()
        {
            text = GetComponent<TMP_Text>();
            allegiance = GetComponent<Allegiance>();
        }

        // Update is called once per frame
        private void Update()
        {
            text.text = format.Replace("{Spawnrate}", SpawnrateProvider.GetSpawnrateMultiplier(allegiance).ToString());
        }
    }
}