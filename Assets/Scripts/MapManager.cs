using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MapManager : MonoBehaviour
{
    //몬스터 종류
    public enum MonsterType
    {
        GreenSlime = 0,
        BloodSlime,
        HellDog,
        Ghost_Spirit,
        Wolf
    };

    //아이템 종류
    public enum ItemType
    {
        Coin = 0,
        item
    };

    //스킬 종류
    public enum WeaponType
    {
        NormalAttack = 0,
        Fireball,
        Magic_Lighting,
        StarLight,
        Moving_Weapon,
        Rocket,
        Barrior,
        Passive_Health,
        Passive_Speed,
        Passive_Magnet,
        Passive_MagicSize,
        Passive_Damage,
        bomb,
        WeaponAttack,
        CoolDown,
        Health_Regen
    };

    //스킬 세부 정보
    [System.Serializable]
    public struct Skill
    {
        public string _SkillName;
        [Multiline(3)]
        public string _SkillComment;
        public WeaponType _weaponType;
        public int _ShootingCOunter;
        public float _Damage;
        public float _CoolTime;
    }

    //스킬 종류
    [System.Serializable]
    public struct SkillList
    {
        public Sprite _Icon;
        public GameObject _skillPrefab;
        public WeaponType _weapon;
        public Skill[] _skill;
        public double _Rarelity;
        public bool _Non_Active;
    }

    //보유한 스킬 리스트
    [System.Serializable]
    public struct MySkillList
    {
        public WeaponType _weapon;
        public Skill[] _skills;
        public int currectLevel;
    }

    [SerializeField]
    public List<MySkillList> _MySkill;

    public static MapManager instance;
    public UIManager _uiManager;
    public PlayerController _player;
    public AP_Pool[] WeaponManager;
    public AP_Pool[] MobManager;
    public AP_Pool[] ItemList;
    public RuleTile _MapTileSet;
    public Item[] items;
    public GameObject[] _Effects;
    public float _GameTime;

    [SerializeField]
    public List<SkillList> _skillList;

    public List<AudioClip> _vfx;

    public AudioSource _MainBGMListener;

    int min;
    int sec;

    [SerializeField]
    float _MagicSize = 0f;

    float _CommonDamage = 0f;

    [SerializeField]
    float _SpawnInterval;

    BossPattern _bossList;
    bool _BossAlive = true;

    bool _isPlay = true;

    public int Min { get => min; set => min = value; }
    public int Sec { get => sec; set => sec = value; }
    public float MagicSize { get => _MagicSize; set => _MagicSize = value; }
    public float CommonDamage { get => _CommonDamage; set => _CommonDamage = value; }
    public float SpawnInterval { get => _SpawnInterval; set => _SpawnInterval = value; }
    public BossPattern BossList { get => _bossList; set => _bossList = value; }
    public bool BossAlive { get => _BossAlive; set => _BossAlive = value; }

    private void Awake()
    {
        instance = this;

        ChangeMap();

        Application.targetFrameRate = 60;
        Screen.sleepTimeout = SleepTimeout.NeverSleep;

        GameObject[] _MonsterPool = GameObject.FindGameObjectsWithTag("MonsterPool");
        GameObject[] _WeaponPool = GameObject.FindGameObjectsWithTag("WeaponPool");

        MobManager = new AP_Pool[_MonsterPool.Length];
        WeaponManager = new AP_Pool[_WeaponPool.Length];
        
        //몬스터 풀 관리
        for (int i = 0; i < _MonsterPool.Length; i++)
            MobManager[i] = _MonsterPool[i].GetComponent<AP_Pool>();

        //마법 풀 관리
        for (int i = 0; i < _WeaponPool.Length; i++)
            WeaponManager[i] = _WeaponPool[i].GetComponent<AP_Pool>();

        _MySkill = new List<MySkillList>();
        BossList = transform.GetComponent<BossPattern>();
        Time.timeScale = 1f;
        SetReSolution();
    }

    public void ChangeMap()
    {
        _MapTileSet.m_TilingRules[0].m_PerlinScale = Random.Range(0.01f, 0.999f);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
            LoadMap();

        if (!_BossAlive)
        {
            if (_isPlay)
            {
                _uiManager.Clear();
                _isPlay = false;
            }
            return;
        }

        _GameTime += Time.deltaTime;

        min = Mathf.FloorToInt(_GameTime / 60f);
        sec = Mathf.FloorToInt(_GameTime % 60f);
    }

    public void ChangeInterval(float _value)
    {
        SpawnInterval += _value;

        AP_DemoSpawn[] _Weapons = _player._WeaponSpawner.GetComponents<AP_DemoSpawn>();

        foreach (AP_DemoSpawn _WeaponSkill in _Weapons)
        {
            if (!_WeaponSkill.spawnPrefab.GetComponent<SkillData>().Passive)
            {
                _WeaponSkill.spawnInterval = _WeaponSkill.spawnInterval - _value >= 0.2f ? _WeaponSkill.spawnInterval -= _value : _WeaponSkill.spawnInterval = 0.2f;
                _WeaponSkill.spawnInterval = Mathf.Floor(_WeaponSkill.spawnInterval * 100f) / 100f;
            }

        }
    }

    public void Change_BGM(AudioClip _number)
    {
        _MainBGMListener.Stop();
        _MainBGMListener.clip = _number;
        _MainBGMListener.Play();
    }

    public void SetReSolution()
    {
        Screen.SetResolution(Screen.width, Screen.height, true);
    }

    public void CommonDamageUp(float _value)
    {
        _CommonDamage += _value;
    }

    public void MagicSizeUp(float _value)
    {
        MagicSize += _value;
    }
        
    public void SpawnEffects(GameObject _Object, int _EffectNumber)
    {
        if (GameObject.Find("Healing_Effects"))
            return;

        GameObject effectsPrefabs = Instantiate(_Effects[_EffectNumber], Vector3.zero, Quaternion.identity);
        effectsPrefabs.transform.SetParent(_Object.transform);
        effectsPrefabs.transform.localPosition = Vector3.zero;
        effectsPrefabs.gameObject.name = "Healing_Effects";
        Destroy(effectsPrefabs, 3f);
    }

    //무기 등록
    public void Add_Weapon(WeaponType _weapon)
    {
        foreach (SkillList _skill in _skillList)
        {
            if (_skill._weapon == _weapon)
            {
                MySkillList _newSkill = new MySkillList();
                _newSkill._weapon = _weapon;
                _newSkill._skills = _skill._skill;
                _newSkill.currectLevel = 0;
                _MySkill.Add(_newSkill);
                break;
            }
        }
    }

    public void LoadMap()
    {
        SceneManager.LoadScene(1);
    }

    public void Home()
    {
        SceneManager.LoadScene(0);
    }
}
