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

    public bool AddSkill(MapManager.WeaponType _weaponType)
    {
        AP_DemoSpawn[] _SkillList = _WeaponSpawner.GetComponents<AP_DemoSpawn>();
        AP_DemoSpawn _spawnerConfig = null;
        GameObject _SkillPrefab = null;

        if (_SkillList.Length != 0)
        {
            foreach (AP_DemoSpawn _Skill in _SkillList)
            {
                if(_Skill.spawnPrefab.GetComponent<SkillData>()._WeaponType == _weaponType)
                {
                    //Config
                    return false;
                }
            }
        }
        _WeaponSpawner.AddComponent<AP_DemoSpawn>();
        _spawnerConfig = _WeaponSpawner.GetComponent<AP_DemoSpawn>();

        foreach (AP_Pool _Skill in MapManager.instance.WeaponManager)
        {
            if (_Skill.poolBlock.prefab.GetComponent<SkillData>()._WeaponType == _weaponType)
            {
                _SkillPrefab = _Skill.poolBlock.prefab;
                break;
            }
        }

        if (_SkillPrefab == null) return false;

        _spawnerConfig.spawnPrefab = _SkillPrefab;
        _spawnerConfig._Type = SpawnType.Weapon;
        _spawnerConfig.randomChild = true;
        _spawnerConfig.spawnInterval = 1;

        return true;

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            AddSkill(MapManager.WeaponType.Fireball);
        }

        if (_RecentEXP >= _ExpList[_level])
        {
            _RecentEXP = 0;
            if (_level < _ExpList.Length - 1) _level++;

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
