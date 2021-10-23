using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;

//見た目の変更用
public class Display : MonoBehaviour
{
    public Text time;
    public Text phase;
    public Text turn;
    public Text life;
    public Text sp;
    public Text[] color;
    public Text deck;
    public Text death;
    public Text gauge;
    public Text stock;
    public Text lifeEnemy;
    public Text spEnemy;
    public Text[] colorEnemy;
    public Text deckEnemy;
    public Text deathEnemy;
    public Text gaugeEnemy;
    public Text stockEnemy;
    public Text specialStatus;
    public Image[] shieldCards;
    public Image[] shieldCardsEnemy;
    public GameObject[] shieldBreak;
    public Image[] deckUnits;
    public Image[] fieldUnit;
    public Image[] fieldUnitEnemy;
    public GameObject[] hpBar;
    public Text[] damage;
    public GameObject[] hpBarEnemy;
    public Text[] damageEnemy;
    public Image[] trashCard;
    public Image[] trashCardEnemy;
    public Image[] vanishCard;
    public Image[] vanishCardEnemy;
    public GameObject[] target;
    public GameObject[] targetEnemy;
    public GameObject[] targetRevival;
    public Image special;
    public Image setCard;
    public Image setCardEnemy;
    public Button okButton;
    public Button okDeckButton;
    public GameObject specialCard;
    public GameObject colorUpDown;
    public GameObject deckWindow;
    public GameObject unitStatusWindow;
    public GameObject specialStatusWindow;
    public GameObject shieldStatusWindow;
    public GameObject waitMessage;
    public GameObject unitSkillWindow;
    public GameObject unitSkillWindowEnemy;
    public GameObject shieldSkillWindow;
    public GameObject shieldSkillWindowEnemy;
    public GameObject specialSkillWindow;
    public GameObject specialSkillWindowEnemy;
    public GameObject matchingWindow;
    public GameObject turnDisplay;
    public Text turnDisplayText;
    public GameObject openDisplay;
    public DeckCardsInteractive deckCardsInteractive;
    public GameObject command;
    public GameObject[] atackBreak;
    public GameObject[] move;
    public GameObject[] action1;
    public GameObject[] action2;
    public CardMaster cardMaster;
    public ShieldMaster shieldMaster;
    public SpecialMaster specialMaster;
    public GameObject finishEffecseer;
    public SpecialEffect specialEffect;

    public IEnumerator CardFlavorDisplay(Card card, Image image)
    {
        if (card.type == "魔法")
        {
            var unit = Addressables.LoadAssetAsync<Sprite>("dummy");
            yield return unit;
            image.sprite = unit.Result;

            var background = Addressables.LoadAssetAsync<Sprite>(card.id);
            yield return background;
            image.gameObject.transform.parent.gameObject.transform.parent.gameObject.GetComponent<Image>().sprite = background.Result;
        }
        else
        {
            var unit = Addressables.LoadAssetAsync<Sprite>(card.id);
            yield return unit;
            image.sprite = unit.Result;

            var background = Addressables.LoadAssetAsync<Sprite>("dummy");
            yield return background;
            image.gameObject.transform.parent.gameObject.transform.parent.gameObject.GetComponent<Image>().sprite = background.Result;

        }
        var frame = Addressables.LoadAssetAsync<Sprite>("枠" + card.color + "2");
        yield return frame;
        image.gameObject.transform.parent.gameObject.GetComponent<Image>().sprite = frame.Result;

        image.gameObject.transform.parent.gameObject.transform.parent.gameObject.SetActive(true);
    }
    public IEnumerator CardDisplay(Card card, Image image)
    {
        if (card.type == "魔法") 
        {
            var unit = Addressables.LoadAssetAsync<Sprite>("dummy");
            yield return unit;
            image.sprite = unit.Result;

            var background = Addressables.LoadAssetAsync<Sprite>(card.id);
            yield return background;
            image.gameObject.transform.parent.gameObject.transform.parent.gameObject.GetComponent<Image>().sprite = background.Result;
        }
        else
        {
            var unit = Addressables.LoadAssetAsync<Sprite>(card.id);
            yield return unit;
            image.sprite = unit.Result;

            var background = Addressables.LoadAssetAsync<Sprite>("背景" + card.color + "1");
            yield return background;
            image.gameObject.transform.parent.gameObject.transform.parent.gameObject.GetComponent<Image>().sprite = background.Result;

        }
        var frame = Addressables.LoadAssetAsync<Sprite>("枠" + card.color + "2");
        yield return frame;
        image.gameObject.transform.parent.gameObject.GetComponent<Image>().sprite = frame.Result;

        image.gameObject.transform.parent.gameObject.transform.parent.gameObject.SetActive(true);
    }
    public IEnumerator CardDisplay(string cardId, Image image)
    {
        Card card = cardMaster.CardList.Find(m => m.itemId == cardId);
        yield return CardDisplay(card, image);
    }
    public IEnumerator DeckStateDisplay(int deckNumber,int deckState)
    {
        if (deckState > 0)
        {
            if(deckState!=4) deckUnits[deckNumber].gameObject.transform.GetChild(1).gameObject.transform.GetChild(0).gameObject.GetComponent<Text>().text = "";
            var state = Addressables.LoadAssetAsync<Sprite>("state" + deckState);
            yield return state;
            deckUnits[deckNumber].gameObject.transform.GetChild(1).gameObject.GetComponent<Image>().sprite = state.Result;
            deckUnits[deckNumber].gameObject.transform.GetChild(1).gameObject.SetActive(true);
            deckUnits[deckNumber].gameObject.GetComponent<Button>().interactable = false;
        }
        else
        {
            deckUnits[deckNumber].gameObject.transform.GetChild(1).gameObject.SetActive(false);
        }
    }
    public IEnumerator DeckStateDisplay(int deckNumber, int deckState,int lockTurn)
    {
        deckUnits[deckNumber].gameObject.transform.GetChild(1).gameObject.transform.GetChild(0).gameObject.GetComponent<Text>().text = lockTurn.ToString();
        yield return DeckStateDisplay(deckNumber, deckState);
    }
    public IEnumerator ShieldDisplay(Shield shield, Image image)
    {
        var unit = Addressables.LoadAssetAsync<Sprite>(shield.id);
        yield return unit;
        image.sprite = unit.Result;
        image.gameObject.SetActive(true);
    }
    public IEnumerator ShieldDisplay(string shieldId, Image image)
    {
        Shield shield = shieldMaster.ShieldList.Find(m => m.itemId == shieldId);
        yield return ShieldDisplay(shield, image);
    }
    public IEnumerator SpecialDisplay(Special special, Image image)
    {
        var unit = Addressables.LoadAssetAsync<Sprite>(special.id);
        yield return unit;
        image.sprite = unit.Result;
        var color = Addressables.LoadAssetAsync<Sprite>("奥義ストック"+special.color);
        yield return color;
        image.gameObject.transform.parent.gameObject.GetComponent<Image>().sprite = color.Result;
    }
    public IEnumerator SpecialDisplay(string specialId, Image image)
    {
        Special special = specialMaster.SpecialList.Find(m => m.id == specialId);
        yield return SpecialDisplay(special, image);
    }
}