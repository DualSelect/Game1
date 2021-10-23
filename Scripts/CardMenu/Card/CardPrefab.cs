using BattleFolder;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;

public class CardPrefab : MonoBehaviour
{
    public string cardId;
    public GameObject unitStatus;
    public CardMaster cardMaster;
    public Display display;

    private void Start()
    {
        CardInitial cardInitial = GameObject.Find("CardList").GetComponent<CardInitial>();
        unitStatus = cardInitial.unitStatusWindow;
        display = cardInitial.display;
        cardMaster = cardInitial.cardMaster;
    }
    public void CardClick()
    {
        StartCoroutine(CardClicl2());
    }
    public IEnumerator CardClicl2()
    {
        GameObject.Find("AudioTapEffect").GetComponent<AudioController>().TapEffect(0);
        UnitStatusWindow unitStatusWindow = unitStatus.GetComponent<UnitStatusWindow>();
        Card card = cardMaster.CardList.Find(m => m.itemId == cardId);
        yield return unitStatusWindow.UnitStatusWindowOpen(card, display);
    }
}