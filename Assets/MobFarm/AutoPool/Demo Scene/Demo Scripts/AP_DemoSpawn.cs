﻿using UnityEngine;
using System.Collections;

public enum SpawnType
{
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
	int SpawnTime;
	int SpawnTimeMax;
	public float spawnInterval;
	public float spawnVelocity;
	public float spawnAngleError;

	float nextSpawn;
	Rigidbody myRigidbody;

	void Awake () {
		myRigidbody = gameObject.GetComponent<Rigidbody>();
		MF_AutoPool.InitializeSpawn( spawnPrefab, addToPool, minPool );
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
		if (spawnPrefab == null)
			return;

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

			if (randomChild == true)
			{
				if (_Type == SpawnType.Weapon)
				{
					if (spawnPrefab.GetComponent<SkillData>().Data1.WeaponType == WeaponType.세갈래공격)
					{
						obj = MF_AutoPool.Spawn(spawnPrefab, Random.Range(0, 3), transform.position + (spawnAngle * transform.forward), transform.rotation * spawnAngle);
                        obj = MF_AutoPool.Spawn(spawnPrefab, Random.Range(0, 3), new Vector3(transform.position.x - 1f, transform.position.y, transform.position.z), transform.rotation * spawnAngle);
						obj = MF_AutoPool.Spawn(spawnPrefab, Random.Range(0, 3), new Vector3(transform.position.x + 1f, transform.position.y, transform.position.z), transform.rotation * spawnAngle);
					}
                    else if (spawnPrefab.GetComponent<SkillData>().Data1.WeaponType == WeaponType.갈래공격)
					{
						obj = MF_AutoPool.Spawn(spawnPrefab, Random.Range(0, 3), transform.position + (spawnAngle * transform.forward), transform.rotation * spawnAngle);
						obj = MF_AutoPool.Spawn(spawnPrefab, Random.Range(0, 3), new Vector3(transform.position.x - 1f, transform.position.y, transform.position.z), transform.rotation * spawnAngle);
                    }
                    else
                    {
						obj = MF_AutoPool.Spawn(spawnPrefab, Random.Range(0, 3), transform.position + (spawnAngle * transform.forward), transform.rotation * spawnAngle);
					}
				}
				else if (_Type == SpawnType.Monster)
					obj = MF_AutoPool.Spawn(spawnPrefab, Random.Range(0, 3), MapManager.instance._player._SpawnPoint[Random.Range(0, MapManager.instance._player._SpawnPoint.Length)].position, Quaternion.identity);
			}
			else
			{
				obj = MF_AutoPool.Spawn(spawnPrefab, transform.position + (spawnAngle * transform.forward), transform.rotation * spawnAngle);
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
