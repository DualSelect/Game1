using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattleFolder
{
    public class DeckStatus
    {
        public string unitId { get; set; }
        public int lockTurn { get; set; }
        public int playStatus { get; set; }//0:未使用　1:場に出ている　2:墓地にある　3:消滅している 4:ロックされている
    }
}