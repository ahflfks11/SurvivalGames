using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSkillData : MonoBehaviour
{
    public Transform _WeaponDir;
    public GameObject _Weapon;
    bool _SpawnWeapon;

    public bool SpawnWeapon { get => _SpawnWeapon; set => _SpawnWeapon = value; }

    private void Awake()
    {
        MF_AutoPool.InitializeSpawn(_Weapon, 0, 0);
    }

    // Start is called before the first frame update
    void Start()
    {
        if (_WeaponDir == null) _WeaponDir = transform;
    }

    public void SpawnWeaponMethod(int count)
    {
        for (int i = 0; i < count; i++)
        {
            GameObject _bullet = MF_AutoPool.Spawn(_Weapon, _WeaponDir.position, Quaternion.Euler(0, 0, -45f * i));
            _bullet.GetComponent<MonsterBullet>()._Number = i;
        }
    }

    // Update is called once per frame
    void Update()
    {
    }
}
