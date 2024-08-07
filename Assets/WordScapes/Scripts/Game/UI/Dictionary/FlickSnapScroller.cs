using EnhancedUI.EnhancedScroller;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class FlickSnapScroller : MonoBehaviour, IBeginDragHandler, IEndDragHandler
{
    public EnhancedScroller scroller;
    public EnhancedScroller.TweenType snapTweenType;
    public float snapTweenTime;
    public int MaxDataElements { get; set; }
    private bool _isDragging = false;

    private Vector2 _dragStartPosition = Vector2.zero;
    private int _currentIndex = -1;

    public void OnBeginDrag(PointerEventData data)
    {
        if (scroller.IsTweening)
        {
            return;
        }

        _isDragging = true;

        _dragStartPosition = data.position;

        _currentIndex = scroller.StartDataIndex;
    }

    public void OnEndDrag(PointerEventData data)
    {
        if (_isDragging)
        {
            _isDragging = false;

            var delta = data.position - _dragStartPosition;

            var jumpToIndex = -1;

            if (delta.y < 0)
            {
                jumpToIndex = _currentIndex - 1;
            }
            else if (delta.y > 0)
            {
                jumpToIndex = _currentIndex + 1;
            }

            if (jumpToIndex != -1)
            {
                scroller.JumpToDataIndex(Mathf.Clamp(jumpToIndex, 0, MaxDataElements - 1), tweenType: snapTweenType, tweenTime: snapTweenTime);
            }
        }
    }
}