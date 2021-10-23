using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BattleResult : MonoBehaviour
{
    public GameObject win;
    public GameObject lose;
    public Text rp;
    public Text add;
    int rpPoint;
    int addPoint;
    public Image rankImage;
    public Text rankText;
    public GameObject up;
    public GameObject down;
    void Start()
    {
        StartCoroutine(Rate());
        if (PlayerPrefs.GetString("BattleResult") == "win") win.SetActive(true);
        if (PlayerPrefs.GetString("BattleResult") == "lose") lose.SetActive(true);
    }
    private IEnumerator Rate()
    {
        rpPoint = PlayerPrefs.GetInt("MatchRate");
        addPoint = PlayerPrefs.GetInt("AddRate");

        int rate = rpPoint;
        string rank = "";

        if (rate >= 1000)
        {
            var color = Addressables.LoadAssetAsync<Sprite>("奥義ストック黒");
            yield return color;
            rankImage.sprite = color.Result;
            rankText.text = "Ⅰ";
            rank = "43";
        }
        if (rate <= 599)
        {
            var color = Addressables.LoadAssetAsync<Sprite>("奥義ストック青");
            yield return color;
            rankImage.sprite = color.Result;
            rankText.text = "Ⅲ";
            rank = "11";
        }
        if (600 <= rate && rate <= 999)
        {
            if (600 <= rate && rate <= 699)
            {
                var color = Addressables.LoadAssetAsync<Sprite>("奥義ストック青");
                yield return color;
                rankImage.sprite = color.Result;
                rank = "1";
            }
            if (700 <= rate && rate <= 799)
            {
                var color = Addressables.LoadAssetAsync<Sprite>("奥義ストック黄");
                yield return color;
                rankImage.sprite = color.Result;
                rank = "2";
            }
            if (800 <= rate && rate <= 899)
            {
                var color = Addressables.LoadAssetAsync<Sprite>("奥義ストック赤");
                yield return color;
                rankImage.sprite = color.Result;
                rank = "3";
            }
            if (900 <= rate && rate <= 999)
            {
                var color = Addressables.LoadAssetAsync<Sprite>("奥義ストック黒");
                yield return color;
                rankImage.sprite = color.Result;
                rank = "4";
            }
            int l = rate % 100;
            if (0 <= l && l <= 33)
            {
                rankText.text = "Ⅲ";
                rank = rank + "1";
            }
            if (34 <= l && l <= 66)
            {
                rankText.text = "Ⅱ";
                rank = rank + "2";
            }
            if (67 <= l && l <= 99)
            {
                rankText.text = "Ⅰ";
                rank = rank + "3";
            }


            rp.text = rpPoint.ToString();
            add.text = "(" + addPoint + ")";
            yield return new WaitForSeconds(1.5f);
            while (addPoint > 0)
            {
                rpPoint++;
                addPoint--;
                rp.text = rpPoint.ToString();
                add.text = "(+" + addPoint + ")";
                yield return new WaitForSeconds(0.1f);
            }
            while (addPoint < 0)
            {
                rpPoint--;
                addPoint++;
                rp.text = rpPoint.ToString();
                add.text = "(" + addPoint + ")";
                yield return new WaitForSeconds(0.1f);
            }
        }

        rate = rpPoint;

        if (rate >= 1000)
        {
            var color = Addressables.LoadAssetAsync<Sprite>("奥義ストック黒");
            yield return color;
            rankImage.sprite = color.Result;
            rankText.text = "Ⅰ";
            rank = "黒1";
        }
        if (rate <= 599)
        {
            var color = Addressables.LoadAssetAsync<Sprite>("奥義ストック青");
            yield return color;
            rankImage.sprite = color.Result;
            rankText.text = "Ⅲ";
            rank = "青3";
        }
        if (600 <= rate && rate <= 999)
        {
            int l = rate % 100;
            if (600 <= rate && rate <= 699)
            {
                var color = Addressables.LoadAssetAsync<Sprite>("奥義ストック青");
                yield return color;
                rankImage.sprite = color.Result;
                if (0 <= l && l <= 33)
                {
                    rankText.text = "Ⅲ";
                    if (int.Parse(rank) > 11) down.SetActive(true);
                }
                if (34 <= l && l <= 66)
                {
                    rankText.text = "Ⅱ";
                    if (int.Parse(rank) > 12) down.SetActive(true);
                    if (int.Parse(rank) < 12) up.SetActive(true);
                }
                if (67 <= l && l <= 99)
                {
                    rankText.text = "Ⅰ";
                    if (int.Parse(rank) > 13) down.SetActive(true);
                    if (int.Parse(rank) < 13) up.SetActive(true);
                }
            }
            if (700 <= rate && rate <= 799)
            {
                var color = Addressables.LoadAssetAsync<Sprite>("奥義ストック黄");
                yield return color;
                rankImage.sprite = color.Result;
                if (0 <= l && l <= 33)
                {
                    rankText.text = "Ⅲ";
                    if (int.Parse(rank) > 21) down.SetActive(true);
                    if (int.Parse(rank) < 21) up.SetActive(true);
                }
                if (34 <= l && l <= 66)
                {
                    rankText.text = "Ⅱ";
                    if (int.Parse(rank) > 22) down.SetActive(true);
                    if (int.Parse(rank) < 22) up.SetActive(true);
                }
                if (67 <= l && l <= 99)
                {
                    rankText.text = "Ⅰ";
                    if (int.Parse(rank) > 23) down.SetActive(true);
                    if (int.Parse(rank) < 23) up.SetActive(true);
                }
            }
            if (800 <= rate && rate <= 899)
            {
                var color = Addressables.LoadAssetAsync<Sprite>("奥義ストック赤");
                yield return color;
                rankImage.sprite = color.Result;
                if (0 <= l && l <= 33)
                {
                    rankText.text = "Ⅲ";
                    if (int.Parse(rank) > 31) down.SetActive(true);
                    if (int.Parse(rank) < 31) up.SetActive(true);
                }
                if (34 <= l && l <= 66)
                {
                    rankText.text = "Ⅱ";
                    if (int.Parse(rank) > 32) down.SetActive(true);
                    if (int.Parse(rank) < 32) up.SetActive(true);
                }
                if (67 <= l && l <= 99)
                {
                    rankText.text = "Ⅰ";
                    if (int.Parse(rank) > 33) down.SetActive(true);
                    if (int.Parse(rank) < 33) up.SetActive(true);
                }
            }
            if (900 <= rate && rate <= 999)
            {
                var color = Addressables.LoadAssetAsync<Sprite>("奥義ストック黒");
                yield return color;
                rankImage.sprite = color.Result;
                if (0 <= l && l <= 33)
                {
                    rankText.text = "Ⅲ";
                    if (int.Parse(rank) > 41) down.SetActive(true);
                    if (int.Parse(rank) < 41) up.SetActive(true);
                }
                if (34 <= l && l <= 66)
                {
                    rankText.text = "Ⅱ";
                    if (int.Parse(rank) > 42) down.SetActive(true);
                    if (int.Parse(rank) < 42) up.SetActive(true);
                }
                if (67 <= l && l <= 99)
                {
                    rankText.text = "Ⅰ";
                    if (int.Parse(rank) < 43) up.SetActive(true);
                }
            }
        }
    }
    public void HomeButton()
    {
        PlayerPrefs.SetString("menu", "home");
        SceneManager.LoadScene("MainMenu");
    }
}
