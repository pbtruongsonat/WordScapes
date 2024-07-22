using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelButton : MonoBehaviour
{
    public Button button;
    public int id;
    public TextMeshProUGUI textId;

    // Start is called before the first frame update
    void Start()
    {
        button = GetComponent<Button>();
        textId = GetComponentInChildren<TextMeshProUGUI>();
        button.onClick.AddListener(() => OnClickButton());
    }

    public void SetIDLevel(int id)
    {
        this.id = id;
        textId.text = id.ToString();
    }

    public void OnClickButton()
    {
        this.gameObject.GetComponentInParent<ListLevelScroll>().RemoveLevel(this.gameObject);
    }
}
