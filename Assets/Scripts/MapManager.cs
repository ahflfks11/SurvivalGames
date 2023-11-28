using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MapManager : MonoBehaviour
{
    public enum MonsterType
    {
        GreenSlime = 0,
        BloodSlime,
        HellDog,
        Ghost_Spirit
    };

    public enum ItemType
    {
        Coin = 0,
        item
    };

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
        bomb
    };

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
    int min;
    int sec;

    [SerializeField]
    float _MagicSize = 0f;

    float _CommonDamage = 0f;

    public int Min { get => min; set => min = value; }
    public int Sec { get => sec; set => sec = value; }
    public float MagicSize { get => _MagicSize; set => _MagicSize = value; }
    public float CommonDamage { get => _CommonDamage; set => _CommonDamage = value; }

    private void Awake()
    {
        instance = this;
        _MapTileSet.m_TilingRules[0].m_PerlinScale = Random.Range(0.01f, 0.999f);
        Application.targetFrameRate = 60;

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
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
            LoadMap();

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
        SceneManager.LoadScene(0);
    }
}
