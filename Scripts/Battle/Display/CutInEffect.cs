using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutInEffect : MonoBehaviour
{
    private SimpleAnimation anime;
    public AnimationClip[] animationClips;
    void Start()
    {
        anime = GetComponent<SimpleAnimation>();
    }

    public IEnumerator StartCutInEffect(string effect)
    {
        int i;
        for (i = 0; i < animationClips.Length; i++)
        {
            if (effect == animationClips[i].name) break;
        }
        GameObject.Find("AudioAnimeEffect").GetComponent<AudioController>().CutinEffect(i);
        anime.AddClip(animationClips[i], animationClips[i].name);
        anime.Play(animationClips[i].name);
        yield return new WaitForSeconds(1.5f);
        anime.Stop();
    }
    /*
    public IEnumerator StartCutInEffect(string effect)
    {
        anime.Stop();
        bool flag = true;
        switch (effect)
        {
            case "白大魔法":
                GameObject.Find("AudioAnimeEffect").GetComponent<AudioController>().CutinEffect(0);
                break;
            case "赤大魔法":
                GameObject.Find("AudioAnimeEffect").GetComponent<AudioController>().CutinEffect(1);
                break;
            default:
                flag = false;
                break;

        }
        if (flag)
        {
            anime.Play(effect);
            yield return new WaitForSeconds(1.5f);
            anime.Stop();
        }
    }
    */
}
