using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragObj : MonoBehaviour, IBeginDragHandler, IEndDragHandler
{
    private Vector2 prevPos;
    public void OnBeginDrag(PointerEventData data)
    {
        Debug.Log("OnBeginDrag");
        prevPos = transform.position;
    }
    /*
    public void OnDrag(PointerEventData data)
    {
        Vector2 vector2 = new Vector2(data.position.x * 0.00625f, data.position.y * 0.00625f);
        transform.position = vector2;
    }
    */
    public void OnEndDrag(PointerEventData data)
    {
        Debug.Log("OnEndDrag");
        transform.position = prevPos;
    }
}
