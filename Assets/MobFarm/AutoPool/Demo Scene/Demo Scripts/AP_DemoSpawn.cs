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
	int _level;
	int _WeaponNumber;
	[SerializeField]
	int _ShootingCounter = 1;
	int SpawnTime;
	int SpawnTimeMax;
	[SerializeField]
	float _dmg = -1f;
	public float spawnInterval;
	public float spawnVelocity;
	public float spawnAngleError;

	float nextSpawn;
	Rigidbody myRigidbody;

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
	}

    void Update () {
		if (spawnPrefab == null) return;

		if (MapManager.instance._player.Scanner.nearestTarget == null && _Type == SpawnType.Weapon)
			return;

		if (MapManager.instance.Min < SpawnTime)
			return;

		if (_Type == SpawnType.Monster)
		{
			if (SpawnTimeMax != 0 && MapManager.instance.Min >= SpawnTimeMax)
				return;
		}

		if ( Time.time >= nextSpawn ) {

			Vector2 errorV2 = Random.insideUnitCircle * spawnAngleError;
			Quaternion spawnAngle = Quaternion.Euler( errorV2.x, errorV2.y, 0 );

			GameObject obj = null;

			if (_Type == SpawnType.Weapon)
			{
				for (int i = 0; i < _ShootingCounter; i++)
				{
					obj = MF_AutoPool.Spawn(spawnPrefab, Random.Range(0, 3), new Vector3(transform.position.x - i, transform.position.y, transform.position.z), transform.rotation * spawnAngle);
				}
			}
			else if (_Type == SpawnType.Monster)
				obj = MF_AutoPool.Spawn(spawnPrefab, Random.Range(0, 3), MapManager.instance._player._SpawnPoint[Random.Range(0, MapManager.instance._player._SpawnPoint.Length)].position, Quaternion.identity);

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
