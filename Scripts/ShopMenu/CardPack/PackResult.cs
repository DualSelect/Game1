using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PackResult : MonoBehaviour
{
    public Text nowNum;
    public Text maxNum;
    int num = 0;
    int packNum;
    public GameObject resultWindow;
    public Text[] resultText;
    public Text[] resultRare;
    public CardMaster cardMaster;
    public ShieldMaster shieldMaster;
    public GameObject cardDetailWindow;
    public GameObject shieldDetailWindow;
    public Image image;
    public Display display;
    public GameObject mask;
    public GameObject tap;
    public Button detail;
    public Button next;
    public Image frame;
    void Start()
    {
        GameObject.Find("AudioBGM").GetComponent<AudioController>().BGMStop();
        GameObject.Find("AudioTapEffect").GetComponent<AudioController>().TapEffect(1);
        packNum = PlayerPrefs.GetInt("PackNum");
        maxNum.text = "/" + packNum;
        StartCoroutine(ChangeCard());

        for(int i = 0; i < packNum; i++)
        {
            string itemId = PlayerPrefs.GetString("Pack" + (i+1));
            Card card = cardMaster.CardList.Find(m => m.itemId == itemId);
            Shield shield = shieldMaster.ShieldList.Find(m => m.itemId == itemId);
            if (card != null)
            {
                resultText[i].text = card.name;
                resultRare[i].text = card.rare;
            }
            if (shield != null)
            {
                resultText[i].text = shield.name;
                resultRare[i].text = shield.rare;
            }
        }
        GameObject.Find("Loading").GetComponent<Loading>().LoadingEnd();
    }
    private IEnumerator ChangeCard()
    {
        num++;
        nowNum.text = num.ToString();
        detail.interactable = false;
        tap.SetActive(true);
        mask.gameObject.GetComponent<Transition>().ResetAnime();
        string itemId = PlayerPrefs.GetString("Pack" + num);
        Card card = cardMaster.CardList.Find(m => m.itemId == itemId);
        Shield shield = shieldMaster.ShieldList.Find(m => m.itemId == itemId);
        if (card != null)
        {
            var unit = Addressables.LoadAssetAsync<Sprite>(card.id);
            yield return unit;
            image.sprite = unit.Result;
            var fr = Addressables.LoadAssetAsync<Sprite>("ガチャ枠" + card.rare);
            yield return fr;
            frame.sprite = fr.Result;
        }
        if (shield != null)
        {
            var unit = Addressables.LoadAssetAsync<Sprite>(shield.id);
            yield return unit;
            image.sprite = unit.Result;
            var fr = Addressables.LoadAssetAsync<Sprite>("ガチャ枠" + shield.rare);
            yield return fr;
            frame.sprite = fr.Result;
        }
    }
    public void TapDraw()
    {
        next.interactable = false;
        GameObject.Find("AudioAnimeEffect").GetComponent<AudioController>().AnimeEffect(1);
        tap.SetActive(false);
        detail.interactable = true;
        StartCoroutine(mask.gameObject.GetComponent<Transition>().BeginTransition());
        StartCoroutine(TapDraw2());

    }
    public IEnumerator TapDraw2()
    {
        yield return new WaitForSeconds(1.5f);
        next.interactable = true;
    }
    public void Next()
    {
        GameObject.Find("AudioTapEffect").GetComponent<AudioController>().TapEffect(0);
        if (num != packNum)
        {
            StartCoroutine(ChangeCard());
        }
        else
        {
            Skip();
        }
    }
    public void Detail()
    {
        GameObject.Find("AudioTapEffect").GetComponent<AudioController>().TapEffect(0);
        string itemId = PlayerPrefs.GetString("Pack" + num);
        Card card = cardMaster.CardList.Find(m => m.itemId == itemId);
        Shield shield = shieldMaster.ShieldList.Find(m => m.itemId == itemId);
        if (card != null)
        {
            UnitStatusWindow unitStatusWindow = cardDetailWindow.GetComponent<UnitStatusWindow>();
            StartCoroutine(unitStatusWindow.UnitStatusWindowOpen(card, display));
        }
        if (shield != null)
        {
            ShieldStatusWindow shieldStatusWindow = shieldDetailWindow.GetComponent<ShieldStatusWindow>();
            StartCoroutine(shieldStatusWindow.ShieldStatusWindowOpen(shield, display));
        }
    }
    public void Skip()
    {
        GameObject.Find("AudioTapEffect").GetComponent<AudioController>().TapEffect(0);
        resultWindow.SetActive(true);
    }
    public void Complete()
    {
        GameObject.Find("AudioTapEffect").GetComponent<AudioController>().TapEffect(0);
        GameObject.Find("AudioBGM").GetComponent<AudioController>().BGMStart();
        SceneManager.LoadScene("CardPack");
    }
}
