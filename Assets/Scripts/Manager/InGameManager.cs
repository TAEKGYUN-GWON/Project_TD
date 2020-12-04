using System.Collections;
using System.Collections.Generic;
using System.Transactions;
using UnityEngine;

public class InGameManager : Singleton<InGameManager>
{
    void Awake()
    {
        Instance.init();
    }

    public int gameLevel = 1;

    private void init()
    {
        var TableMgr = TableManager.Instance;

        TableMgr.Read("info_character_base");
        TableMgr.Read("info_character_enhance");
        TableMgr.Read("info_character_levelup");
        TableMgr.Read("info_enemy");
        TableMgr.Read("info_enemy_wave");
    }
}
