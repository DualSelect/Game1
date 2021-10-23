using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BattleFolder;

public class AreaEffect : MonoBehaviour
{
    public AnimationClip[] animationClips;
    public SimpleAnimation setAreaEffect;
    public SimpleAnimation setAreaEnemyEffect;
    public SimpleAnimation[] battleAreaEffect;
    public SimpleAnimation[] battleAreaEnemyEffect;
    public SimpleAnimation shieldEffect;

    public IEnumerator SetAreaEffect(bool enemy,string effect)
    {
        SimpleAnimation simpleAnimation;
        if (!enemy)
        {
            simpleAnimation = setAreaEffect;
        }
        else
        {
            simpleAnimation = setAreaEnemyEffect;
        }
        int i;
        for (i = 0; i < animationClips.Length; i++)
        {
            if (effect == animationClips[i].name) break;
        }
        GameObject.Find("AudioAnimeEffect").GetComponent<AudioController>().BattleEffect(i);
        simpleAnimation.AddClip(animationClips[i], animationClips[i].name);
        simpleAnimation.Play(animationClips[i].name);
        yield return new WaitForSeconds(1.0f);
        simpleAnimation.Stop();
    }
    /*
    public IEnumerator BattleAreaEffect(int[] playerArea,int[] enemyArea,string effect)
    {
        int i;
        SimpleAnimation[] playerEffect = new SimpleAnimation[playerArea.Length];
        SimpleAnimation[] enemyEffect = new SimpleAnimation[enemyArea.Length];
        for (i = 0; i < titles.Length; i++)
        {
            if (effect == titles[i]) break;
        }
        if (i == titles.Length) Debug.Log(effect);
        for (int j = 0; j < playerArea.Length; j++)
        {
            playerEffect[j] = battleAreaEffect[playerArea[j]];
            playerEffect[j].AddClip(animationClips[i], titles[i]);
            playerEffect[j].Play(titles[i]);
        }
        for (int j = 0; j < enemyArea.Length; j++)
        {
            enemyEffect[j] = battleAreaEnemyEffect[enemyArea[j]];
            enemyEffect[j].AddClip(animationClips[i], titles[i]);
            enemyEffect[j].Play(titles[i]);
        }
        yield return new WaitForSeconds(1.5f);
        for (int j = 0; j < playerArea.Length; j++)
        {
            playerEffect[j].Stop();
        }
        for (int j = 0; j < enemyArea.Length; j++)
        {
            enemyEffect[j].Stop();
        }
    }
    */
    public IEnumerator BattleAreaEffect(bool enemy,int area, string effect)
    {
        Debug.Log(effect);
        SimpleAnimation simpleAnimation;
        if (!enemy)
        {
            simpleAnimation = battleAreaEffect[area];
        }
        else
        {
            simpleAnimation = battleAreaEnemyEffect[area];
        }
        int i;
        for (i = 0; i < animationClips.Length; i++)
        {
            if (effect == animationClips[i].name) break;
        }
        GameObject.Find("AudioAnimeEffect").GetComponent<AudioController>().BattleEffect(i);
        simpleAnimation.AddClip(animationClips[i], animationClips[i].name);
        simpleAnimation.Play(animationClips[i].name);
        yield return new WaitForSeconds(1.0f);
        simpleAnimation.Stop();
    }
    public IEnumerator ShieldAreaEffect(bool enemy, int area, Card card)
    {
        BattleJson battleJson = new BattleJson();
        string effect = battleJson.toEffect(card.attackEffect).enemy;
        Debug.Log(effect);
        SimpleAnimation simpleAnimation = shieldEffect;
        if (!enemy)
        {
            shieldEffect.gameObject.transform.position = new Vector3(2.3f, 0.8f + 0.6f * area, 0);
        }
        else
        {
            shieldEffect.gameObject.transform.position = new Vector3(2.3f, -0.7f - 0.6f * area, 0);
        }
        int i;
        for (i = 0; i < animationClips.Length; i++)
        {
            if (effect == animationClips[i].name) break;
        }
        GameObject.Find("AudioAnimeEffect").GetComponent<AudioController>().BattleEffect(i);
        simpleAnimation.AddClip(animationClips[i], animationClips[i].name);
        simpleAnimation.Play(animationClips[i].name);
        yield return new WaitForSeconds(1.0f);
        simpleAnimation.Stop();
    }
}
