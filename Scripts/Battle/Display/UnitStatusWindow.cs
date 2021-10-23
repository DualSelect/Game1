using BattleFolder;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;

public class UnitStatusWindow : MonoBehaviour
{
    public Text rare;
    public Text unitName;
    public Text lv;
    public Text hp;
    public Text mhp;
    public Text stk;
    public Text atk;
    public Text def;
    public Text agi;
    public Text rng;
    public Text[] skillName;
    public Text[] skillType;
    public Text[] skillMana;
    public Text[] skillDetail;
    public Image card;
    public GameObject flavor;
    public Text type;
    public Text type1;
    public Text type2;

    public CardInitial cardInitial;

    public void UnitStatusWindowClose()
    {
        GameObject.Find("AudioTapEffect").GetComponent<AudioController>().TapEffect(0);
        this.gameObject.SetActive(false);
    }
    public void UnitFlavor()
    {
        GameObject.Find("AudioTapEffect").GetComponent<AudioController>().TapEffect(0);
        this.gameObject.SetActive(false);
        flavor.SetActive(true);
    }
    public IEnumerator UnitStatusWindowOpen(Card card,Display display)
    {
        if (card.type == "ユニット") type.text = "汎用ユニット";
        if (card.type == "魔法") type.text = "魔法";
        if (card.type == "ネームド") type.text = "固有ユニット";
        if (card.type1 != "") { type1.text = card.type1; } else { type1.text = "なし"; }
        if (card.type2 != "") { type2.text = card.type2; } else { type2.text = "なし"; }

        rare.text = card.rare + "_" + card.pack;
        lv.text = card.level.ToString();
        unitName.text = card.name;
        hp.text = card.hp.ToString();
        mhp.text = card.hp.ToString();
        stk.text = card.stock.ToString();
        atk.text = card.atk.ToString();
        def.text = card.dfe.ToString();
        agi.text = card.agi.ToString();
        rng.text = card.rng.ToString();

        int i = 0;
        if (card.openSkillName != "")
        {
            skillName[i].text = card.openSkillName;
            skillType[i].text = "公開";
            skillMana[i].text = card.openSkillSp.ToString() + "マナ";
            skillDetail[i].text = card.openSkillDetail;
            i++;
        }
        if (card.startSkillName != "")
        {
            skillName[i].text = card.startSkillName;
            skillType[i].text = "開始";
            skillMana[i].text = card.startSkillSp.ToString() + "マナ";
            skillDetail[i].text = card.startSkillDetail;
            i++;
        }
        if (card.autoSkillName != "")
        {
            skillName[i].text = card.autoSkillName;
            skillType[i].text = "自動";
            skillMana[i].text = card.autoSkillSp.ToString() + "マナ";
            skillDetail[i].text = card.autoSkillDetail;
            i++;
        }
        if (card.actionSkillName1 != "")
        {
            skillName[i].text = card.actionSkillName1;
            skillType[i].text = "行動";
            skillMana[i].text = card.actionSkillSp1.ToString() + "マナ";
            skillDetail[i].text = card.actionSkillDetail1;
            i++;
        }
        if (card.actionSkillName2 != "")
        {
            skillName[i].text = card.actionSkillName2;
            skillType[i].text = "行動";
            skillMana[i].text = card.actionSkillSp2.ToString() + "マナ";
            skillDetail[i].text = card.actionSkillDetail2;
            i++;
        }
        if (card.closeSkillName != "")
        {
            skillName[i].text = card.closeSkillName;
            skillType[i].text = "後退";
            skillMana[i].text = card.closeSkillSp.ToString() + "マナ";
            skillDetail[i].text = card.closeSkillDetail;
            i++;
        }
        if (card.textSkillName != "")
        {
            skillName[i].text = card.textSkillName;
            skillType[i].text = "特殊";
            skillMana[i].text = card.textSkillSp.ToString() + "マナ";
            skillDetail[i].text = card.textSkillDetail;
            i++;
        }
        if (card.rankUp != "")
        {
            skillName[i].text = "成長";
            string str = "";
            BattleJson battleJson = new BattleJson();
            RankUp rankUp = battleJson.toRankUp(card.rankUp);
            str = rankUp.color + "属性" + rankUp.num + "以上";
            if (rankUp.level != 0) str = str + "　レベル:" + rankUp.level;
            if (rankUp.stock != 0) str = str + "　ストック:" + rankUp.stock;
            if (rankUp.hp != 0) str = str + "　HP:" + rankUp.hp;
            if (rankUp.atk != 0) str = str + "　攻撃:" + rankUp.atk;
            if (rankUp.dfe != 0) str = str + "　防御:" + rankUp.dfe;
            if (rankUp.agi != 0) str = str + "　速さ:" + rankUp.agi;
            if (rankUp.rng != 0) str = str + "　射程:" + rankUp.rng;
            skillDetail[i].text = str;
        }
        while(i < 4)
        {
            skillName[i].text = "";
            skillType[i].text = "";
            skillMana[i].text = "";
            skillDetail[i].text = "";
            i++;
        }
        yield return display.CardDisplay(card, this.card);
        this.gameObject.SetActive(true);




        UnitFlavorWindow unitFlavorWindow = flavor.GetComponent<UnitFlavorWindow>();
        /*
        var unit = Addressables.LoadAssetAsync<Sprite>("枠" + card.rare);
        yield return unit;
        unitFlavorWindow.frame.sprite = unit.Result;
        */
        unitFlavorWindow.unitName.text = card.name;
        unitFlavorWindow.flavor.text = card.flavor;
        unitFlavorWindow.illust.text = card.illust;
        unitFlavorWindow.cardId = card.id;
        yield return display.CardFlavorDisplay(card, unitFlavorWindow.card);
        if(unitFlavorWindow.cardWin!=null)yield return cardInitial.CardWin(card,unitFlavorWindow.cardWin,unitFlavorWindow.aibouButton);

    }
}
