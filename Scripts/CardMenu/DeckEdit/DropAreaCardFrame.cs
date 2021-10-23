using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DropAreaCardFrame : MonoBehaviour, IDropHandler
{
    public void OnDrop(PointerEventData data)
    {
        DragObj dragObj = data.pointerDrag.GetComponent<DragObj>();
        if (dragObj != null)
        {
            DeckEditCard drag = data.pointerDrag.transform.parent.parent.parent.gameObject.GetComponent<DeckEditCard>();
            DeckEditCard drop = gameObject.GetComponent<DeckEditCard>();


            Debug.Log(drop.cardId + "に" + drag.cardId + "をドロップ");
            string cardTmp = drop.cardId;
            drop.cardId = drag.cardId;
            drag.cardId = cardTmp;

            StartCoroutine(drop.display.CardDisplay(drop.cardId, drop.card));
            StartCoroutine(drop.DisplayLV(drop.cardMaster.CardList.Find(m => m.itemId == drop.cardId).level, drop.transform.GetChild(2).gameObject.GetComponent<Image>()));

            drag.transform.GetChild(0).gameObject.SetActive(false);
            drag.transform.GetChild(2).gameObject.SetActive(false);
        }
    }
}