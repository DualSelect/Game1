using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattleFolder
{
    public class UnitStatus
    {
        //def:初期 up:永続変化 tup:ターン中変化
        public int deckId { get; set; } = 99;
        public string unitId { get; set; } = "";
        public int defLV { get; set; }
        public int upLV { get; set; }
        public int tupLV { get; set; }
        public int defHP { get; set; }
        public int upHP { get; set; }
        public int tupHP { get; set; }
        public int nowHP { get; set; }
        public int defSTK { get; set; }
        public int upSTK { get; set; }
        public int tupSTK { get; set; }
        public int defATK { get; set; }
        public int upATK { get; set; }
        public int tupATK { get; set; }
        public int defDFE { get; set; }
        public int upDFE { get; set; }
        public int tupDFE { get; set; }
        public int defAGI { get; set; }
        public int upAGI { get; set; }
        public int tupAGI { get; set; }
        public int defRNG { get; set; }
        public int upRNG { get; set; }
        public int tupRNG { get; set; }
        public bool action { get; set; } = false;//true:行動済み　false:未行動
        public bool close { get; set; } = false;//true:クローズしている　false:クローズしてない
        public bool revival { get; set; } = false;//true:復活する　false:復活しない
        public void SetUnit(Card card)
        {
            unitId = card.itemId;
            defAGI = card.agi;
            defATK = card.atk;
            defDFE = card.dfe;
            defHP = card.hp;
            nowHP = card.hp;
            defLV = card.level;
            defRNG = card.rng;
            defSTK = card.stock;

            upAGI = 0;
            upATK = 0;
            upDFE = 0;
            upHP = 0;
            upLV = 0;
            upRNG = 0;
            upSTK = 0;

            tupAGI = 0;
            tupATK = 0;
            tupDFE = 0;
            tupHP = 0;
            tupLV = 0;
            tupRNG = 0;
            tupSTK = 0;

            action = false;
            close = false;
            revival = false;
        }
        public void SetUnit(Card card,int deckNumber)
        {
            deckId = deckNumber;
            SetUnit(card);
        }
        public UnitStatus Clone()
        {
            return (UnitStatus)MemberwiseClone();
        }
    }

}