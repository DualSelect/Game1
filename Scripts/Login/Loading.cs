using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;

public class Loading : MonoBehaviour
{
    public GameObject load;
    public new Text name;
    public Text flavor;
    public Text illust;
    public Image image;
    public CardMaster cardMaster;
    public ShieldMaster shieldMaster;
    void Start()
    {
        DontDestroyOnLoad(this);
    }
    public IEnumerator LoadingRamdom() {
        System.Random r = new System.Random();
        int number = r.Next(0, 4);
        if (number <= 2)
        {
            number = r.Next(0, cardMaster.CardList.Capacity);
            Card card = cardMaster.CardList[number];
            var unit = Addressables.LoadAssetAsync<Sprite>(card.id);
            yield return unit;
            image.sprite = unit.Result;
            name.text = card.name;
            flavor.text = card.flavor;
            illust.text = card.illust;
            if (card.flavor == "") flavor.text = "フレーバー未登録";
        }
        else
        {
            number = r.Next(0, shieldMaster.ShieldList.Capacity);
            Shield shield = shieldMaster.ShieldList[number];
            var unit = Addressables.LoadAssetAsync<Sprite>(shield.id);
            yield return unit;
            image.sprite = unit.Result;
            name.text = shield.name;
            flavor.text = shield.flavor;
            illust.text = shield.illust;
            if (shield.flavor == "") flavor.text = "フレーバー未登録";
        }
    }
    public void LoadingStart()
    {
        this.gameObject.transform.GetChild(0).gameObject.GetComponent<FadeUI>().Range = 1f;
        load.SetActive(true);
        this.gameObject.transform.GetChild(0).gameObject.GetComponent<Fade>().FadeOut(3, () => { });
    }
    public void LoadingEnd()
    {
        load.SetActive(false);
    }
}
