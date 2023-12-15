using ElementalEngagement.Player;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameStartPopup : MonoBehaviour
{
    float originalTime = 0;
    private TMP_Text textBox;

    // Start is called before the first frame update
    void Start()
    {
        textBox = GetComponentInChildren<TMP_Text>();
        AllegianceManager.onGameStarted.AddListener(() => Destroy(gameObject));
        originalTime = AllegianceManager.timeUntilGameStart;
    }

    // Update is called once per frame
    void Update()
    {
        if (AllegianceManager.timeUntilGameStart != originalTime)
        {
            textBox.text = $"Game starting in {Mathf.CeilToInt(AllegianceManager.timeUntilGameStart)}";
        }
    }
}
