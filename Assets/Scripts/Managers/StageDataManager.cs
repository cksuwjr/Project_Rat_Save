using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageDataManager : SingletonDestroy<StageDataManager>, IManager
{
    public List<StageData> stageDatas;

    public void Init() { }
}
