
using DG.Tweening;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIPopupReward : UIPopupBase
{
    [Header("Component")]
    public TextMeshProUGUI textLevelID;
    public TextMeshProUGUI textComplete;
    public TextMeshProUGUI textValueReward;
    public int valueReward;

    public Button collectButton;

    [Header("Coin Move")]
    public GameObject coinMovePrefab;
    public RectTransform rectForm;
    public RectTransform rectTo;

    [Header("Jump Settings")]
    public float jumpPower;
    public int numJump;
    public float duration;

    private void Start()
    {
        collectButton.onClick.AddListener(() => CollectCoinReward());
    }

    private void CollectCoinReward()
    {
        collectButton.gameObject.SetActive(false);
        StartCoroutine(IEDisplayCoinMove());
    }

    IEnumerator IEDisplayCoinMove()
    {
        for(int i = 0; i < 5; i++)
        {
            var coinMove = Instantiate(coinMovePrefab, rectForm.position, rectForm.rotation);
            coinMove.transform.DOJump(rectTo.position, jumpPower, numJump, duration);
            yield return new WaitForSeconds(0.1f);
        }
        DataManager.EarnCoin(valueReward);
        OnDisablePopup();
    }

    public override void OnEnablePopup()
    {
        base.OnEnablePopup();
        textLevelID.text = $"LEVEL {GameManager.Instance.currentLevel}";

        valueReward = Random.Range(10, 30);
        textValueReward.text = valueReward.ToString();
    }

    public override void OnDisablePopup()
    {
        base.OnDisablePopup();
        DOVirtual.DelayedCall(0.2f, () => { gameObject.SetActive(false); });

        UIManager.Instance.DisplayMainMenu();
    }

    private void OnEnable()
    {
        OnEnablePopup();
    }
}
