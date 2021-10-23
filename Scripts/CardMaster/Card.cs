using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// マスタ管理するデータの一単位。
/// 変数は必ずスプレッドシートのマスタと同じにする
/// </summary>
[System.Serializable]
public class Card {
	public string id;
    public string pack;
    public string type;
    public string name;
    public string color;
    public string rare;
    public string type1;
    public string type2;
    public int level;
    public int stock;
    public int hp;
    public int atk;
    public int dfe;
    public int agi;
    public int rng;
    public string openSkillName;
    public int openSkillSp;
    public string openSkillDetail;
    public string openEffect;
    public string startSkillName;
    public int startSkillSp;
    public string startSkillDetail;
    public string startEffect;
    public string autoSkillName;
    public int autoSkillSp;
    public string autoSkillDetail;
    public string autoEffect;
    public string actionSkillName1;
    public int actionSkillSp1;
    public string actionSkillDetail1;
    public string action1Effect;
    public string actionSkillName2;
    public int actionSkillSp2;
    public string actionSkillDetail2;
    public string action2Effect;
    public string closeSkillName;
    public int closeSkillSp;
    public string closeSkillDetail;
    public string closeEffect;
    public string textSkillName;
    public int textSkillSp;
    public string textSkillDetail;
    public string textEffect;
    public string attackEffect;
    public string rankUp;
    public string background;
    public string flavor;
    public string inventory;
    public string itemId;
    public string illust;
}
