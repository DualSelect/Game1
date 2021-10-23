using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;

public class OpenDisplay : MonoBehaviour
{
    public GameObject playerCanvas;
    public GameObject enemyCanvas;
    //public GameObject playerFade;
    //public GameObject enemyFade;
    public Image playerUnit;
    public Image enemyUnit;
    //public GameObject matchFade;
    public CardMaster cardMaster;
    public ShieldMaster shieldMaster;
    public Transition mask;
    public Transition maskEnemy;
    public GameObject clush;
    public IEnumerator DoOpenDisplay(string playerUnitId,string enemyUnitId)
    {
        mask.gameObject.GetComponent<Transition>().ResetAnime();
        maskEnemy.gameObject.GetComponent<Transition>().ResetAnime();
        playerCanvas.SetActive(true);
        enemyCanvas.SetActive(true);
        var unit1 = Addressables.LoadAssetAsync<Sprite>(cardMaster.CardList.Find(m => m.itemId == playerUnitId).id);
        yield return unit1;
        playerUnit.sprite = unit1.Result;
        var unit2 = Addressables.LoadAssetAsync<Sprite>(cardMaster.CardList.Find(m => m.itemId == enemyUnitId).id);
        yield return unit2;
        enemyUnit.sprite = unit2.Result;

        GameObject.Find("AudioAnimeEffect").GetComponent<AudioController>().AnimeEffect(1);
        this.gameObject.transform.GetChild(0).gameObject.SetActive(true);
        StartCoroutine(mask.gameObject.GetComponent<Transition>().BeginTransition());
        StartCoroutine(maskEnemy.gameObject.GetComponent<Transition>().BeginTransition());
        //playerFade.GetComponent<Fade>().FadeOut(2);
        //enemyFade.GetComponent<Fade>().FadeOut(2);
        yield return new WaitForSeconds(3.0f);

        this.gameObject.transform.GetChild(0).gameObject.SetActive(false);
    }
    public IEnumerator DoOpenDisplayPlayer(string playerUnitId)
    {
        mask.gameObject.GetComponent<Transition>().ResetAnime();
        maskEnemy.gameObject.GetComponent<Transition>().ResetAnime();
        playerCanvas.SetActive(true);
        enemyCanvas.SetActive(false);
        Card card = cardMaster.CardList.Find(m => m.itemId == playerUnitId);
        Shield shield = shieldMaster.ShieldList.Find(m => m.itemId == playerUnitId);
        if (card != null)
        {
            var unit1 = Addressables.LoadAssetAsync<Sprite>(card.id);
            yield return unit1;
            playerUnit.sprite = unit1.Result;
        }
        if (shield != null)
        {
            var unit1 = Addressables.LoadAssetAsync<Sprite>(shield.id);
            yield return unit1;
            playerUnit.sprite = unit1.Result;
        }
        GameObject.Find("AudioAnimeEffect").GetComponent<AudioController>().AnimeEffect(1);
        this.gameObject.transform.GetChild(0).gameObject.SetActive(true);
        StartCoroutine(mask.gameObject.GetComponent<Transition>().BeginTransition());
        //playerFade.GetComponent<Fade>().FadeOut(2);
        yield return new WaitForSeconds(3.0f);
        this.gameObject.transform.GetChild(0).gameObject.SetActive(false);
    }
    public IEnumerator DoOpenDisplayEnemy(string enemyUnitId)
    {
        mask.gameObject.GetComponent<Transition>().ResetAnime();
        maskEnemy.gameObject.GetComponent<Transition>().ResetAnime();
        playerCanvas.SetActive(false);
        enemyCanvas.SetActive(true);
        Card card = cardMaster.CardList.Find(m => m.itemId == enemyUnitId);
        Shield shield = shieldMaster.ShieldList.Find(m => m.itemId == enemyUnitId);
        if (card != null)
        {
            var unit2 = Addressables.LoadAssetAsync<Sprite>(card.id);
            yield return unit2;
            enemyUnit.sprite = unit2.Result;
        }
        if (shield != null)
        {
            var unit2 = Addressables.LoadAssetAsync<Sprite>(shield.id);
            yield return unit2;
            enemyUnit.sprite = unit2.Result;
        }


        GameObject.Find("AudioAnimeEffect").GetComponent<AudioController>().AnimeEffect(1);
        this.gameObject.transform.GetChild(0).gameObject.SetActive(true);
        StartCoroutine(maskEnemy.gameObject.GetComponent<Transition>().BeginTransition());
        //enemyFade.GetComponent<Fade>().FadeOut(2);
        yield return new WaitForSeconds(3.0f);

        this.gameObject.transform.GetChild(0).gameObject.SetActive(false);
    }
    public IEnumerator DoOpenDisplayMatch(string playerUnitId, string enemyUnitId)
    {
        mask.gameObject.GetComponent<Transition>().ResetAnime();
        maskEnemy.gameObject.GetComponent<Transition>().ResetAnime();
        playerCanvas.SetActive(true);
        enemyCanvas.SetActive(true);
        var unit1 = Addressables.LoadAssetAsync<Sprite>(cardMaster.CardList.Find(m => m.itemId == playerUnitId).id);
        yield return unit1;
        playerUnit.sprite = unit1.Result;
        var unit2 = Addressables.LoadAssetAsync<Sprite>(cardMaster.CardList.Find(m => m.itemId == enemyUnitId).id);
        yield return unit2;
        enemyUnit.sprite = unit2.Result;

        GameObject.Find("AudioAnimeEffect").GetComponent<AudioController>().AnimeEffect(1);
        this.gameObject.transform.GetChild(0).gameObject.SetActive(true);
        StartCoroutine(mask.gameObject.GetComponent<Transition>().BeginTransition());
        StartCoroutine(maskEnemy.gameObject.GetComponent<Transition>().BeginTransition());
        //playerFade.GetComponent<Fade>().FadeOut(2);
        //enemyFade.GetComponent<Fade>().FadeOut(2);
        yield return new WaitForSeconds(1.0f);
        GameObject.Find("AudioAnimeEffect").GetComponent<AudioController>().AnimeEffect(3);
        clush.SetActive(true);
        //バッティングアニメーション
        //matchFade.SetActive(true);
        //matchFade.GetComponent<Fade>().FadeOut(1);
        yield return new WaitForSeconds(1.0f);
        //matchFade.SetActive(false);
        this.gameObject.transform.GetChild(0).gameObject.SetActive(false);
        clush.SetActive(false);
    }
}
