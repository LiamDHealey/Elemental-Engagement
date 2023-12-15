
using ElementalEngagement.Favor;
using ElementalEngagement.Player;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


[RequireComponent(typeof(TMP_Text))]
public class SpawnCapDisplay : MonoBehaviour
{
    public MinorGod god;

    TMP_Text text;
    Allegiance allegiance;
    (Faction, MinorGod) allegianceKey => (allegiance.faction, god);

    // Start is called before the first frame update
    void Start()
    {
        text = GetComponent<TMP_Text>();
        allegiance = GetComponentInParent<Allegiance>();
    }

    // Update is called once per frame
    void Update()
    {
        text.text = $"{Spawner.spawnedObjects[allegianceKey].Count}/{Spawner.spawnCaps[allegianceKey]}";
    }
}
