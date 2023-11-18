using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    public static MapManager instance;
    public PlayerController _player;
    public AP_Pool WeaponManager;
    public AP_Pool MobManager;
    public RuleTile _MapTileSet;
    private void Awake()
    {
        instance = this;
        _MapTileSet.m_TilingRules[0].m_PerlinScale = Random.Range(0.01f, 0.999f);
    }
}
