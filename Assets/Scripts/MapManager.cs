using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    public enum MonsterType
    {
        GreenSlime = 0,
        BloodSlime
    };

    public enum ItemType
    {
        Coin = 0,
        item
    };

    public static MapManager instance;
    public UIManager _uiManager;
    public PlayerController _player;
    public AP_Pool WeaponManager;
    public AP_Pool[] MobManager;
    public AP_Pool[] ItemList;
    public RuleTile _MapTileSet;
    public Item[] items;
    public float _GameTime;
    int min;
    int sec;

    public int Min { get => min; set => min = value; }
    public int Sec { get => sec; set => sec = value; }

    private void Awake()
    {
        instance = this;
        _MapTileSet.m_TilingRules[0].m_PerlinScale = Random.Range(0.01f, 0.999f);
        Application.targetFrameRate = 60;
    }

    private void Update()
    {
        _GameTime += Time.deltaTime;

        min = Mathf.FloorToInt(_GameTime / 60f);
        sec = Mathf.FloorToInt(_GameTime % 60f);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (Time.timeScale != 0)
                Time.timeScale = 0;
            else
                Time.timeScale = 1f;
        }
    }
}
