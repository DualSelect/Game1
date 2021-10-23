using BattleFolder;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColorUpDown : MonoBehaviour
{
    public Button brueUp;
    public Button brueDown;
    public Button yelloUp;
    public Button yelloDown;
    public Button redUp;
    public Button redDown;
    public Button blackUp;
    public Button blackDown;
    public GameObject battleManager;
    BattleStatus battleStatus;
    Display display;
    DeckCardsInteractive deckCardsInteractive;
    bool initial=true;
    public void BottunInitialize()
    {
        if (initial)
        {
            battleStatus = battleManager.GetComponent<Battle>().battleStatus;
            display = battleManager.GetComponent<Display>();
            deckCardsInteractive = display.deckWindow.GetComponent<DeckCardsInteractive>();
            initial = false;
        }
        if (battleStatus.sp > 0)
        {
            brueUp.interactable = true;
            yelloUp.interactable = true;
            redUp.interactable = true;
            blackUp.interactable = true;
        }
        else
        {
            brueUp.interactable = false;
            yelloUp.interactable = false;
            redUp.interactable = false;
            blackUp.interactable = false;
        }
        brueDown.interactable = false;
        yelloDown.interactable = false;
        redDown.interactable = false;
        blackDown.interactable = false;
        deckCardsInteractive.DeckCardInteractable();
    }
    public void BrueUp()
    {
        GameObject.Find("AudioTapEffect").GetComponent<AudioController>().TapEffect(2);
        battleStatus.sp = battleStatus.sp - 1;
        battleStatus.colorUp[0] = battleStatus.colorUp[0] + 1;
        battleStatus.colorUp[4] = battleStatus.colorUp[4] + 1;

        brueDown.interactable = true;
        if (battleStatus.sp == 0)
        {
            brueUp.interactable = false;
            yelloUp.interactable = false;
            redUp.interactable = false;
            blackUp.interactable = false;
        }
        display.color[0].text = (battleStatus.color[0] + battleStatus.colorUp[0]).ToString();
        display.color[4].text = (battleStatus.color[4] + battleStatus.colorUp[4]).ToString();
        display.sp.text = battleStatus.sp.ToString();
        deckCardsInteractive.DeckCardInteractable();
    }
    public void YelloUp()
    {
        GameObject.Find("AudioTapEffect").GetComponent<AudioController>().TapEffect(2);
        battleStatus.sp = battleStatus.sp - 1;
        battleStatus.colorUp[1] = battleStatus.colorUp[1] + 1;
        battleStatus.colorUp[4] = battleStatus.colorUp[4] + 1;

        yelloDown.interactable = true;
        if (battleStatus.sp == 0)
        {
            brueUp.interactable = false;
            yelloUp.interactable = false;
            redUp.interactable = false;
            blackUp.interactable = false;
        }
        display.color[1].text = (battleStatus.color[1] + battleStatus.colorUp[1]).ToString();
        display.color[4].text = (battleStatus.color[4] + battleStatus.colorUp[4]).ToString();
        display.sp.text = battleStatus.sp.ToString();
        deckCardsInteractive.DeckCardInteractable();
    }
    public void RedUp()
    {
        GameObject.Find("AudioTapEffect").GetComponent<AudioController>().TapEffect(2);
        battleStatus.sp = battleStatus.sp - 1;
        battleStatus.colorUp[2] = battleStatus.colorUp[2] + 1;
        battleStatus.colorUp[4] = battleStatus.colorUp[4] + 1;

        redDown.interactable = true;
        if (battleStatus.sp == 0)
        {
            brueUp.interactable = false;
            yelloUp.interactable = false;
            redUp.interactable = false;
            blackUp.interactable = false;
        }
        display.color[2].text = (battleStatus.color[2] + battleStatus.colorUp[2]).ToString();
        display.color[4].text = (battleStatus.color[4] + battleStatus.colorUp[4]).ToString();
        display.sp.text = battleStatus.sp.ToString();
        deckCardsInteractive.DeckCardInteractable();
    }
    public void BlackUp()
    {
        GameObject.Find("AudioTapEffect").GetComponent<AudioController>().TapEffect(2);
        battleStatus.sp = battleStatus.sp - 1;
        battleStatus.colorUp[3] = battleStatus.colorUp[3] + 1;
        battleStatus.colorUp[4] = battleStatus.colorUp[4] + 1;

        blackDown.interactable = true;
        if (battleStatus.sp == 0)
        {
            brueUp.interactable = false;
            yelloUp.interactable = false;
            redUp.interactable = false;
            blackUp.interactable = false;
        }
        display.color[3].text = (battleStatus.color[3] + battleStatus.colorUp[3]).ToString();
        display.color[4].text = (battleStatus.color[4] + battleStatus.colorUp[4]).ToString();
        display.sp.text = battleStatus.sp.ToString();
        deckCardsInteractive.DeckCardInteractable();
    }
    public void BrueDown()
    {
        GameObject.Find("AudioTapEffect").GetComponent<AudioController>().TapEffect(0);
        battleStatus.sp = battleStatus.sp + 1;
        battleStatus.colorUp[0] = battleStatus.colorUp[0] - 1;
        battleStatus.colorUp[4] = battleStatus.colorUp[4] - 1;

        brueUp.interactable = true;
        yelloUp.interactable = true;
        redUp.interactable = true;
        blackUp.interactable = true;

        if (battleStatus.colorUp[0] == 0)
        {
            brueDown.interactable = false;
        }
        display.color[0].text = (battleStatus.color[0] + battleStatus.colorUp[0]).ToString();
        display.color[4].text = (battleStatus.color[4] + battleStatus.colorUp[4]).ToString();
        display.sp.text = battleStatus.sp.ToString();
        deckCardsInteractive.DeckCardInteractable();
        setCardCheak();
    }
    public void YelloDown()
    {
        GameObject.Find("AudioTapEffect").GetComponent<AudioController>().TapEffect(0);
        battleStatus.sp = battleStatus.sp + 1;
        battleStatus.colorUp[1] = battleStatus.colorUp[1] - 1;
        battleStatus.colorUp[4] = battleStatus.colorUp[4] - 1;

        brueUp.interactable = true;
        yelloUp.interactable = true;
        redUp.interactable = true;
        blackUp.interactable = true;

        if (battleStatus.colorUp[1] == 0)
        {
            yelloDown.interactable = false;
        }
        display.color[1].text = (battleStatus.color[1] + battleStatus.colorUp[1]).ToString();
        display.color[4].text = (battleStatus.color[4] + battleStatus.colorUp[4]).ToString();
        display.sp.text = battleStatus.sp.ToString();
        deckCardsInteractive.DeckCardInteractable();
        setCardCheak();
    }
    public void RedDown()
    {
        GameObject.Find("AudioTapEffect").GetComponent<AudioController>().TapEffect(0);
        battleStatus.sp = battleStatus.sp + 1;
        battleStatus.colorUp[2] = battleStatus.colorUp[2] - 1;
        battleStatus.colorUp[4] = battleStatus.colorUp[4] - 1;

        brueUp.interactable = true;
        yelloUp.interactable = true;
        redUp.interactable = true;
        blackUp.interactable = true;

        if (battleStatus.colorUp[2] == 0)
        {
            redDown.interactable = false;
        }
        display.color[2].text = (battleStatus.color[2] + battleStatus.colorUp[2]).ToString();
        display.color[4].text = (battleStatus.color[4] + battleStatus.colorUp[4]).ToString();
        display.sp.text = battleStatus.sp.ToString();
        deckCardsInteractive.DeckCardInteractable();
        setCardCheak();
    }
    public void BlackDown()
    {
        GameObject.Find("AudioTapEffect").GetComponent<AudioController>().TapEffect(0);
        battleStatus.sp = battleStatus.sp + 1;
        battleStatus.colorUp[3] = battleStatus.colorUp[3] - 1;
        battleStatus.colorUp[4] = battleStatus.colorUp[4] - 1;

        brueUp.interactable = true;
        yelloUp.interactable = true;
        redUp.interactable = true;
        blackUp.interactable = true;

        if (battleStatus.colorUp[3] == 0)
        {
            blackDown.interactable = false;
        }
        display.color[3].text = (battleStatus.color[3] + battleStatus.colorUp[3]).ToString();
        display.color[4].text = (battleStatus.color[4] + battleStatus.colorUp[4]).ToString();
        display.sp.text = battleStatus.sp.ToString();
        deckCardsInteractive.DeckCardInteractable();
        setCardCheak();
    }
    private void setCardCheak()
    {
        if (battleStatus.setCard < 25)
        {
            Card card = battleManager.GetComponent<Battle>().cardMaster.CardList.Find(m => m.itemId == battleStatus.deckStatus[battleStatus.setCard].unitId);
            if (card.color == "青" && (battleStatus.color[0] + battleStatus.colorUp[0]) < card.level)
            {
                display.setCard.gameObject.transform.parent.gameObject.transform.parent.gameObject.SetActive(false);
                battleStatus.sp = battleStatus.sp + card.level;
                display.sp.text = battleStatus.sp.ToString();
                battleStatus.setCard = 99;
            }
            else if (card.color == "黄" && (battleStatus.color[1] + battleStatus.colorUp[1]) < card.level)
            {
                display.setCard.gameObject.transform.parent.gameObject.transform.parent.gameObject.SetActive(false);
                battleStatus.sp = battleStatus.sp + card.level;
                display.sp.text = battleStatus.sp.ToString();
                battleStatus.setCard = 99;
            }
            else if (card.color == "赤" && (battleStatus.color[2] + battleStatus.colorUp[2]) < card.level)
            {
                display.setCard.gameObject.transform.parent.gameObject.transform.parent.gameObject.SetActive(false);
                battleStatus.sp = battleStatus.sp + card.level;
                display.sp.text = battleStatus.sp.ToString();
                battleStatus.setCard = 99;
            }
            else if (card.color == "黒" && (battleStatus.color[3] + battleStatus.colorUp[3]) < card.level)
            {
                display.setCard.gameObject.transform.parent.gameObject.transform.parent.gameObject.SetActive(false);
                battleStatus.sp = battleStatus.sp + card.level;
                display.sp.text = battleStatus.sp.ToString();
                battleStatus.setCard = 99;
            }
            else if (card.color == "無" && (battleStatus.color[4] + battleStatus.colorUp[4]) < card.level)
            {
                display.setCard.gameObject.transform.parent.gameObject.transform.parent.gameObject.SetActive(false);
                battleStatus.sp = battleStatus.sp + card.level;
                display.sp.text = battleStatus.sp.ToString();
                battleStatus.setCard = 99;
            }
        }
    }
}
