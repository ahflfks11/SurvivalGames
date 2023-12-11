using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthRegen : MonoBehaviour
{
    SkillData _skillData;
    public float _genTime = 20f;
    float _TempGenTime;

    // Start is called before the first frame update
    void Start()
    {
        _skillData = transform.GetComponent<SkillData>();
        _TempGenTime = _genTime;
    }

    // Update is called once per frame
    void Update()
    {
        if (_TempGenTime <= 0)
        {
            MapManager.instance.SpawnEffects(MapManager.instance._player.gameObject, 0);
            MapManager.instance._player.CharacterData.Healing(_skillData.Data1._Damage);
            _TempGenTime = _genTime;
        }
        else
        {
            _TempGenTime -= Time.deltaTime;
        }
    }
}
