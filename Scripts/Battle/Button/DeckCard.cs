using BattleFolder;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.EventSystems;

public class DeckCard : MonoBehaviour
{
    public int cardNumber;
    public GameObject battleManager;
    BattleStatus battleStatus;
    Display display;
    void Start()
    {
        battleStatus = battleManager.GetComponent<Battle>().battleStatus;
        display = battleManager.GetComponent<Display>();


    }
    public void DeckCardClick()
    {
        if (battleStatus.setCard == cardNumber)
        {
            GameObject.Find("AudioTapEffect").GetComponent<AudioController>().TapEffect(0);
            battleStatus.setCard = 99;
            display.setCard.gameObject.transform.parent.gameObject.transform.parent.gameObject.SetActive(false);
            battleStatus.sp = battleStatus.sp + battleManager.GetComponent<Battle>().cardMaster.CardList.Find(m => m.itemId == battleStatus.deckStatus[cardNumber].unitId).level;
            display.sp.text = battleStatus.sp.ToString();
        }
        else
        {
            GameObject.Find("AudioTapEffect").GetComponent<AudioController>().TapEffect(2);
            StartCoroutine(SetCardImage());
            if (battleStatus.setCard < 25)
            {
                battleStatus.sp = battleStatus.sp + battleManager.GetComponent<Battle>().cardMaster.CardList.Find(m => m.itemId == battleStatus.deckStatus[battleStatus.setCard].unitId).level;
                display.sp.text = battleStatus.sp.ToString();
            }
            battleStatus.setCard = cardNumber;
            battleStatus.sp = battleStatus.sp - battleManager.GetComponent<Battle>().cardMaster.CardList.Find(m => m.itemId == battleStatus.deckStatus[cardNumber].unitId).level;
            display.sp.text = battleStatus.sp.ToString();
            if(battleStatus.sp == 0)
            {
                display.colorUpDown.GetComponent<ColorUpDown>().brueUp.interactable = false;
                display.colorUpDown.GetComponent<ColorUpDown>().yelloUp.interactable = false;
                display.colorUpDown.GetComponent<ColorUpDown>().redUp.interactable = false;
                display.colorUpDown.GetComponent<ColorUpDown>().blackUp.interactable = false;
            }
        }
    }
    public IEnumerator SetCardImage()
    {
        Card card = battleManager.GetComponent<Battle>().cardMaster.CardList.Find(m => m.itemId == battleStatus.deckStatus[cardNumber].unitId);
        yield return display.CardDisplay(card, display.setCard);
        display.setCard.gameObject.transform.parent.gameObject.transform.parent.gameObject.SetActive(true);
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
            UnitStatusWindow unitStatusWindow = display.unitStatusWindow.GetComponent<UnitStatusWindow>();
            Card card = battleManager.GetComponent<Battle>().cardMaster.CardList.Find(m => m.itemId == battleStatus.deckStatus[cardNumber].unitId);
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





}
