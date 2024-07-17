using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using TMPro;

public class TestUI : MonoBehaviour
{
    public GameObject impressiveObj;
    public List<GameObject> listStars = new List<GameObject>();
    public List<GameObject> probarReward = new List<GameObject>();
    public Slider sliderProbar;

    [Header("Jump")]
    public GameObject inputCell;
    public Transform transform;
    public float jumpPower;
    public int numJump;
    public float duration;
    public bool snapping;


    void Start()
    {
        InWinPopup();
    }

    public void InWinPopup()
    {
        impressiveObj.transform.localScale = Vector3.zero;

        impressiveObj.transform.DOScale(Vector3.one, 2f).SetEase(Ease.InOutBack);

        StartCoroutine("StarsAnimation");

        inputCell.transform.DOJump(transform.position, jumpPower, numJump, duration, snapping);
    }

    IEnumerator StarsAnimation()
    {
        foreach (GameObject star in listStars)
        {
            star.transform.localScale = Vector3.zero;
        }
        yield return new WaitForSeconds(1f);
        foreach (GameObject obj in listStars)
        {
            obj.transform.DOScale(Vector3.one, 2f).SetEase(Ease.OutBack);
            yield return new WaitForSeconds(1f);
        }

        sliderProbar.DOValue(3, 3f, false);
    }
}
