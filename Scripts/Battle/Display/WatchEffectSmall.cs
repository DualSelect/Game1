using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WatchEffectSmall : MonoBehaviour
{
    private int frameCnt = 0; // フレームカウント
    void FixedUpdate()
    {
        frameCnt += 1;
        if (10000 <= frameCnt)
        {
            frameCnt = 0;
        }
        if (0 == frameCnt % 2)
        {
            iTween.RotateAdd(gameObject, iTween.Hash("z", -0.02f, "time", 0f));
        }
    }
}
