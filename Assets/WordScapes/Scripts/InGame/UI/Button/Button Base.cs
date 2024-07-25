using UnityEngine;
using UnityEngine.UI;

public abstract class ButtonBase : MonoBehaviour
{
    [SerializeField] protected Button button;
    [SerializeField] protected RectTransform rectTransform;

    protected virtual void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(() => this.OnClick());
    }
    protected void Reset()
    {
        this.LoadComponents();
    }
    protected virtual void LoadComponents()
    {
        button = GetComponent<Button>();
        rectTransform = GetComponent<RectTransform>();
    }
    protected virtual void OnClick()
    {
        Debug.Log("OnClick In Base");
    }
    protected abstract void OnEnableButton(bool enable);

}
