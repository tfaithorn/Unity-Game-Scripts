using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

public class Drag : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public RectTransform objectToMove;
    private Canvas canvas;
    private Vector2 originalPosition;
    private Vector2 originalClickPosition;

    private void Awake()
    {
        canvas = GetComponentInParent<Canvas>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        var pointerEventData = (PointerEventData)eventData;

        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            (RectTransform)canvas.transform,
            pointerEventData.position,
            canvas.worldCamera,
            out originalClickPosition);

        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            (RectTransform)canvas.transform,
            objectToMove.position,
            canvas.worldCamera,
            out originalPosition);

    }

    public void OnDrag(PointerEventData data)
    {
        PointerEventData pointerEventData = (PointerEventData)data;
        Vector2 position;

        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            (RectTransform)canvas.transform,
            pointerEventData.position,
            canvas.worldCamera,
            out position);

        objectToMove.position = canvas.transform.TransformPoint(originalPosition + (position - originalClickPosition));
    }

    public void OnEndDrag(PointerEventData eventData)
    {

    }


        public void DragHandler(BaseEventData data)
    {
        PointerEventData pointerEventData = (PointerEventData)data;

        Vector2 position;
        
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            (RectTransform)canvas.transform,
            pointerEventData.position,
            canvas.worldCamera,
            out position);

        objectToMove.position = canvas.transform.TransformPoint(position);
    }

}
