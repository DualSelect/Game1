using BattleFolder;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CardPrefabEdit : MonoBehaviour
{
    public string cardId;
    public int cardNum;
    public GameObject unitStatus;
    public CardMaster cardMaster;
    public DeckEditCard selectCard;
    public Display display;
    public DeckEdit deckEdit;

    private void Start()
    {
        CardInitialEdit cardInitialEdit = GameObject.Find("CardList").GetComponent<CardInitialEdit>();
        unitStatus = cardInitialEdit.unitStatusWindow;
        display = cardInitialEdit.display;
        selectCard = cardInitialEdit.selectCard;
        deckEdit = GameObject.Find("DeckEdit").GetComponent<DeckEdit>();
        cardMaster = cardInitialEdit.cardMaster;
    }
    public void CardClick()
    {
        GameObject.Find("AudioTapEffect").GetComponent<AudioController>().TapEffect(0);
        StartCoroutine(CardClick2());
    }
    public IEnumerator CardClick2()
    {
        Card card = cardMaster.CardList.Find(m => m.itemId == cardId);
        yield return display.CardDisplay(card, selectCard.card);
        yield return DisplayLV(card.level,selectCard.gameObject.transform.GetChild(2).gameObject.GetComponent<Image>());
        selectCard.cardNum = cardNum;
        selectCard.cardId = cardId;
        selectCard.stock.text = cardNum + "/" + cardMaster.CardList.Find(m => m.itemId == cardId).stock;
        if (selectCard.clickNumber != 99 && cardNum < cardMaster.CardList.Find(m => m.itemId == cardId).stock) deckEdit.deckEditCards[selectCard.clickNumber].CardSelect();
        GameObject.Find("CardList").SetActive(false);
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
    private IEnumerator DisplayLV(int lv, Image image)
    {
        var unit = Addressables.LoadAssetAsync<Sprite>("サンセリフホワイト48_" + lv);
        yield return unit;
        image.sprite = unit.Result;
        image.gameObject.SetActive(true);
    }

}
