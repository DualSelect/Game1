using BattleFolder;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DeckCardsInteractive : MonoBehaviour
{
    public Button[] deckCards;
    public Button specialCard;
    public Button specialSwitch;
    public GameObject battleManager;
    BattleStatus battleStatus;
    BattleStatus battleStatusEnemy;
    Battle battle;
    bool initial = true;

    public void DeckCardInteractable()
    {
        if (initial)
        {
            battle = battleManager.GetComponent<Battle>();
            battleStatus = battle.battleStatus;
            battleStatusEnemy = battle.battleStatusEnemy;
            for (int i = 0; i < 25; i++)
            {
                deckCards[i].interactable = false;
            }
            specialCard.interactable = false;
            initial = false;
        }
        bool areaSpace = false;
        for (int i = 0; i < 9; i++)
        {
            if (battleStatus.unitStatus[i].unitId == "")
            {
                areaSpace = true;
                break;
            }
        }
        for (int i = 0; i < 25; i++)
        {
            if (battleStatus.deckStatus[i].playStatus == 0)
            {
                Card card = battle.cardMaster.CardList.Find(m => m.itemId == battleStatus.deckStatus[i].unitId);
                if (battleStatus.sp >= card.level && battle.battleStatus.deckStatus[i].playStatus == 0)
                {
                    bool colorCost = card.color == "青" && (battleStatus.color[0] + battleStatus.colorUp[0]) >= card.level;
                    colorCost = colorCost || (card.color == "黄" && (battleStatus.color[1] + battleStatus.colorUp[1]) >= card.level);
                    colorCost = colorCost || (card.color == "赤" && (battleStatus.color[2] + battleStatus.colorUp[2]) >= card.level);
                    colorCost = colorCost || (card.color == "黒" && (battleStatus.color[3] + battleStatus.colorUp[3]) >= card.level);
                    colorCost = colorCost || (card.color == "無" && (battleStatus.color[4] + battleStatus.colorUp[4]) >= card.level);
                    if (colorCost)
                    {
                        bool namedMatch = false;
                        if ((card.type == "ネームド" || card.type == "ユニット") && !areaSpace)
                        {
                            deckCards[i].interactable = false;
                        }
                        else if (card.type == "ネームド")
                        {
                            if (card.id == battle.prevSetCardEnemy) namedMatch = true;
                            for (int j = 0; j < 9; j++)
                            {
                                if (card.id == battleStatus.unitStatus[j].unitId || card.id == battleStatusEnemy.unitStatus[j].unitId)
                                {
                                    namedMatch = true;
                                }
                                if (namedMatch) break;
                            }

                        }
                        if (namedMatch)
                        {
                            deckCards[i].interactable = false;
                            if (battle.cardMaster.CardList.Find(m => m.itemId == battleStatus.deckStatus[i].unitId).type == "魔法") deckCards[i].gameObject.transform.parent.gameObject.transform.parent.gameObject.GetComponent<Image>().color = new Color(0.5f, 0.5f, 0.5f);
                        }
                        else
                        {
                            deckCards[i].interactable = true;
                            if (battle.cardMaster.CardList.Find(m => m.itemId == battleStatus.deckStatus[i].unitId).type == "魔法") deckCards[i].gameObject.transform.parent.gameObject.transform.parent.gameObject.GetComponent<Image>().color = new Color(1f, 1f, 1f);
                        }


                    }
                    else
                    {
                        deckCards[i].interactable = false;
                        if (battle.cardMaster.CardList.Find(m => m.itemId == battleStatus.deckStatus[i].unitId).type == "魔法") deckCards[i].gameObject.transform.parent.gameObject.transform.parent.gameObject.GetComponent<Image>().color = new Color(0.5f, 0.5f, 0.5f);
                    }
                }
                else
                {
                    deckCards[i].interactable = false;
                    if (battle.cardMaster.CardList.Find(m => m.itemId == battleStatus.deckStatus[i].unitId).type == "魔法") deckCards[i].gameObject.transform.parent.gameObject.transform.parent.gameObject.GetComponent<Image>().color = new Color(0.5f, 0.5f, 0.5f);
                }
            }
        }
    }
    public void DeckCardInteractableFalse()
    {
        for (int i = 0; i < 25; i++)
        {
            deckCards[i].interactable = false;
            if(battle.cardMaster.CardList.Find(m => m.itemId == battleStatus.deckStatus[i].unitId).type=="魔法") deckCards[i].gameObject.transform.parent.gameObject.transform.parent.gameObject.GetComponent<Image>().color = new Color(0.5f, 0.5f, 0.5f);
        }
    }
}
