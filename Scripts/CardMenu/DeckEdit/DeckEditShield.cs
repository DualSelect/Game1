using BattleFolder;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DeckEditShield : MonoBehaviour
{
    public int number;
    public int clickNumber;
    public string shieldId;
    public Image card;
    public int cardNum;
    public Text stock;
    public DeckEditShield selectShield;
    public Display display;
    public GameObject shieldStatus;
    public ShieldMaster shieldMaster;
    public DeckEdit deckEdit;
    public GameObject shieldList;
    public Text life;

    public void ShieldSelect()
    {
        StartCoroutine(ShieldSelect2());
    }
    public IEnumerator ShieldSelect2()
    {
        selectShield.clickNumber = number;
        GameObject.Find("AudioTapEffect").GetComponent<AudioController>().TapEffect(0);
        if (selectShield.shieldId == "")
        {
            shieldList.SetActive(true);
            yield return shieldList.GetComponent<ShieldInitialEdit>().FilterSearch();
        }
        else
        {
            if (selectShield.cardNum < shieldMaster.ShieldList.Find(m => m.itemId == selectShield.shieldId).stock)
            {
                shieldId = selectShield.shieldId;
                life.text = shieldMaster.ShieldList.Find(m => m.itemId == shieldId).life.ToString();
                selectShield.cardNum++;
                selectShield.stock.text = selectShield.cardNum + "/" + shieldMaster.ShieldList.Find(m => m.itemId == shieldId).stock;
                yield return display.ShieldDisplay(shieldId, card);
                this.gameObject.transform.GetChild(0).gameObject.transform.GetChild(0).gameObject.SetActive(true);
                selectShield.shieldId = "";
            }
            else
            {
                shieldList.SetActive(true);
                yield return shieldList.GetComponent<ShieldInitialEdit>().FilterSearch();
            }
        }
    }
    public void ShieldDelete()
    {
        GameObject.Find("AudioTapEffect").GetComponent<AudioController>().TapEffect(0);
        if (selectShield.shieldId == shieldId)
        {
            selectShield.cardNum--;
            selectShield.stock.text = selectShield.cardNum + "/" + shieldMaster.ShieldList.Find(m => m.itemId == shieldId).stock;
        }
        shieldId = "";
        life.text = "";
        this.gameObject.transform.GetChild(0).transform.GetChild(0).gameObject.gameObject.SetActive(false);
    }
    public void ShieldDelete2()
    {
        GameObject.Find("AudioTapEffect").GetComponent<AudioController>().TapEffect(0);
        selectShield.stock.text = "";
        shieldId = "";
        this.gameObject.transform.GetChild(0).transform.GetChild(0).gameObject.gameObject.SetActive(false);
        this.gameObject.transform.GetChild(1).gameObject.SetActive(false);
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

