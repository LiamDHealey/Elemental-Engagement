
using ElementalEngagement.Favor;
using ElementalEngagement.Player;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


namespace ElementalEngagement.UI
{
    [RequireComponent(typeof(TMP_Text))]
    public class SpawnCapDisplay : MonoBehaviour
    {
        TMP_Text text;
        Allegiance allegiance;

        // Start is called before the first frame update
        void Start()
        {
            text = GetComponent<TMP_Text>();
            allegiance = GetComponentInParent<Allegiance>();
        }

        // Update is called once per frame
        void Update()
        {
            text.text = $"{Spawner.spawnedObjects[allegiance.faction].Count}/{Spawner.spawnCaps[allegiance.faction]} units";
        }
    }

}