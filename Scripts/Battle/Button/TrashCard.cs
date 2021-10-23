using BattleFolder;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrashCard : MonoBehaviour
{
    public int cardNumber;
    public GameObject battleManager;
    BattleStatus battleStatus;
    BattleStatus battleStatusEnemy;
    Display display;
    public bool vanish;
    public bool enemy;
    void Start()
    {
        battleStatus = battleManager.GetComponent<Battle>().battleStatus;
        battleStatusEnemy = battleManager.GetComponent<Battle>().battleStatusEnemy;
        display = battleManager.GetComponent<Display>();
    }
    public void TrashCardClick()
    {
        StartCoroutine(TrashCardDisplay());
    }
    public IEnumerator TrashCardDisplay()
    {
        GameObject.Find("AudioTapEffect").GetComponent<AudioController>().TapEffect(0);
        UnitStatusWindow unitStatusWindow = display.unitStatusWindow.GetComponent<UnitStatusWindow>();
        Card card = new Card();
        if (!enemy)
        {
            if (!vanish)
            {
                card = battleManager.GetComponent<Battle>().cardMaster.CardList.Find(m => m.itemId == battleStatus.trash[0,cardNumber]);
            }
            else
            {
                card = battleManager.GetComponent<Battle>().cardMaster.CardList.Find(m => m.itemId == battleStatus.trash[1, cardNumber]);
            }
        }
        else
        {
            if (!vanish)
            {
                card = battleManager.GetComponent<Battle>().cardMaster.CardList.Find(m => m.itemId == battleStatusEnemy.trash[0, cardNumber]);
            }
            else
            {
                card = battleManager.GetComponent<Battle>().cardMaster.CardList.Find(m => m.itemId == battleStatusEnemy.trash[1, cardNumber]);
            }
        }
        yield return unitStatusWindow.UnitStatusWindowOpen(card, display);
    }
}
