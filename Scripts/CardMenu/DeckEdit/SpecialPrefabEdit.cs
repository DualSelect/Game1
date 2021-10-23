using BattleFolder;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SpecialPrefabEdit : MonoBehaviour
{
    public string specialId;
    public GameObject specialStatus;
    public GameObject specialFlavor;
    public SpecialMaster specialMaster;
    public DeckEditSpecial selectSpecial;
    public Display display;

    private void Start()
    {
        SpecialInitialEdit specialInitialEdit = GameObject.Find("SpecialList").GetComponent<SpecialInitialEdit>();
        specialStatus = specialInitialEdit.specialStatusWindow;
        specialFlavor = specialInitialEdit.specialFlavorWindow;
        display = specialInitialEdit.display;
        selectSpecial = specialInitialEdit.selectSpecial;
        specialMaster = specialInitialEdit.specialMaster;
    }
    public void SpecialClick()
    {
        GameObject.Find("AudioTapEffect").GetComponent<AudioController>().TapEffect(0);
        StartCoroutine(SpecialClick2());
    }
    public IEnumerator SpecialClick2()
    {
        yield return display.SpecialDisplay(specialId, selectSpecial.card);
        selectSpecial.specialId = specialId;
        GameObject.Find("SpecialList").SetActive(false);
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
            SpecialStatusWindow specialStatusWindow = specialStatus.GetComponent<SpecialStatusWindow>();
            Special special = specialMaster.SpecialList.Find(m => m.id == specialId);
            yield return specialStatusWindow.OpenSpecialStatusWindow(special, display);
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
