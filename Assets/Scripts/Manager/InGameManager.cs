using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameManager : MonoBehaviour
{
    private static InGameManager instance = null;


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            instance.init();
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    public static InGameManager Instance
    {
        get
        {
            if (instance == null)
            {
                return null;
            }

            return instance;
        }
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
