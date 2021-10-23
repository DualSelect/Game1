using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SpecialInitialEdit : MonoBehaviour
{
    public SpecialMaster specialMaster;
    public AssetReference m_Ref;
    GameObject m_Result = null;
    public GameObject specialStatusWindow;
    public GameObject specialFlavorWindow;
    public GameObject cardList;
    public GameObject shieldList;
    public DeckEditSpecial selectSpecial;
    public Display display;

    public IEnumerator FilterSearch()
    {
        var prefab = Addressables.LoadAssetAsync<GameObject>(m_Ref);
        yield return prefab;
        m_Result = prefab.Result;
        var cardlist = new List<Special>(specialMaster.SpecialList);

        GameObject content = GameObject.Find("Content");
        var transforms = content.GetComponentsInChildren<Transform>();
        var gameObjects = from t in transforms select t.gameObject;
        var cards = gameObjects.ToArray();
        for (int i = 1; i < cards.Length; i++) Destroy(cards[i]);
        Vector2 sd = new Vector2(0, 180 * ((cardlist.Capacity - 1) / 4 + 1));
        content.GetComponent<RectTransform>().sizeDelta = sd;
        for (int i = 0; i < cardlist.Count; i++)
        {
            Vector3 v = new Vector3(-250 + (i % 4) * 180, -90 - (i / 4) * 180, 0);
            GameObject gameObject = Instantiate(m_Result, v, Quaternion.identity);
            gameObject.GetComponent<SpecialPrefabEdit>().specialId = cardlist[i].id;
            gameObject.GetComponent<RectTransform>().anchorMax = new Vector2(0.5f, 1);
            gameObject.GetComponent<RectTransform>().anchorMin = new Vector2(0.5f, 1);
            gameObject.transform.SetParent(content.transform, false);
            StartCoroutine(display.SpecialDisplay(cardlist[i], gameObject.transform.GetChild(0).gameObject.GetComponent<Image>()));


        }
    }


    public void BackButtunDownEdit()
    {
        GameObject.Find("AudioTapEffect").GetComponent<AudioController>().TapEffect(0);
        this.gameObject.SetActive(false);
    }
    public void CardButtunDownEdit()
    {
        GameObject.Find("AudioTapEffect").GetComponent<AudioController>().TapEffect(0);
        this.gameObject.SetActive(false);
        cardList.SetActive(true);
        cardList.GetComponent<CardInitialEdit>().FilterSearchDown();

    }
    public void ShieldButtunDownEdit()
    {
        GameObject.Find("AudioTapEffect").GetComponent<AudioController>().TapEffect(0);
        this.gameObject.SetActive(false);
        shieldList.SetActive(true);
        shieldList.GetComponent<ShieldInitialEdit>().FilterSearchDown();
    }
}
