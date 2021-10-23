using BattleFolder;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class ShieldCard : MonoBehaviour
{
    public int cardNumber;
    public GameObject battleManager;
    BattleStatus battleStatus;
    BattleStatus battleStatusEnemy;
    Display display;


    public void ShieldCardDown()
    {
        GameObject.Find("AudioTapEffect").GetComponent<AudioController>().TapEffect(0);
        battleStatus = battleManager.GetComponent<Battle>().battleStatus;
        battleStatusEnemy = battleManager.GetComponent<Battle>().battleStatusEnemy;
        display = battleManager.GetComponent<Display>();
        ShieldStatusWindow shieldStatusWindow = display.shieldStatusWindow.GetComponent<ShieldStatusWindow>();
        Shield shield;
        if (cardNumber < 5)
        {
            shield = battleManager.GetComponent<Battle>().shieldMaster.ShieldList.Find(m => m.itemId == battleStatus.shieldStatus[cardNumber].shieldId);
        }
        else
        {
            shield = battleManager.GetComponent<Battle>().shieldMaster.ShieldList.Find(m => m.itemId == battleStatusEnemy.shieldStatus[cardNumber-5].shieldId);
        }
        StartCoroutine(shieldStatusWindow.ShieldStatusWindowOpen(shield, display));
    }
}
