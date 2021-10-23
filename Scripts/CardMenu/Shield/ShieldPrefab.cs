using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldPrefab : MonoBehaviour
{
    public string shieldId;
    public GameObject shieldStatus;
    public ShieldMaster shieldMaster;
    public Display display;

    private void Start()
    {
        ShieldInitial shieldInitial = GameObject.Find("ShieldList").GetComponent<ShieldInitial>();
        shieldStatus = shieldInitial.shieldStatusWindow;
        display = shieldInitial.display;
        shieldMaster = shieldInitial.shieldMaster;
    }
    public void CardClick()
    {
        StartCoroutine(CardClicl2());
    }
    public IEnumerator CardClicl2()
    {
        GameObject.Find("AudioTapEffect").GetComponent<AudioController>().TapEffect(0);
        Shield shield = shieldMaster.ShieldList.Find(m => m.itemId == shieldId);
        ShieldStatusWindow shieldStatusWindow = shieldStatus.GetComponent<ShieldStatusWindow>();
        yield return shieldStatusWindow.ShieldStatusWindowOpen(shield, display);
    }
}
