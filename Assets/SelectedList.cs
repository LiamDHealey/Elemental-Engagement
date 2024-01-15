using ElementalEngagement.Player;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SelectedList : MonoBehaviour
{

    public GameObject template;
    [SerializeField] List<SerailizedDude> sprites;

    private SelectionInputHandler selectionInputHandler;
    private void Start()
    {
        selectionInputHandler = GetComponentInParent<SelectionInputHandler>();
    }

    // Update is called once per frame
    void Update()
    {
        foreach(Transform child in transform)
        {
            if (child == template.transform)
                continue;

            Destroy(child.gameObject);
        }

        foreach (var uniqueUnit in selectionInputHandler.selectedObjects
            .Select(s => s.tag)
            .Distinct()
            .ToDictionary(tag => tag, tag => selectionInputHandler.selectedObjects
                                                .Distinct()
                                                .Count(s => s != null && s.tag == tag)))
        {
            GameObject item = Instantiate(template, transform);
            item.GetComponentInChildren<TMP_Text>().text = $"×{uniqueUnit.Value}";
            item.GetComponentsInChildren<Image>()[1].sprite = sprites.First(s => s.unitName == uniqueUnit.Key).sprite;
            item.SetActive(true);
        }
    }

    [System.Serializable]
    private class SerailizedDude
    {
        public Sprite sprite;
        public string unitName;
    }
}
