using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BattleFolder;
public class BattleCard : MonoBehaviour
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
    public void BattleCardClick()
    {
        GameObject.Find("AudioTapEffect").GetComponent<AudioController>().TapEffect(0);
        StartCoroutine(BattleCardClick2());
    }
    public IEnumerator BattleCardClick2()
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
        unitStatusWindow.lv.text = (card.level+unitStatus.upLV + unitStatus.tupLV) + "["+ unitStatus.upLV + "]"+"("+unitStatus.tupLV+")";
        unitStatusWindow.stk.text = (card.stock + unitStatus.upSTK + unitStatus.tupSTK) + "[" + unitStatus.upSTK + "]" + "(" + unitStatus.tupSTK + ")";
        unitStatusWindow.unitName.text = card.name;
        unitStatusWindow.hp.text = unitStatus.nowHP.ToString();
        unitStatusWindow.mhp.text = (card.hp + unitStatus.upHP + unitStatus.tupHP) + "[" + unitStatus.upHP + "]" + "(" + unitStatus.tupHP + ")";
        unitStatusWindow.atk.text = (card.atk + unitStatus.upATK + unitStatus.tupATK) + "[" + unitStatus.upATK + "]" + "(" + unitStatus.tupATK + ")";
        unitStatusWindow.def.text = (card.dfe + unitStatus.upDFE + unitStatus.tupDFE) + "[" + unitStatus.upDFE + "]" + "(" + unitStatus.tupDFE + ")";
        unitStatusWindow.agi.text = (card.agi + unitStatus.upAGI + unitStatus.tupAGI) + "[" + unitStatus.upAGI + "]" + "(" + unitStatus.tupAGI + ")";
        unitStatusWindow.rng.text = (card.rng + unitStatus.upRNG + unitStatus.tupRNG) + "[" + unitStatus.upRNG + "]" + "(" + unitStatus.tupRNG + ")";
    }
}
