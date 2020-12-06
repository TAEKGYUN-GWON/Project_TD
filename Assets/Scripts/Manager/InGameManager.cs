using System;
using System.Collections;
using System.Collections.Generic;
using System.Transactions;
using UnityEngine;
using UnityEngine.UI;

public class InGameManager : Singleton<InGameManager>
{

    public int gameLevel = 1;

    public float WaveTimer { get => _waveCountdownTimer; }

    private float _waveCountdownTimer = 0;

    public bool IsWaveActive { get => isWaveActive; set => isWaveActive = value; }

    [SerializeField]
    private Text waveCountdownText;

    [SerializeField]
    private bool isWaveActive = false;
    void Awake()
    {
        Instance.Init();
    }

    private void Init()
    {
        var tableMgr = TableManager.Instance;

        tableMgr.Read("info_character_base");
        tableMgr.Read("info_character_enhance");
        tableMgr.Read("info_character_levelup");
        tableMgr.Read("info_enemy");
        tableMgr.Read("info_enemy_wave");

        _waveCountdownTimer = 300f;

        StartCoroutine(GameTimerCountDown());
    }

    IEnumerator GameTimerCountDown()
    {
        while(true)
        {
            if (isWaveActive)
            {
                _waveCountdownTimer--;
                waveCountdownText.text = Math.Round(_waveCountdownTimer).ToString();

                if (_waveCountdownTimer <= 0f)
                {
                    _waveCountdownTimer = 0f;
                    isWaveActive = false;
                    waveCountdownText.text = "Time Over";
                }
            }

            yield return OverStory.YieldInstructionCache.WaitForSeconds(1f);
        }
    }
}
