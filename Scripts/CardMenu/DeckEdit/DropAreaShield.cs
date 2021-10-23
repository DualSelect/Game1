using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DropAreaShield : MonoBehaviour, IDropHandler
{
    public void OnDrop(PointerEventData data)
    {
        DragObj dragObj = data.pointerDrag.GetComponent<DragObj>();
        if (dragObj != null)
        {
            DeckEditShield drag = data.pointerDrag.transform.parent.parent.gameObject.GetComponent<DeckEditShield>();
            DeckEditShield drop = gameObject.transform.parent.parent.gameObject.GetComponent<DeckEditShield>();


            Debug.Log(drop.shieldId + "に" + drag.shieldId + "をドロップ");
            string cardTmp = drop.shieldId;
            drop.shieldId = drag.shieldId;
            drag.shieldId = cardTmp;

            StartCoroutine(drop.display.ShieldDisplay(drop.shieldId, drop.card));
            drop.life.text = drop.shieldMaster.ShieldList.Find(m => m.itemId == drop.shieldId).life.ToString();

            StartCoroutine(drag.display.ShieldDisplay(drag.shieldId, drag.card));
            drag.life.text = drag.shieldMaster.ShieldList.Find(m => m.itemId == drag.shieldId).life.ToString();
        }
    }
}