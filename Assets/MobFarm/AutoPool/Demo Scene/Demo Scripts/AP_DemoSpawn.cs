using UnityEngine;
using System.Collections;

public enum SpawnType
{
	None,
	Monster,
	Weapon,
	Item
};

public class AP_DemoSpawn : MonoBehaviour {

	public SpawnType _Type;
	public GameObject spawnPrefab;
	public bool randomChild;
	public int addToPool;
	public int minPool;

	[SerializeField]
	int _level;
	int _WeaponNumber;
	[SerializeField]
	int _ShootingCounter = 1;
	int SpawnTime;
	int SpawnTimeMax;
	[SerializeField]
	float _dmg = -1f;
	float _TempSpawnInterval;
	public float spawnInterval;
	public float spawnVelocity;
	public float spawnAngleError;

	float _spawnPosX = -2.5f;
	float _Max_spawnPosX = 2.5f;
	float _spawnPosY = -2.5f;
	float _Max_SpawnPosY = 2.5f;

	bool _SpawningTimeChange;
	[SerializeField]
	bool _randomCounterSpawn;
	bool _isBattleUIActive;
	float nextSpawn;
	Rigidbody myRigidbody;
	SkillData _skillData;

    public int Level { get => _level; set => _level = value; }
    public int WeaponNumber { get => _WeaponNumber; set => _WeaponNumber = value; }
    public float Dmg { get => _dmg; set => _dmg = value; }
    public int ShootingCounter { get => _ShootingCounter; set => _ShootingCounter = value; }

    void Awake () {
		myRigidbody = gameObject.GetComponent<Rigidbody>();
		MF_AutoPool.InitializeSpawn( spawnPrefab, addToPool, minPool );
		_level = 0;
	}

    private void OnEnable()
    {
		if (_Type == SpawnType.Monster)
		{
			SpawnTime = spawnPrefab.GetComponent<Monster>().Data.SpawnLevel;
			SpawnTimeMax = spawnPrefab.GetComponent<Monster>().Data.SpawnLimitLevel;
		}
		else if (_Type == SpawnType.Weapon)
		{
			_TempSpawnInterval = -1;
		}
	}

    void Update () {
		if (spawnPrefab == null || _Type == SpawnType.None) return;

		if (_skillData == null && _Type == SpawnType.Weapon) _skillData = spawnPrefab.GetComponent<SkillData>();

		//특수효과
		if (_Type == SpawnType.Monster)
		{
			Monster _MobData = spawnPrefab.GetComponent<Monster>();

			if (_MobData.Data._PopupText != null)
			{
				if (_MobData.Data.SpawnLevel - 1 == MapManager.instance.Min && MapManager.instance.Sec >= 30f)
				{
					MapManager.instance._uiManager.BattleUI(_MobData.Data._PopupText);
					_isBattleUIActive = true;
				}

				if (_MobData.Data.SpawnLevel - 1 < MapManager.instance.Min && _isBattleUIActive)
				{
					MapManager.instance._uiManager.BattleUI("");
					_isBattleUIActive = false;
				}
			}
		}

		if (MapManager.instance._player.Scanner.nearestTarget == null && _Type == SpawnType.Weapon && (int)_skillData.Data1.WeaponType < 8)
			return;

		if (MapManager.instance.Min < SpawnTime)
			return;

		if (_Type == SpawnType.Monster)
		{
			if (SpawnTimeMax != 0 && MapManager.instance.Min >= SpawnTimeMax)
				return;

			if ((MapManager.instance.Min != 0 && MapManager.instance.Min % 2 == 0) && spawnInterval > 0.05f)
			{
				if (!_SpawningTimeChange)
				{
					spawnInterval -= 0.01f;
					_SpawningTimeChange = true;
				}
			}
			else
			{
				_SpawningTimeChange = false;
			}
		}

		if ( Time.time >= nextSpawn) {

			Vector2 errorV2 = Random.insideUnitCircle * spawnAngleError;
			Quaternion spawnAngle = Quaternion.Euler( errorV2.x, errorV2.y, 0 );

			GameObject obj = null;

			if (_Type == SpawnType.Weapon)
			{
				for (int i = 0; i < _ShootingCounter; i++)
				{
					obj = MF_AutoPool.Spawn(spawnPrefab, Random.Range(0, 3), new Vector3(transform.position.x - i, transform.position.y, transform.position.z), spawnPrefab.transform.rotation * spawnAngle);
					//obj.GetComponent<SkillData>().MySpanwer = this;
				}
			}
			else if (_Type == SpawnType.Monster)
			{
				int _SpawnNumber = Random.Range(0, MapManager.instance._player._SpawnPoint.Length);
				int _randomSpawnCounter = _randomCounterSpawn ? _randomSpawnCounter = Random.Range(1, _ShootingCounter) : _randomSpawnCounter = ShootingCounter;

				for (int i = 0; i < _randomSpawnCounter; i++)
				{
					obj = MF_AutoPool.Spawn(spawnPrefab, Random.Range(0, 3), new Vector3(MapManager.instance._player._SpawnPoint[_SpawnNumber].position.x - Random.Range(_spawnPosX, _Max_spawnPosX), MapManager.instance._player._SpawnPoint[_SpawnNumber].position.y - Random.Range(_spawnPosY, _Max_SpawnPosY), MapManager.instance._player._SpawnPoint[_SpawnNumber].position.z), Quaternion.identity);
				}
            }
            else
            {
				int _randomSpawn = Random.Range(0, 3);

				if (_randomSpawn < 1)
				{
					int _SpawnNumber = Random.Range(0, MapManager.instance._player._SpawnPoint.Length);
					obj = MF_AutoPool.Spawn(spawnPrefab, Random.Range(0, 3), new Vector3(MapManager.instance._player._SpawnPoint[_SpawnNumber].position.x - Random.Range(-1, 1), MapManager.instance._player._SpawnPoint[_SpawnNumber].position.y - Random.Range(-1, 1), MapManager.instance._player._SpawnPoint[_SpawnNumber].position.z), Quaternion.identity);
				}
			}

			Rigidbody rb = null;
			if ( obj ) { rb = obj.GetComponent<Rigidbody>(); }
			if ( rb ) {
				Vector3 myVelocity = myRigidbody ? myRigidbody.velocity : Vector3.zero;
				rb.velocity = myVelocity + ( obj.transform.forward * spawnVelocity );
			}

			nextSpawn = Time.time + spawnInterval;
		}
	}
}
