using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InputManager : MonoSingleton<InputManager>, IPointerDownHandler, IDragHandler, IBeginDragHandler
{
    public static Action<PointerEventData> OnFingerTap;
    public static Action<SwipeDirection> OnFingerSwipe;

    private Vector2 firstTouchPosition;

    [SerializeField] private float swipeThreshold;

    public void OnPointerDown(PointerEventData eventData)
    {
        OnFingerTap?.Invoke(eventData);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        firstTouchPosition = eventData.position;
    }

    public void OnDrag(PointerEventData eventData)
    {
        var endTouchPos = eventData.position;

        var x = endTouchPos.x - firstTouchPosition.x;
        var y = endTouchPos.y - firstTouchPosition.y;

        if (Mathf.Abs(x) - Mathf.Abs(y) > swipeThreshold)
        {
            if (x > 0)
                OnFingerSwipe?.Invoke(SwipeDirection.Right);
            else
                OnFingerSwipe?.Invoke(SwipeDirection.Left);
        }
    }


}

public enum SwipeDirection
{
    Left,
    Right
}