using Gs2.Core;
using Gs2.Unity;
using Gs2.Unity.Gs2Datastore.Result;
using Gs2.Unity.Gs2Formation.Model;
using Gs2.Unity.Gs2Formation.Result;
using Gs2.Unity.Gs2Inventory.Result;
using Gs2.Unity.Util;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Text;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DeckEdit : MonoBehaviour
{
    public DeckEditCard[] deckEditCards;
    public DeckEditCard selectCard;
    public DeckEditShield[] deckEditShields;
    public DeckEditShield selectShield;
    public DeckEditSpecial deckEditSpecial;
    public Text deckTitle;
    public GameObject message;
    int getItem = 0;
    public CardMaster cardMaster;
    public ShieldMaster shieldMaster;
    public SpecialMaster specialMaster;
    List<EzSlotWithSignature> slots = new List<EzSlotWithSignature>();

    GameObject login;
    Client gs2;
    GameSession session;
    public CardInitialEdit cardInitialEdit;
    public ShieldInitialEdit shieldInitialEdit;
    public InputField inputField;
    public GameObject error;
    public InputField deckCode;
    public GameObject codeWindow;


    private void Start()
    {
        login = GameObject.Find("Login");
        gs2 = login.GetComponent<LoginInitial>().GetClient();
        session = login.GetComponent<LoginInitial>().GetSession();
        StartCoroutine(cardInitialEdit.ListItem());
        StartCoroutine(shieldInitialEdit.ListItem());
        StartCoroutine(DeckLoad());
        StartCoroutine(DeckName());
    }
    public void BackButtunDown()
    {
        GameObject.Find("AudioTapEffect").GetComponent<AudioController>().TapEffect(0);
        PlayerPrefs.SetString("menu", "deck");
        SceneManager.LoadScene("MainMenu");
    }
    public void SaveButtunDown()
    {
        GameObject.Find("AudioTapEffect").GetComponent<AudioController>().TapEffect(0);
        GameObject.Find("Loading").GetComponent<Loading>().LoadingStart();
        bool cheak = false;
        for (int i = 0; i < deckEditCards.Length; i++)
        {
            if (deckEditCards[i].cardId == "") cheak = true;
        }
        for (int i = 0; i < deckEditShields.Length; i++)
        {
            if (deckEditShields[i].shieldId == "") cheak = true;
        }
        if (deckEditSpecial.specialId == "") cheak = true;

        if (cheak)
        {
            GameObject.Find("Loading").GetComponent<Loading>().LoadingEnd();
            message.SetActive(true);
        }
        else
        {
            StartCoroutine(DeckSave());
        }
        //PlayerPrefs.SetString("deckName" + PlayerPrefs.GetInt("deckNum"), deckTitle.text);
    }
    private IEnumerator DeckSave()
    {
        for (int i = 0; i < deckEditCards.Length; i++)
        {
            Card card = cardMaster.CardList.Find(m => m.itemId == deckEditCards[i].cardId);
            StartCoroutine(SetSlots(card.inventory, card.inventory, card.itemId, "card" + (i + 1)));
        }
        for (int i = 0; i < deckEditShields.Length; i++)
        {
            Shield shield = shieldMaster.ShieldList.Find(m => m.itemId == deckEditShields[i].shieldId);
            StartCoroutine(SetSlots(shield.inventory, shield.inventory, shield.itemId, "shield" + (i + 1)));
        }
        Special special = specialMaster.SpecialList.Find(m => m.id == deckEditSpecial.specialId);
        StartCoroutine(SetSlots("special", "special", special.itemId, "special"));


        while (getItem != 31)
        {
            yield return new WaitForSeconds(1.0f);
        }
        {
            AsyncResult<EzSetFormResult> asyncResult = null;
            var current = gs2.Formation.SetForm(
                  r => { asyncResult = r; },
                  session: session,
                  namespaceName: "deck",
                  moldName: "deck",
                  index: PlayerPrefs.GetInt("deckNum"),
                  slots: slots,
                  keyId: "grn:gs2:ap-northeast-1:uFLAkqDK-Development:key:inventory-key:key:inventory-key"
            );
            yield return current;
            if (asyncResult.Error != null)
            {
                OnErrorSave(asyncResult.Error);
                yield break;
            }
        }
        {
            AsyncResult<EzPrepareUploadResult> asyncResult = null;
            var current = gs2.Datastore.PrepareUpload(
                  r => { asyncResult = r; },
                  session: session,
                  namespaceName: "datastore",
                  name: "deck" + PlayerPrefs.GetInt("deckNum").ToString(),
                  scope: "private",
                  allowUserIds: null,
                  updateIfExists: true
            );
            yield return current;
            if (asyncResult.Error != null)
            {
                OnError(asyncResult.Error);
                yield break;
            }
            byte[] myData = System.Text.Encoding.UTF8.GetBytes(deckTitle.text);
            UnityWebRequest www = UnityWebRequest.Put(asyncResult.Result.UploadUrl, myData);
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.Log(www.error);
                yield break;
            }
        }
        {
            AsyncResult<EzDoneUploadResult> asyncResult = null;
            var current = gs2.Datastore.DoneUpload(
                  r => { asyncResult = r; },
                  session: session,
                  namespaceName: "datastore",
                  dataObjectName: "deck" + PlayerPrefs.GetInt("deckNum").ToString()
            );
            yield return current;
            if (asyncResult.Error != null)
            {
                OnError(asyncResult.Error);
                yield break;
            }
        }
        if (PlayerPrefs.GetString("deckEdit") == "battle")
        {
            PlayerPrefs.SetString("menu", "battle");
        }
        else
        {
            PlayerPrefs.SetString("menu", "deck");
        }
        GameObject.Find("Loading").GetComponent<Loading>().LoadingEnd();
        SceneManager.LoadScene("MainMenu");
    }
    private IEnumerator CardCheak(string namespaceName, string inventoryName, string itemName, UnityEngine.Events.UnityAction<bool> callback)
    {
        AsyncResult<EzGetItemWithSignatureResult> asyncResult = null;
        var current = gs2.Inventory.GetItemWithSignature(
              r => { asyncResult = r; },
              session: session,
              namespaceName: namespaceName,
              inventoryName: inventoryName,
              itemName: itemName,
              keyId: "grn:gs2:ap-northeast-1:uFLAkqDK-Development:key:inventory-key:key:inventory-key"
        );
        yield return current;
        if (asyncResult.Error != null)
        {
            OnError(asyncResult.Error);
            yield break;
        }
        if (asyncResult.Result.Items.Count == 0) callback(false);
        else callback(true);
    }
    public IEnumerator SetSlots(string namespaceName, string inventoryName, string itemName, string slotName)
    {
        AsyncResult<EzGetItemWithSignatureResult> asyncResult = null;
        var current = gs2.Inventory.GetItemWithSignature(
              r => { asyncResult = r; },
              session: session,
              namespaceName: namespaceName,
              inventoryName: inventoryName,
              itemName: itemName,
              keyId: "grn:gs2:ap-northeast-1:uFLAkqDK-Development:key:inventory-key:key:inventory-key"
        );
        yield return current;
        if (asyncResult.Error != null)
        {
            OnError(asyncResult.Error);
            yield break;
        }
        var item = asyncResult.Result;
        EzSlotWithSignature slot = new EzSlotWithSignature();
        slot.PropertyType = "gs2_inventory";
        slot.Body = item.Body;
        slot.Signature = item.Signature;
        slot.Name = slotName;
        slots.Add(slot);
        getItem++;
        Debug.Log(getItem);
    }
    public void MessageClose()
    {
        GameObject.Find("AudioTapEffect").GetComponent<AudioController>().TapEffect(0);
        message.SetActive(false);
    }
    private void OnError(Exception e)
    {
        Debug.Log(e.ToString());
    }
    private void OnErrorSave(Exception e)
    {
        Debug.Log(e.ToString());
        error.SetActive(true);
    }
    private IEnumerator DeckLoad()
    {
        //inputField.text = PlayerPrefs.GetString("deckName" + PlayerPrefs.GetInt("deckNum"));
        AsyncResult<EzGetFormResult> asyncResult = null;
        var current = gs2.Formation.GetForm(
              r => { asyncResult = r; },
              session: session,
              namespaceName: "deck",
              moldName: "deck",
              index: PlayerPrefs.GetInt("deckNum")
        );
        yield return current;
        if (asyncResult.Error != null)
        {
            OnError(asyncResult.Error);
            yield break;
        }
        for (int i = 0; i < asyncResult.Result.Item.Slots.Count; i++)
        {
            if (asyncResult.Result.Item.Slots[i].Name == "card1")
            {

                string itemId = GetBetweenStrings("item:", ":itemSet", asyncResult.Result.Item.Slots[i].PropertyId);
                DeckEditCard deckEditCard = deckEditCards[0];
                deckEditCard.cardId = itemId;
                yield return deckEditCard.display.CardDisplay(itemId, deckEditCard.card);
                yield return DisplayLV(cardMaster.CardList.Find(m => m.itemId == deckEditCard.cardId).level, deckEditCard.gameObject.transform.GetChild(2).gameObject.GetComponent<Image>());
            }
            if (asyncResult.Result.Item.Slots[i].Name == "card2")
            {

                string itemId = GetBetweenStrings("item:", ":itemSet", asyncResult.Result.Item.Slots[i].PropertyId);
                DeckEditCard deckEditCard = deckEditCards[1];
                deckEditCard.cardId = itemId;
                yield return deckEditCard.display.CardDisplay(itemId, deckEditCard.card);
                yield return DisplayLV(cardMaster.CardList.Find(m => m.itemId == deckEditCard.cardId).level, deckEditCard.gameObject.transform.GetChild(2).gameObject.GetComponent<Image>());
            }
            if (asyncResult.Result.Item.Slots[i].Name == "card3")
            {

                string itemId = GetBetweenStrings("item:", ":itemSet", asyncResult.Result.Item.Slots[i].PropertyId);
                DeckEditCard deckEditCard = deckEditCards[2];
                deckEditCard.cardId = itemId;
                yield return deckEditCard.display.CardDisplay(itemId, deckEditCard.card);
                yield return DisplayLV(cardMaster.CardList.Find(m => m.itemId == deckEditCard.cardId).level, deckEditCard.gameObject.transform.GetChild(2).gameObject.GetComponent<Image>());
            }
            if (asyncResult.Result.Item.Slots[i].Name == "card4")
            {

                string itemId = GetBetweenStrings("item:", ":itemSet", asyncResult.Result.Item.Slots[i].PropertyId);
                DeckEditCard deckEditCard = deckEditCards[3];
                deckEditCard.cardId = itemId;
                yield return deckEditCard.display.CardDisplay(itemId, deckEditCard.card);
                yield return DisplayLV(cardMaster.CardList.Find(m => m.itemId == deckEditCard.cardId).level, deckEditCard.gameObject.transform.GetChild(2).gameObject.GetComponent<Image>());
            }
            if (asyncResult.Result.Item.Slots[i].Name == "card5")
            {

                string itemId = GetBetweenStrings("item:", ":itemSet", asyncResult.Result.Item.Slots[i].PropertyId);
                DeckEditCard deckEditCard = deckEditCards[4];
                deckEditCard.cardId = itemId;
                yield return deckEditCard.display.CardDisplay(itemId, deckEditCard.card);
                yield return DisplayLV(cardMaster.CardList.Find(m => m.itemId == deckEditCard.cardId).level, deckEditCard.gameObject.transform.GetChild(2).gameObject.GetComponent<Image>());
            }
            if (asyncResult.Result.Item.Slots[i].Name == "card6")
            {

                string itemId = GetBetweenStrings("item:", ":itemSet", asyncResult.Result.Item.Slots[i].PropertyId);
                DeckEditCard deckEditCard = deckEditCards[5];
                deckEditCard.cardId = itemId;
                yield return deckEditCard.display.CardDisplay(itemId, deckEditCard.card);
                yield return DisplayLV(cardMaster.CardList.Find(m => m.itemId == deckEditCard.cardId).level, deckEditCard.gameObject.transform.GetChild(2).gameObject.GetComponent<Image>());
            }
            if (asyncResult.Result.Item.Slots[i].Name == "card7")
            {

                string itemId = GetBetweenStrings("item:", ":itemSet", asyncResult.Result.Item.Slots[i].PropertyId);
                DeckEditCard deckEditCard = deckEditCards[6];
                deckEditCard.cardId = itemId;
                yield return deckEditCard.display.CardDisplay(itemId, deckEditCard.card);
                yield return DisplayLV(cardMaster.CardList.Find(m => m.itemId == deckEditCard.cardId).level, deckEditCard.gameObject.transform.GetChild(2).gameObject.GetComponent<Image>());
            }
            if (asyncResult.Result.Item.Slots[i].Name == "card8")
            {

                string itemId = GetBetweenStrings("item:", ":itemSet", asyncResult.Result.Item.Slots[i].PropertyId);
                DeckEditCard deckEditCard = deckEditCards[7];
                deckEditCard.cardId = itemId;
                yield return deckEditCard.display.CardDisplay(itemId, deckEditCard.card);
                yield return DisplayLV(cardMaster.CardList.Find(m => m.itemId == deckEditCard.cardId).level, deckEditCard.gameObject.transform.GetChild(2).gameObject.GetComponent<Image>());
            }
            if (asyncResult.Result.Item.Slots[i].Name == "card9")
            {

                string itemId = GetBetweenStrings("item:", ":itemSet", asyncResult.Result.Item.Slots[i].PropertyId);
                DeckEditCard deckEditCard = deckEditCards[8];
                deckEditCard.cardId = itemId;
                yield return deckEditCard.display.CardDisplay(itemId, deckEditCard.card);
                yield return DisplayLV(cardMaster.CardList.Find(m => m.itemId == deckEditCard.cardId).level, deckEditCard.gameObject.transform.GetChild(2).gameObject.GetComponent<Image>());
            }
            if (asyncResult.Result.Item.Slots[i].Name == "card10")
            {

                string itemId = GetBetweenStrings("item:", ":itemSet", asyncResult.Result.Item.Slots[i].PropertyId);
                DeckEditCard deckEditCard = deckEditCards[9];
                deckEditCard.cardId = itemId;
                yield return deckEditCard.display.CardDisplay(itemId, deckEditCard.card);
                yield return DisplayLV(cardMaster.CardList.Find(m => m.itemId == deckEditCard.cardId).level, deckEditCard.gameObject.transform.GetChild(2).gameObject.GetComponent<Image>());
            }
            if (asyncResult.Result.Item.Slots[i].Name == "card11")
            {

                string itemId = GetBetweenStrings("item:", ":itemSet", asyncResult.Result.Item.Slots[i].PropertyId);
                DeckEditCard deckEditCard = deckEditCards[10];
                deckEditCard.cardId = itemId;
                yield return deckEditCard.display.CardDisplay(itemId, deckEditCard.card);
                yield return DisplayLV(cardMaster.CardList.Find(m => m.itemId == deckEditCard.cardId).level, deckEditCard.gameObject.transform.GetChild(2).gameObject.GetComponent<Image>());
            }
            if (asyncResult.Result.Item.Slots[i].Name == "card12")
            {

                string itemId = GetBetweenStrings("item:", ":itemSet", asyncResult.Result.Item.Slots[i].PropertyId);
                DeckEditCard deckEditCard = deckEditCards[11];
                deckEditCard.cardId = itemId;
                yield return deckEditCard.display.CardDisplay(itemId, deckEditCard.card);
                yield return DisplayLV(cardMaster.CardList.Find(m => m.itemId == deckEditCard.cardId).level, deckEditCard.gameObject.transform.GetChild(2).gameObject.GetComponent<Image>());
            }
            if (asyncResult.Result.Item.Slots[i].Name == "card13")
            {

                string itemId = GetBetweenStrings("item:", ":itemSet", asyncResult.Result.Item.Slots[i].PropertyId);
                DeckEditCard deckEditCard = deckEditCards[12];
                deckEditCard.cardId = itemId;
                yield return deckEditCard.display.CardDisplay(itemId, deckEditCard.card);
                yield return DisplayLV(cardMaster.CardList.Find(m => m.itemId == deckEditCard.cardId).level, deckEditCard.gameObject.transform.GetChild(2).gameObject.GetComponent<Image>());
            }
            if (asyncResult.Result.Item.Slots[i].Name == "card14")
            {

                string itemId = GetBetweenStrings("item:", ":itemSet", asyncResult.Result.Item.Slots[i].PropertyId);
                DeckEditCard deckEditCard = deckEditCards[13];
                deckEditCard.cardId = itemId;
                yield return deckEditCard.display.CardDisplay(itemId, deckEditCard.card);
                yield return DisplayLV(cardMaster.CardList.Find(m => m.itemId == deckEditCard.cardId).level, deckEditCard.gameObject.transform.GetChild(2).gameObject.GetComponent<Image>());
            }
            if (asyncResult.Result.Item.Slots[i].Name == "card15")
            {

                string itemId = GetBetweenStrings("item:", ":itemSet", asyncResult.Result.Item.Slots[i].PropertyId);
                DeckEditCard deckEditCard = deckEditCards[14];
                deckEditCard.cardId = itemId;
                yield return deckEditCard.display.CardDisplay(itemId, deckEditCard.card);
                yield return DisplayLV(cardMaster.CardList.Find(m => m.itemId == deckEditCard.cardId).level, deckEditCard.gameObject.transform.GetChild(2).gameObject.GetComponent<Image>());
            }
            if (asyncResult.Result.Item.Slots[i].Name == "card16")
            {

                string itemId = GetBetweenStrings("item:", ":itemSet", asyncResult.Result.Item.Slots[i].PropertyId);
                DeckEditCard deckEditCard = deckEditCards[15];
                deckEditCard.cardId = itemId;
                yield return deckEditCard.display.CardDisplay(itemId, deckEditCard.card);
                yield return DisplayLV(cardMaster.CardList.Find(m => m.itemId == deckEditCard.cardId).level, deckEditCard.gameObject.transform.GetChild(2).gameObject.GetComponent<Image>());
            }
            if (asyncResult.Result.Item.Slots[i].Name == "card17")
            {

                string itemId = GetBetweenStrings("item:", ":itemSet", asyncResult.Result.Item.Slots[i].PropertyId);
                DeckEditCard deckEditCard = deckEditCards[16];
                deckEditCard.cardId = itemId;
                yield return deckEditCard.display.CardDisplay(itemId, deckEditCard.card);
                yield return DisplayLV(cardMaster.CardList.Find(m => m.itemId == deckEditCard.cardId).level, deckEditCard.gameObject.transform.GetChild(2).gameObject.GetComponent<Image>());
            }
            if (asyncResult.Result.Item.Slots[i].Name == "card18")
            {

                string itemId = GetBetweenStrings("item:", ":itemSet", asyncResult.Result.Item.Slots[i].PropertyId);
                DeckEditCard deckEditCard = deckEditCards[17];
                deckEditCard.cardId = itemId;
                yield return deckEditCard.display.CardDisplay(itemId, deckEditCard.card);
                yield return DisplayLV(cardMaster.CardList.Find(m => m.itemId == deckEditCard.cardId).level, deckEditCard.gameObject.transform.GetChild(2).gameObject.GetComponent<Image>());
            }
            if (asyncResult.Result.Item.Slots[i].Name == "card19")
            {

                string itemId = GetBetweenStrings("item:", ":itemSet", asyncResult.Result.Item.Slots[i].PropertyId);
                DeckEditCard deckEditCard = deckEditCards[18];
                deckEditCard.cardId = itemId;
                yield return deckEditCard.display.CardDisplay(itemId, deckEditCard.card);
                yield return DisplayLV(cardMaster.CardList.Find(m => m.itemId == deckEditCard.cardId).level, deckEditCard.gameObject.transform.GetChild(2).gameObject.GetComponent<Image>());
            }
            if (asyncResult.Result.Item.Slots[i].Name == "card20")
            {

                string itemId = GetBetweenStrings("item:", ":itemSet", asyncResult.Result.Item.Slots[i].PropertyId);
                DeckEditCard deckEditCard = deckEditCards[19];
                deckEditCard.cardId = itemId;
                yield return deckEditCard.display.CardDisplay(itemId, deckEditCard.card);
                yield return DisplayLV(cardMaster.CardList.Find(m => m.itemId == deckEditCard.cardId).level, deckEditCard.gameObject.transform.GetChild(2).gameObject.GetComponent<Image>());
            }
            if (asyncResult.Result.Item.Slots[i].Name == "card21")
            {

                string itemId = GetBetweenStrings("item:", ":itemSet", asyncResult.Result.Item.Slots[i].PropertyId);
                DeckEditCard deckEditCard = deckEditCards[20];
                deckEditCard.cardId = itemId;
                yield return deckEditCard.display.CardDisplay(itemId, deckEditCard.card);
                yield return DisplayLV(cardMaster.CardList.Find(m => m.itemId == deckEditCard.cardId).level, deckEditCard.gameObject.transform.GetChild(2).gameObject.GetComponent<Image>());
            }
            if (asyncResult.Result.Item.Slots[i].Name == "card22")
            {

                string itemId = GetBetweenStrings("item:", ":itemSet", asyncResult.Result.Item.Slots[i].PropertyId);
                DeckEditCard deckEditCard = deckEditCards[21];
                deckEditCard.cardId = itemId;
                yield return deckEditCard.display.CardDisplay(itemId, deckEditCard.card);
                yield return DisplayLV(cardMaster.CardList.Find(m => m.itemId == deckEditCard.cardId).level, deckEditCard.gameObject.transform.GetChild(2).gameObject.GetComponent<Image>());
            }
            if (asyncResult.Result.Item.Slots[i].Name == "card23")
            {

                string itemId = GetBetweenStrings("item:", ":itemSet", asyncResult.Result.Item.Slots[i].PropertyId);
                DeckEditCard deckEditCard = deckEditCards[22];
                deckEditCard.cardId = itemId;
                yield return deckEditCard.display.CardDisplay(itemId, deckEditCard.card);
                yield return DisplayLV(cardMaster.CardList.Find(m => m.itemId == deckEditCard.cardId).level, deckEditCard.gameObject.transform.GetChild(2).gameObject.GetComponent<Image>());
            }
            if (asyncResult.Result.Item.Slots[i].Name == "card24")
            {

                string itemId = GetBetweenStrings("item:", ":itemSet", asyncResult.Result.Item.Slots[i].PropertyId);
                DeckEditCard deckEditCard = deckEditCards[23];
                deckEditCard.cardId = itemId;
                yield return deckEditCard.display.CardDisplay(itemId, deckEditCard.card);
                yield return DisplayLV(cardMaster.CardList.Find(m => m.itemId == deckEditCard.cardId).level, deckEditCard.gameObject.transform.GetChild(2).gameObject.GetComponent<Image>());
            }
            if (asyncResult.Result.Item.Slots[i].Name == "card25")
            {

                string itemId = GetBetweenStrings("item:", ":itemSet", asyncResult.Result.Item.Slots[i].PropertyId);
                DeckEditCard deckEditCard = deckEditCards[24];
                deckEditCard.cardId = itemId;
                yield return deckEditCard.display.CardDisplay(itemId, deckEditCard.card);
                yield return DisplayLV(cardMaster.CardList.Find(m => m.itemId == deckEditCard.cardId).level, deckEditCard.gameObject.transform.GetChild(2).gameObject.GetComponent<Image>());
            }
            if (asyncResult.Result.Item.Slots[i].Name == "shield1")
            {
                string itemId = GetBetweenStrings("item:", ":itemSet", asyncResult.Result.Item.Slots[i].PropertyId);
                DeckEditShield deckEditShield = deckEditShields[0];
                deckEditShield.shieldId = itemId;
                deckEditShield.life.text = shieldMaster.ShieldList.Find(m => m.itemId == itemId).life.ToString();
                yield return deckEditShield.display.ShieldDisplay(itemId, deckEditShield.card);
            }
            if (asyncResult.Result.Item.Slots[i].Name == "shield2")
            {
                string itemId = GetBetweenStrings("item:", ":itemSet", asyncResult.Result.Item.Slots[i].PropertyId);
                DeckEditShield deckEditShield = deckEditShields[1];
                deckEditShield.shieldId = itemId;
                deckEditShield.life.text = shieldMaster.ShieldList.Find(m => m.itemId == itemId).life.ToString();
                yield return deckEditShield.display.ShieldDisplay(itemId, deckEditShield.card);
            }
            if (asyncResult.Result.Item.Slots[i].Name == "shield3")
            {
                string itemId = GetBetweenStrings("item:", ":itemSet", asyncResult.Result.Item.Slots[i].PropertyId);
                DeckEditShield deckEditShield = deckEditShields[2];
                deckEditShield.shieldId = itemId;
                deckEditShield.life.text = shieldMaster.ShieldList.Find(m => m.itemId == itemId).life.ToString();
                yield return deckEditShield.display.ShieldDisplay(itemId, deckEditShield.card);
            }
            if (asyncResult.Result.Item.Slots[i].Name == "shield4")
            {
                string itemId = GetBetweenStrings("item:", ":itemSet", asyncResult.Result.Item.Slots[i].PropertyId);
                DeckEditShield deckEditShield = deckEditShields[3];
                deckEditShield.shieldId = itemId;
                deckEditShield.life.text = shieldMaster.ShieldList.Find(m => m.itemId == itemId).life.ToString();
                yield return deckEditShield.display.ShieldDisplay(itemId, deckEditShield.card);
            }
            if (asyncResult.Result.Item.Slots[i].Name == "shield5")
            {
                string itemId = GetBetweenStrings("item:", ":itemSet", asyncResult.Result.Item.Slots[i].PropertyId);
                DeckEditShield deckEditShield = deckEditShields[4];
                deckEditShield.shieldId = itemId;
                deckEditShield.life.text = shieldMaster.ShieldList.Find(m => m.itemId == itemId).life.ToString();
                yield return deckEditShield.display.ShieldDisplay(itemId, deckEditShield.card);
            }
            if (asyncResult.Result.Item.Slots[i].Name == "special")
            {
                string itemId = GetBetweenStrings("item:", ":itemSet", asyncResult.Result.Item.Slots[i].PropertyId);
                Special special = specialMaster.SpecialList.Find(m => m.itemId == itemId);
                deckEditSpecial.specialId = special.id;
                yield return deckEditSpecial.display.SpecialDisplay(special, deckEditSpecial.card);
            }
        }
        yield return new WaitForSeconds(2);
        GameObject.Find("Loading").GetComponent<Loading>().LoadingEnd();
    }
    private IEnumerator DeckName()
    {
        AsyncResult<EzPrepareDownloadOwnDataResult> asyncResult = null;
        var current = gs2.Datastore.PrepareDownloadOwnData(
              r => { asyncResult = r; },
              session: session,
              namespaceName: "datastore",
              dataObjectName: "deck" + PlayerPrefs.GetInt("deckNum").ToString()
        );
        yield return current;
        if (asyncResult.Error != null)
        {
            OnError(asyncResult.Error);
            inputField.text = "デッキ名";
            yield break;
        }
        UnityWebRequest www = UnityWebRequest.Get(asyncResult.Result.FileUrl);
        yield return www.SendWebRequest();

        if (www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.Log(www.error);
        }
        else
        {
            inputField.text = www.downloadHandler.text;
        }
    }
    public string GetBetweenStrings(string str1, string str2, string orgStr)
    {
        int str1Len = str1.Length; //str1の長さ

        int str1Num = orgStr.IndexOf(str1); //str1が原文のどの位置にあるか

        string s = ""; //返す文字列

        //例外処理
        try
        {
            s = orgStr.Remove(0, str1Num + str1Len); //原文の初めからstr1のある位置まで削除
            int str2Num = s.IndexOf(str2); //str2がsのどの位置にあるか
            s = s.Remove(str2Num); //sのstr2のある位置から最後まで削除
        }
        catch (Exception)
        {
            return orgStr; //原文を返す
        }

        return s; //戻り値
    }
    private IEnumerator DisplayLV(int lv, Image image)
    {
        var unit = Addressables.LoadAssetAsync<Sprite>("サンセリフホワイト48_" + lv);
        yield return unit;
        image.sprite = unit.Result;
    }


    [System.Runtime.Serialization.DataMember()]
    public string[] cards { get; set; } = new string[31];

    public string ToJson(string[] cards)
    {
        DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(string[]));
        MemoryStream ms = new MemoryStream();
        serializer.WriteObject(ms, cards);
        return Encoding.UTF8.GetString(ms.ToArray());
    }
    public string[] ToClass(string str)
    {
        DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(string[]));
        byte[] bytes = Encoding.UTF8.GetBytes(str);
        MemoryStream ms = new MemoryStream(bytes);
        return (string[])serializer.ReadObject(ms);
    }

    public void CodeWindowOpen()
    {
        codeWindow.SetActive(true);
    }
    public void CodeWindowClose()
    {
        codeWindow.SetActive(false);
    }
    public void CodeWrite()
    {
        int x = 0;

        for (int i = 0; i < deckEditCards.Length; i++)
        {
            cards[x] = deckEditCards[i].cardId;
            x++;
        }
        for (int i = 0; i < deckEditShields.Length; i++)
        {
            cards[x] = deckEditShields[i].shieldId;
            x++;

        }
        Special special = specialMaster.SpecialList.Find(m => m.id == deckEditSpecial.specialId);
        if (special != null) cards[x] = special.itemId;
        else cards[x] = "";
        string json = ToJson(cards);
        deckCode.text = json;
    }
    public void CodeRead()
    {
        StartCoroutine(CodeRoad2());
    }
    private IEnumerator CodeRoad2()
    {
        cards = ToClass(deckCode.text);
        for (int i = 0; i < cards.Length; i++)
        {
            if (cards[i] != "")
            {
                if (i < 25)
                {
                    StartCoroutine(CardCode(i));
                }
                else if (i < 30)
                {
                    StartCoroutine(ShieldCode(i));
                }
                else if (i == 30)
                {

                    bool cardHave = true;
                    yield return CardCheak("special", "special", cards[i], (result) => cardHave = result);
                    if (cardHave)
                    {
                        Special special = specialMaster.SpecialList.Find(m => m.itemId == cards[i]);
                        deckEditSpecial.specialId = special.id;
                        yield return deckEditSpecial.display.SpecialDisplay(special, deckEditSpecial.card);
                    }
                }
            }
        }
    }
    private IEnumerator CardCode(int i)
    {
        Card card = cardMaster.CardList.Find(m => m.itemId == cards[i]);
        bool cardHave = true;
        yield return CardCheak(card.inventory, card.inventory, card.itemId, (result) => cardHave = result);
        if (cardHave)
        {
            DeckEditCard deckEditCard = deckEditCards[i];
            deckEditCard.cardId = cards[i];
            yield return deckEditCard.display.CardDisplay(cards[i], deckEditCard.card);
            yield return DisplayLV(cardMaster.CardList.Find(m => m.itemId == deckEditCard.cardId).level, deckEditCard.gameObject.transform.GetChild(2).gameObject.GetComponent<Image>());
        }
    }
    private IEnumerator ShieldCode(int i)
    {
        Shield shield = shieldMaster.ShieldList.Find(m => m.itemId == cards[i]);
        bool cardHave = true;
        yield return CardCheak(shield.inventory, shield.inventory, shield.itemId, (result) => cardHave = result);
        if (cardHave)
        {
            DeckEditShield deckEditShield = deckEditShields[i - 25];
            deckEditShield.shieldId = cards[i];
            deckEditShield.life.text = shieldMaster.ShieldList.Find(m => m.itemId == cards[i]).life.ToString();
            yield return deckEditShield.display.ShieldDisplay(cards[i], deckEditShield.card);
        }
    }
}
