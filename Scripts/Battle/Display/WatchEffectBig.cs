using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WatchEffectBig : MonoBehaviour
{
    private int frameCnt = 0; // フレームカウント
    void FixedUpdate()
    {
        frameCnt += 1;
        if (10000 <= frameCnt)
        {
            frameCnt = 0;
        }
        if (0 == frameCnt % 50)
        {
            iTween.RotateAdd(gameObject, iTween.Hash("z", -6f, "time", 0f));
        }
    }
}
