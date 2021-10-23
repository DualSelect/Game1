using BattleFolder;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DeckEditCard : MonoBehaviour
{
    public int number;
    public int clickNumber;
    public string cardId;
    public Image card;
    public int cardNum;
    public Text stock;
    public DeckEditCard selectCard;
    public Display display;
    public GameObject unitStatus;
    public CardMaster cardMaster;
    public DeckEdit deckEdit;
    public GameObject cardList;

    public void CardSelect()
    {
        StartCoroutine(CardSelect2());
    }
    public IEnumerator CardSelect2()
    {
        GameObject.Find("AudioTapEffect").GetComponent<AudioController>().TapEffect(0);
        selectCard.clickNumber = number;
        if (selectCard.cardId == "")
        {
            cardList.SetActive(true);
            yield return cardList.GetComponent<CardInitialEdit>().FilterSearch();
        }
        else
        {
            if (selectCard.cardNum < cardMaster.CardList.Find(m => m.itemId == selectCard.cardId).stock)
            {
                cardId = selectCard.cardId;
                selectCard.cardNum++;
                selectCard.stock.text = selectCard.cardNum + "/" + cardMaster.CardList.Find(m => m.itemId == cardId).stock;
                yield return display.CardDisplay(cardId, card);
                yield return DisplayLV(cardMaster.CardList.Find(m => m.itemId == cardId).level, this.gameObject.transform.GetChild(2).gameObject.GetComponent<Image>());
            }
            else
            {
                cardList.SetActive(true);
                yield return cardList.GetComponent<CardInitialEdit>().FilterSearch();
            }
        }
    }
    public void CardDelete()
    {
        GameObject.Find("AudioTapEffect").GetComponent<AudioController>().TapEffect(0);
        if(selectCard.cardId == cardId)
        {
            selectCard.cardNum--;
            selectCard.stock.text = selectCard.cardNum + "/" + cardMaster.CardList.Find(m => m.itemId == cardId).stock;
        }
        cardId = "";
        this.gameObject.transform.GetChild(0).gameObject.SetActive(false);
        this.gameObject.transform.GetChild(2).gameObject.SetActive(false);
    }
        //EventTriggerをアタッチしておく
    public EventTrigger _EventTrigger;
    void Awake()
    {
        //PointerDownイベントの登録
        EventTrigger.Entry pressdown = new EventTrigger.Entry();
        pressdown.eventID = EventTriggerType.PointerDown;
        pressdown.callback.AddListener((data) => PointerDown());
        _EventTrigger.triggers.Add(pressdown);

        //PointerUpイベントの登録
        EventTrigger.Entry pressup = new EventTrigger.Entry();
        pressup.eventID = EventTriggerType.PointerUp;
        pressup.callback.AddListener((data) => PointerUp());
        _EventTrigger.triggers.Add(pressup);
    }


    //StopCoroutineのためにCoroutineで宣言しておく
    Coroutine PressCorutine;
    bool isPressDown = false;
    float PressTime = 2f;



    //EventTriggerのPointerDownイベントに登録する処理
    public void PointerDown()
    {
        Debug.Log("Press Start");
        //連続でタップした時に長押しにならないよう前のCoroutineを止める
        if (PressCorutine != null)
        {
            StopCoroutine(PressCorutine);
        }
        //StopCoroutineで止められるように予め宣言したCoroutineに代入
        PressCorutine = StartCoroutine(TimeForPointerDown());
    }

    //長押しコルーチン
    IEnumerator TimeForPointerDown()
    {
        //プレス開始
        isPressDown = true;

        //待機時間
        yield return new WaitForSeconds(PressTime);

        //押されたままなら長押しの挙動
        if (isPressDown)
        {
            GameObject.Find("AudioTapEffect").GetComponent<AudioController>().TapEffect(0);
            UnitStatusWindow unitStatusWindow = unitStatus.GetComponent<UnitStatusWindow>();
            Card card = cardMaster.CardList.Find(m => m.itemId == cardId);
            yield return unitStatusWindow.UnitStatusWindowOpen(card, display);

        }
        //プレス処理終了
        isPressDown = false;
    }

    //EventTriggerのPointerUpイベントに登録する処理
    public void PointerUp()
    {
        if (isPressDown)
        {
            Debug.Log("Short Press Done");
            isPressDown = false;

            //お好みの短押し時の挙動をここに書く(無い場合は書かなくても良い)

        }
        Debug.Log("Press End");
    }
    public IEnumerator DisplayLV(int lv, Image image)
    {
        var unit = Addressables.LoadAssetAsync<Sprite>("サンセリフホワイト48_" + lv);
        yield return unit;
        image.sprite = unit.Result;
        image.gameObject.SetActive(true);
    }

}
