using BattleFolder;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ShieldPrefabEdit : MonoBehaviour
{

    public string shieldId;
    public int cardNum;
    public GameObject shieldStatus;
    public ShieldMaster shieldMaster;
    public DeckEditShield selectShield;
    public Display display;
    public DeckEdit deckEdit;

    private void Start()
    {
        ShieldInitialEdit shieldInitialEdit = GameObject.Find("ShieldList").GetComponent<ShieldInitialEdit>();
        shieldStatus = shieldInitialEdit.shieldStatusWindow;
        display = shieldInitialEdit.display;
        selectShield = shieldInitialEdit.selectShield;
        deckEdit = GameObject.Find("DeckEdit").GetComponent<DeckEdit>();
        shieldMaster = shieldInitialEdit.shieldMaster;
    }
    public void ShieldClick()
    {
        GameObject.Find("AudioTapEffect").GetComponent<AudioController>().TapEffect(0);
        StartCoroutine(ShieldClick2());
    }
    public IEnumerator ShieldClick2()
    {
        Shield shield = shieldMaster.ShieldList.Find(m => m.itemId == shieldId);
        yield return display.ShieldDisplay(shield, selectShield.card);
        selectShield.transform.GetChild(0).gameObject.transform.GetChild(0).gameObject.SetActive(true);
        selectShield.transform.GetChild(1).gameObject.SetActive(true);
        selectShield.cardNum = cardNum;
        selectShield.shieldId = shieldId;
        selectShield.stock.text = cardNum + "/" + shieldMaster.ShieldList.Find(m => m.itemId == shieldId).stock;
        if (selectShield.clickNumber != 99 && cardNum < shieldMaster.ShieldList.Find(m => m.itemId == shieldId).stock) deckEdit.deckEditShields[selectShield.clickNumber].ShieldSelect();
        GameObject.Find("ShieldList").SetActive(false);
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
            Shield shield = shieldMaster.ShieldList.Find(m => m.itemId == shieldId);
            ShieldStatusWindow shieldStatusWindow = shieldStatus.GetComponent<ShieldStatusWindow>();
            yield return shieldStatusWindow.ShieldStatusWindowOpen(shield, display);

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
