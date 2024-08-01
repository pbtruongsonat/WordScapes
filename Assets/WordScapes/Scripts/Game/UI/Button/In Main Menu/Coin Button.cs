using UnityEngine;

public class CoinButton : ResourceButton
{
    protected override void Start()
    {
        base.Start();
        value = DataManager.coin;
        textValue.text = value.ToString();
    }

    protected override void OnClick()
    {
        OnValueChange(Random.Range(0, 1000));
    }

    private void OnEnable()
    {
        GameEvent.coinChanged += OnValueChange;
    }

    private void OnDisable()
    {
        GameEvent.coinChanged -= OnValueChange;
    }
}
