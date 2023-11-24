using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
public static class Extensions
{
    public static T[] RemoveAt<T>(this T[] source, int index)
    {
        return source.Where((_, i) => i != index).ToArray();
    }
}

public class PlayerController : MonoBehaviour
{
    VariableJoystick _Joystick;
    float _posX;
    float _posY;
    float _speed;
    public float _RecentHP;
    public float _MaxHP;
    public float _RecentEXP;
    public int _level;

    Vector2 moveDir;

    Scanner _scanner;

    Rigidbody2D _rigid;
    public Transform[] _SpawnPoint;
    public GameObject _WeaponSpawner;
    CharaterData _CharacterData;

    public float[] _ExpList;

    public Vector2 MoveDir { get => moveDir; set => moveDir = value; }
    public float PosX { get => _posX; set => _posX = value; }
    public Scanner Scanner { get => _scanner; set => _scanner = value; }
    public Rigidbody2D Rigid { get => _rigid; set => _rigid = value; }
    public CharaterData CharacterData { get => _CharacterData; set => _CharacterData = value; }

    // Start is called before the first frame update
    void Start()
    {
        _Joystick = GameObject.FindAnyObjectByType<VariableJoystick>();
        _scanner = GameObject.FindAnyObjectByType<Scanner>();
        _rigid = transform.GetComponentInChildren<Rigidbody2D>();
        _SpawnPoint = GameObject.Find("Spawner").GetComponentsInChildren<Transform>();
        _SpawnPoint = _SpawnPoint.RemoveAt<Transform>(0);
        CharacterData = transform.GetComponentInChildren<CharaterData>();
        _speed = CharacterData.Stat.Speed;

        if (Application.platform != RuntimePlatform.Android)
            _Joystick.gameObject.SetActive(false);

        _RecentEXP = 0;
        _level = 0;
    }

    //스킬 등록
    public bool AddSkill(MapManager.WeaponType _weaponType)
    {
        AP_DemoSpawn _spawnerConfig = null;
        GameObject _SkillPrefab = null;

        for (int i = 0; i < MapManager.instance._MySkill.Count; i++)
        {
            if(MapManager.instance._MySkill[i]._weapon == _weaponType)
            {
                AP_DemoSpawn[] SpawnList = _WeaponSpawner.GetComponents<AP_DemoSpawn>();
                AP_DemoSpawn _Skill = null;
                foreach (AP_DemoSpawn Ap_Skill in SpawnList)
                {
                    if (Ap_Skill.spawnPrefab.GetComponent<SkillData>()._WeaponType == MapManager.instance._MySkill[i]._skills[Ap_Skill.Level]._weaponType)
                    {
                        _Skill = Ap_Skill;
                        break;
                    }
                }

                if (_Skill != null)
                {
                    if (_Skill.Level < 4)
                    {
                        _Skill.Level++;

                        if (MapManager.instance._MySkill[i]._skills[_Skill.Level]._ShootingCOunter != _Skill.ShootingCounter)
                        {
                            GameObject[] _skillPool = GameObject.FindGameObjectsWithTag("WeaponPool");
                            AP_Pool _tempSkillPool = null;

                            for (int j = 0; j < _skillPool.Length; j++)
                            {
                                if (_skillPool[j].GetComponent<AP_Pool>().poolBlock.prefab.GetComponent<SkillData>()._WeaponType == _weaponType)
                                {
                                    _tempSkillPool = _skillPool[j].GetComponent<AP_Pool>();
                                    break;
                                }
                            }

                            if (_tempSkillPool != null)
                            {
                                if (_tempSkillPool.poolBlock.maxSize < MapManager.instance._MySkill[i]._skills[_Skill.Level]._ShootingCOunter)
                                    _tempSkillPool.poolBlock.maxSize++;

                                _Skill.ShootingCounter = MapManager.instance._MySkill[i]._skills[_Skill.Level]._ShootingCOunter;
                            }
                        }
                    }
                }

                return false;
            }
        }

        _WeaponSpawner.AddComponent<AP_DemoSpawn>();

        AP_DemoSpawn[] SpawnTargets = _WeaponSpawner.GetComponents<AP_DemoSpawn>();

        for (int i = 0; i < SpawnTargets.Length; i++)
        {
            if (SpawnTargets[i]._Type == SpawnType.None)
            {
                _spawnerConfig = SpawnTargets[i];
                break;
            }
        }

        foreach (AP_Pool _Skill in MapManager.instance.WeaponManager)
        {
            if (_Skill.poolBlock.prefab.GetComponent<SkillData>()._WeaponType == _weaponType)
            {
                _SkillPrefab = _Skill.poolBlock.prefab;
                break;
            }
        }

        if (_SkillPrefab == null) return false;

        MapManager.instance.Add_Weapon(_weaponType);
        _spawnerConfig.spawnPrefab = _SkillPrefab;
        _spawnerConfig._Type = SpawnType.Weapon;
        _spawnerConfig.randomChild = true;
        _spawnerConfig.spawnInterval = 1;
        return true;

    }

    public void LevelUp()
    {
        _RecentEXP = 0;
        _level++;
        MapManager.instance._uiManager.Panel();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            LevelUp();
        }

        if (_RecentEXP >= _ExpList[_level])
        {
            if (_level < _ExpList.Length - 1) LevelUp();
        }

        if (Application.platform == RuntimePlatform.Android)
        {
            PosX = _Joystick.Horizontal;
            _posY = _Joystick.Vertical;
        }
        else
        {
            _posX = Input.GetAxis("Horizontal");
            _posY = Input.GetAxis("Vertical");
        }


        MoveDir = new Vector2(PosX, _posY);

        if (MoveDir.sqrMagnitude != 0)
        {
            transform.Translate(MoveDir * _speed * Time.deltaTime, Space.World);
        }
    }
}
