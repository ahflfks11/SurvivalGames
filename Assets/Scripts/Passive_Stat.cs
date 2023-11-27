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
        MagnetRange
    }

    [SerializeField]
    StatTypes _statType;
    SkillData _skillData;
    int level = -1;

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
            }
            level = _skillData.Level;
        }
    }
}
