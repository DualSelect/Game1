using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;

public class ShieldStatusWindow : MonoBehaviour
{
    public Text shieldName;
    public Text rare;
    public Text life;
    public Text mana;
    public Text detail;
    public Image card;
    public GameObject flavor;
    public void ShieldStatusWindowClose()
    {
        GameObject.Find("AudioTapEffect").GetComponent<AudioController>().TapEffect(0);
        this.gameObject.SetActive(false);
    }
    public void ShieldFlavor()
    {
        GameObject.Find("AudioTapEffect").GetComponent<AudioController>().TapEffect(0);
        this.gameObject.SetActive(false);
        flavor.SetActive(true);
    }
    public IEnumerator ShieldStatusWindowOpen(Shield shield, Display display)
    {



        rare.text = shield.rare + "_" + shield.pack;
        shieldName.text = shield.name;
        life.text = shield.life.ToString();
        mana.text = shield.skillSp.ToString();
        detail.text = shield.skillDetail;
        yield return display.ShieldDisplay(shield, card);
        this.gameObject.SetActive(true);

        ShieldFlavorWindow shieldFlavorWindow = flavor.GetComponent<ShieldFlavorWindow>();
        /*
        var unit = Addressables.LoadAssetAsync<Sprite>("枠" + shield.rare);
        yield return unit;
        shieldFlavorWindow.frame.sprite = unit.Result;
        */
        shieldFlavorWindow.shieldName.text = shield.name;
        shieldFlavorWindow.flavor.text = shield.flavor;
        shieldFlavorWindow.illust.text = shield.illust;
        yield return display.ShieldDisplay(shield, shieldFlavorWindow.card);
    }
}