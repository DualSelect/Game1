using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// マスタ管理するデータの一単位。
/// 変数は必ずスプレッドシートのマスタと同じにする
/// </summary>
[System.Serializable]
public class Shield
{
    public string id;
    public string pack;
    public string name;
    public string rare;
    public int life;
    public int stock;
    public int skillSp;
    public string skillType;
    public string skillDetail;
    public string effect;
    public string background;
    public string flavor;
    public string inventory;
    public string itemId;
    public string illust;
}
