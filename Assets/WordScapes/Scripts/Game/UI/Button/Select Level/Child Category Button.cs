using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChildCategoryButton : ButtonBase
{
    public string nameChild;
    public bool onSelect;

    [Header("Component")]
    public Image childBackground;
    public Image borderSelected;
    public TextMeshProUGUI textNameChild;

    public GameObject unlockedChild;
    public GameObject lockedChild;


    public void SetChild(ChildCategory child)
    {
        if (child == null)
        {
            unlockedChild.SetActive(false);
            lockedChild.SetActive(true);
        }
        else
        {
            unlockedChild.SetActive(true);
            lockedChild.SetActive(false);

            nameChild = child.name;
            textNameChild.text = nameChild;
            childBackground.sprite = child.backgroundImage;
        }
    }

    protected override void OnClick()
    {
        onSelect = !onSelect;
        borderSelected.gameObject.SetActive(onSelect);
        Debug.Log(onSelect);
    }
}
