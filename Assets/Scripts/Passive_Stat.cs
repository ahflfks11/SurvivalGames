using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SkillData))]
public class Passive_Stat : MonoBehaviour
{
    public enum StatTypes
    {
        MAX_Health = 0,
        Recent_Health,
        Speed,
        MagicSize,
        MagnetRange,
        CommonDamage
    }

    [SerializeField]
    StatTypes _statType;
    SkillData _skillData;
    int level = -1;
    float _originValue;

    public float OriginValue { get => _originValue; set => _originValue = value; }

    private void OnEnable()
    {
        _skillData = transform.GetComponent<SkillData>();
    }

    private void Update()
    {
        if (level != _skillData.Level)
        {
            switch (_statType)
            {
                case StatTypes.MAX_Health:
                    MapManager.instance._player.HealthUp(_skillData.Data1._Damage, true);
                    break;
                case StatTypes.Speed:
                    MapManager.instance._player.SpeedUp(_skillData.Data1._Damage);
                    break;
                case StatTypes.MagnetRange:
                    MapManager.instance._player.Scanner.MagnetUp(_skillData.Data1._Damage);
                    break;
                case StatTypes.MagicSize:
                    MapManager.instance.MagicSizeUp(_skillData.Data1._Damage);
                    break;
                case StatTypes.CommonDamage:
                    MapManager.instance.CommonDamageUp(_skillData.Data1._Damage);
                    break;
            }
            level = _skillData.Level;
        }
    }
}
