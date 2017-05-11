using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PointOfInterestMetadata {

    [System.Serializable]
    public class EnemyInfo
    {
        public int skull_ghost;
        public int zombie;
        public int shadow;
        public int lich;
        public string ToJson()
        {
            return JsonUtility.ToJson(this, true);
        }
        public static EnemyInfo FromJson(string serzd)
        {
            return JsonUtility.FromJson<EnemyInfo>(serzd);
        }
        public EnemyInfo(int s_g, int z, int s, int l)
        {
            skull_ghost = s_g; zombie = z; shadow = s; lich = l;
        }
        public EnemyInfo() : this(1, 1, 1, 1) {  }
    }

    public EnemyInfo EnmInf = new EnemyInfo();
}
