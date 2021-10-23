using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DropAreaCard : MonoBehaviour, IDropHandler
{
    public void OnDrop(PointerEventData data)
    {
        DragObj dragObj = data.pointerDrag.GetComponent<DragObj>();
        if (dragObj != null)
        {
            DeckEditCard drag = data.pointerDrag.transform.parent.parent.parent.gameObject.GetComponent<DeckEditCard>();
            DeckEditCard drop = gameObject.transform.parent.parent.parent.gameObject.GetComponent<DeckEditCard>();


            Debug.Log(drop.cardId + "に" + drag.cardId + "をドロップ");
            string cardTmp = drop.cardId;
            drop.cardId = drag.cardId;
            drag.cardId = cardTmp;

            StartCoroutine(drop.display.CardDisplay(drop.cardId, drop.card));
            StartCoroutine(drop.DisplayLV(drop.cardMaster.CardList.Find(m => m.itemId == drop.cardId).level, drop.transform.GetChild(2).gameObject.GetComponent<Image>()));

            StartCoroutine(drag.display.CardDisplay(drag.cardId, drag.card));
            StartCoroutine(drag.DisplayLV(drag.cardMaster.CardList.Find(m => m.itemId == drag.cardId).level, drag.transform.GetChild(2).gameObject.GetComponent<Image>()));
        }
    }
}