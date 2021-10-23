using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;

public class SkillWindow : MonoBehaviour
{
    public Text skillName;
    public Text skillDetail;
    public Image card;
    public Display display;
    public void UnitSkillWindow(Card card,string skillName,string skillDetail)
    {
        this.skillName.text = skillName;
        this.skillDetail.text = skillDetail;
        StartCoroutine(UnitSkillWindow(card));
    }
    public IEnumerator UnitSkillWindow(Card card)
    {
        yield return display.CardDisplay(card, this.card);
        GameObject.Find("AudioAnimeEffect").GetComponent<AudioController>().AnimeEffect(2);
        iTween.ScaleTo(gameObject, iTween.Hash("y", 0.1f));
        this.gameObject.transform.GetChild(0).gameObject.SetActive(true);
        iTween.ScaleTo(gameObject, iTween.Hash("y", 1f, "time", 0.5f));
        //this.gameObject.transform.GetChild(0).gameObject.GetComponent<Fade>().FadeOut(1);
    }
    public void ShieldSkillWindow(Shield shield, string skillName, string skillDetail)
    {
        StartCoroutine(ShieldSkillWindow2(shield,skillName,skillDetail));
    }
    public IEnumerator ShieldSkillWindow2(Shield shield,string skillName, string skillDetail)
    {
        this.skillName.text = skillName;
        this.skillDetail.text = skillDetail;
        var card = Addressables.LoadAssetAsync<Sprite>(shield.id);
        yield return card;
        this.card.sprite = card.Result;
        GameObject.Find("AudioAnimeEffect").GetComponent<AudioController>().AnimeEffect(2);
        iTween.ScaleTo(gameObject, iTween.Hash("y", 0.1f));
        this.gameObject.transform.GetChild(0).gameObject.SetActive(true);
        iTween.ScaleTo(gameObject, iTween.Hash("y", 1f, "time", 0.5f));
        //this.gameObject.transform.GetChild(0).gameObject.GetComponent<Fade>().FadeOut(1);
    }
    public void SpecialSkillWindow(Special special, string skillName, string skillDetail)
    {
        this.skillName.text = skillName;
        this.skillDetail.text = skillDetail;
        StartCoroutine(SpecialSkillWindow2(special));
    }
    public IEnumerator SpecialSkillWindow2(Special special)
    {
        yield return display.SpecialDisplay(special, this.card);
        GameObject.Find("AudioAnimeEffect").GetComponent<AudioController>().AnimeEffect(2);
        iTween.ScaleTo(gameObject, iTween.Hash("y", 0.1f));
        this.gameObject.transform.GetChild(0).gameObject.SetActive(true);
        iTween.ScaleTo(gameObject, iTween.Hash("y", 1f, "time", 0.5f));
        //this.gameObject.transform.GetChild(0).gameObject.GetComponent<Fade>().FadeOut(1);
    }
}
