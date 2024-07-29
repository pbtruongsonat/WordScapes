using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChildCategoryButton : ButtonBase
{
    public ChildCategory child;
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
            button.interactable = false;
        }
        else
        {
            this.child = child;
            unlockedChild.SetActive(true);
            lockedChild.SetActive(false);
            button.interactable = true;

            nameChild = this.child.name;
            textNameChild.text = nameChild;
            childBackground.sprite = this.child.backgroundImage;
        }
    }

    private void OnSelectChild(bool selected)
    {
        onSelect = selected;
        borderSelected.gameObject.SetActive(onSelect);
    }

    protected override void OnClick()
    {
        bool selectTmp = !onSelect;
        GameEvent.changeChildSelect?.Invoke(false);
        OnSelectChild(selectTmp);
        GameEvent.displayListLevel(child, selectTmp);
    }

    private void OnEnable()
    {
        GameEvent.changeChildSelect += OnSelectChild;
    }

    private void OnDisable()
    {
        GameEvent.changeChildSelect -= OnSelectChild;
    }
}
