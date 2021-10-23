using BattleFolder;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TargetAndCheck : MonoBehaviour
{
    public int areaNumber;
    public GameObject battleManager;
    BattleStatus battleStatus;
    Display display;
    public bool enemy;
    void Start()
    {
        display = battleManager.GetComponent<Display>();
    }
    public void TargetClick()
    {
        Battle battle = battleManager.GetComponent<Battle>();
        if (battle.targetCount > 0)
        {
            GameObject.Find("AudioTapEffect").GetComponent<AudioController>().TapEffect(2);
            battle.targetCount = battle.targetCount - 1;
            this.gameObject.GetComponent<Button>().enabled = false;
            this.gameObject.transform.GetChild(0).gameObject.SetActive(true);
            if (!enemy){
                battle.targetSelect[0, areaNumber] = true;
            }
            else
            {
                battle.targetSelect[1, areaNumber] = true;
            }
            if (battle.targetCount == 0)
            {
                display.okButton.gameObject.GetComponent<Button>().interactable=true;
                for(int i=0; i < display.target.Length; i++)
                {
                    display.target[i].GetComponent<Button>().interactable = false;
                    display.targetEnemy[i].GetComponent<Button>().interactable = false;
                }
            }
        } 
    }
    public void TargetClickRevival()
    {
        Battle battle = battleManager.GetComponent<Battle>();
        GameObject.Find("AudioTapEffect").GetComponent<AudioController>().TapEffect(2);
        battleStatus = battleManager.GetComponent<Battle>().battleStatus;


        bool revivalCheak = false;
        int i;
        for (i = 0; i < battleStatus.deckStatus.Length; i++)
        {
            if (battleStatus.deckStatus[i].unitId == battleStatus.unitStatus[areaNumber].unitId && battleStatus.deckStatus[i].playStatus == 0)
            {
                revivalCheak = true;
                break;
            }
        }

        if (revivalCheak)
        {
            StartCoroutine(battle.TrushCard(i, 2));
            if (battleManager.GetComponent<Battle>().cardMaster.CardList.Find(m => m.itemId == battleStatus.unitStatus[areaNumber].unitId).textSkillName != "復帰") battle.battleStatus.sp = battle.battleStatus.sp - 1;
            display.sp.text = battle.battleStatus.sp.ToString();
            this.gameObject.GetComponent<Button>().enabled = false;
            this.gameObject.transform.GetChild(0).gameObject.SetActive(true);
            battle.targetSelect[0, areaNumber] = true;
            if (battle.battleStatus.sp == 0)
            {
                for (int j = 0; j < display.target.Length; j++)
                {
                    if(battleManager.GetComponent<Battle>().cardMaster.CardList.Find(m=>m.itemId == battleStatus.unitStatus[areaNumber].unitId).textSkillName!="復帰")display.targetRevival[j].GetComponent<Button>().interactable = false;
                }
            }
        }
    }

    public void CheckClick()
    {
        GameObject.Find("AudioTapEffect").GetComponent<AudioController>().TapEffect(0);
        Battle battle = battleManager.GetComponent<Battle>();
        display.okButton.gameObject.GetComponent<Button>().interactable = false;
        this.gameObject.transform.parent.gameObject.GetComponent<Button>().enabled = true;
        battle.targetCount = battle.targetCount + 1;
        if (!enemy)
        {
            battle.targetSelect[0, areaNumber] = false;
        }
        else
        {
            battle.targetSelect[1, areaNumber] = false;
        }
        this.gameObject.SetActive(false);
        for (int i = 0; i < display.target.Length; i++)
        {
            display.target[i].GetComponent<Button>().interactable = true;
            display.targetEnemy[i].GetComponent<Button>().interactable = true;
        }
    }
    public void CheckClickRevival()
    {
        GameObject.Find("AudioTapEffect").GetComponent<AudioController>().TapEffect(0);
        Battle battle = battleManager.GetComponent<Battle>();
        battleStatus = battleManager.GetComponent<Battle>().battleStatus;
        int i;
        for (i = battleStatus.deckStatus.Length-1; i >= 0; i--)
        {
            if (battleStatus.trash[0,i] == battleStatus.unitStatus[areaNumber].unitId)
            {
                break;
            }
        }
        StartCoroutine(battle.ReviveCard(i, false));//iは一致した墓地番目

        this.gameObject.transform.parent.gameObject.GetComponent<Button>().enabled = true;
        if (battleManager.GetComponent<Battle>().cardMaster.CardList.Find(m => m.itemId == battleStatus.unitStatus[areaNumber].unitId).textSkillName != "復帰") battle.battleStatus.sp = battle.battleStatus.sp + 1;
        display.sp.text = battle.battleStatus.sp.ToString();
        battle.targetSelect[0, areaNumber] = false;
        this.gameObject.SetActive(false);
        if (battle.battleStatus.sp>0)
            for (int j = 0; j < display.targetRevival.Length; j++)
            {
                display.targetRevival[j].GetComponent<Button>().interactable = true;
            }
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
            UnitStatusWindow unitStatusWindow = display.unitStatusWindow.GetComponent<UnitStatusWindow>();
            Card card = new Card();
            if (!enemy)
            {
                battleStatus = battleManager.GetComponent<Battle>().battleStatus;
            }
            else
            {
                battleStatus = battleManager.GetComponent<Battle>().battleStatusEnemy;

            }
            UnitStatus unitStatus = battleStatus.unitStatus[areaNumber];
            card = battleManager.GetComponent<Battle>().cardMaster.CardList.Find(m => m.itemId == unitStatus.unitId);
            yield return unitStatusWindow.UnitStatusWindowOpen(card, display);
            unitStatusWindow.rare.text = card.rare + "_" + card.pack;
            unitStatusWindow.lv.text = (card.level + unitStatus.upLV + unitStatus.tupLV) + "[" + unitStatus.upLV + "]" + "(" + unitStatus.tupLV + ")";
            unitStatusWindow.unitName.text = card.name;
            unitStatusWindow.hp.text = unitStatus.nowHP.ToString();
            unitStatusWindow.mhp.text = (card.hp + unitStatus.upHP + unitStatus.tupHP) + "[" + unitStatus.upHP + "]" + "(" + unitStatus.tupHP + ")";
            unitStatusWindow.stk.text = (card.stock + unitStatus.upSTK + unitStatus.tupSTK) + "[" + unitStatus.upSTK + "]" + "(" + unitStatus.tupSTK + ")";
            unitStatusWindow.atk.text = (card.atk + unitStatus.upATK + unitStatus.tupATK) + "[" + unitStatus.upATK + "]" + "(" + unitStatus.tupATK + ")";
            unitStatusWindow.def.text = (card.dfe + unitStatus.upDFE + unitStatus.tupDFE) + "[" + unitStatus.upDFE + "]" + "(" + unitStatus.tupDFE + ")";
            unitStatusWindow.agi.text = (card.agi + unitStatus.upAGI + unitStatus.tupAGI) + "[" + unitStatus.upAGI + "]" + "(" + unitStatus.tupAGI + ")";
            unitStatusWindow.rng.text = (card.rng + unitStatus.upRNG + unitStatus.tupRNG) + "[" + unitStatus.upRNG + "]" + "(" + unitStatus.tupRNG + ")";

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
