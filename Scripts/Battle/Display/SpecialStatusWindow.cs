using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;

public class SpecialStatusWindow : MonoBehaviour
{
    public Text specialName;
    public Text stk;
    public Text color;
    public Text detail;
    public Image card;
    public GameObject flavor;
    public void SpecialStatusWindowClose()
    {
        GameObject.Find("AudioTapEffect").GetComponent<AudioController>().TapEffect(0);
        this.gameObject.SetActive(false);
    }
    public void SpecialFlavor()
    {
        GameObject.Find("AudioTapEffect").GetComponent<AudioController>().TapEffect(0);
        this.gameObject.SetActive(false);
        flavor.SetActive(true);
    }
    public IEnumerator OpenSpecialStatusWindow(Special special,Display display)
    {
        specialName.text = special.name;
        color.text = special.color;
        stk.text = special.stock.ToString();
        detail.text = special.skillDetail;
        yield return display.SpecialDisplay(special, card);
        this.gameObject.SetActive(true);

        SpecialFlavorWindow specialFlavorWindow = flavor.GetComponent<SpecialFlavorWindow>();
        /*
        var unit = Addressables.LoadAssetAsync<Sprite>("枠LE");
        yield return unit;
        specialFlavorWindow.frame.sprite = unit.Result;
        */
        specialFlavorWindow.specialName.text = special.name;
        specialFlavorWindow.flavor.text = special.flavor;
        specialFlavorWindow.illust.text = special.illust;
        yield return display.SpecialDisplay(special, specialFlavorWindow.card);
    }
}
