using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MatchingWindow : MonoBehaviour
{
    public Text playerName;
    public Text playerRate;
    public Image playerIcon;
    public Text enemyName;
    public Text enemyRate;
    public Image enemyIcon;
    public GameObject playerMove;
    public GameObject enemyMove;

    public IEnumerator MatchingDisplay()
    {
        gameObject.SetActive(true);
        GameObject.Find("AudioAnimeEffect").GetComponent<AudioController>().AnimeEffect(5);
        iTween.MoveTo(playerMove, iTween.Hash("x", 750 * 0.00625f, "time",1f));
        iTween.MoveTo(enemyMove, iTween.Hash("x", 750 * 0.00625f, "time", 1f));
        yield return new WaitForSeconds(2f);
        iTween.RotateTo(gameObject, iTween.Hash("y", 90f, "time", 1f));
        yield return new WaitForSeconds(1f);
        gameObject.SetActive(false);
    }
}
