using System;
using System.Collections;
using System.Collections.Generic;
using System.Transactions;
using UnityEngine;
using UnityEngine.UI;

public class InGameManager : Singleton<InGameManager>
{

    public int gameLevel = 1;

    public float WaveTimer { get => waveCountdownTimer; }

    private float waveCountdownTimer = 0;

    public bool IsWaveActive { get => isWaveActive; set => isWaveActive = value; }

    [SerializeField]
    private Text waveCountdownText;

    [SerializeField]
    private bool isWaveActive = false;
    void Awake()
    {
        Instance.init();
    }

    private void init()
    {
        var TableMgr = TableManager.Instance;

        TableMgr.Read("info_character_base");
        TableMgr.Read("info_character_enhance");
        TableMgr.Read("info_character_levelup");
        TableMgr.Read("info_enemy");
        TableMgr.Read("info_enemy_wave");

        waveCountdownTimer = 20f;

        StartCoroutine(gameTimerCountDown());
    }

    IEnumerator gameTimerCountDown()
    {
        while(true)
        {
            if (isWaveActive)
            {
                waveCountdownTimer--;
                waveCountdownText.text = Math.Round(waveCountdownTimer).ToString();

                if (waveCountdownTimer <= 0f)
                {
                    waveCountdownTimer = 0f;
                    isWaveActive = false;
                    waveCountdownText.text = "Time Over";
                }
            }

            yield return OverStory.YieldInstructionCache.WaitForSeconds(1f);
        }
    }
}
