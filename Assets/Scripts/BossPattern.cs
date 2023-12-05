using System.Collections.Generic;
using UnityEngine;

public class BossPattern : MonoBehaviour
{
    public enum Pattern
    {
        None = 0,
        Teleporting,
        Magic
    }

    [System.Serializable]
    public struct BossSkill
    {
        public Pattern _BossPattern;
        public float _durationTime;
        public float waitingTime;
        public float _SkillCooltime;
        public float castRate;
        public float PatternHP;
        public float _Damage;
        public string _skillName;
        public string _Comment;
    }

    [System.Serializable]
    public struct BossData
    {
        public string _BossName;
        public float HP;
        public float Normal_Damage;
        public BossSkill[] _BossSkill;
    }

    [SerializeField]
    List<BossData> _patternList;

    public List<BossData> PatternList { get => _patternList; set => _patternList = value; }

    public float Boss_HP(GameObject _bossObject, string BossName)
    {
        foreach (BossData _data in _patternList)
        {
            if (_data._BossName == BossName)
            {
                return _data.HP;
            }
        }

        return 0;
    }

    public int Boss_PatternCount(GameObject _bossObject, string BossName, float _hp)
    {
        int count = 0;

        foreach (BossData _data in _patternList)
        {
            if (_data._BossName == BossName)
            {
                for (int i = 0; i < _data._BossSkill.Length; i++)
                {
                    if(_data._BossSkill[i].PatternHP >= _hp)
                    {
                        count = i;
                    }
                }
            }
        }

        if (count == -1)
            return -1;
        else
            return count;
    }

    public float Boss_SkillDamage(GameObject _bossObject, string BossName, int count)
    {
        foreach (BossData _data in _patternList)
        {
            if (_data._BossName == BossName)
            {
                return _data._BossSkill[count]._Damage;
            }
        }

        return 0;
    }

    public float Boss_NormalDamage(GameObject _bossObject, string BossName)
    {
        foreach (BossData _data in _patternList)
        {
            if (_data._BossName == BossName)
            {
                return _data.Normal_Damage;
            }
        }

        return 0;
    }

    public BossSkill[] Process_BossPattern(GameObject _bossObject, string BossName)
    {
        foreach (BossData _data in _patternList)
        {
            if(_data._BossName == BossName)
            {
                return _data._BossSkill;
            }
        }

        return null;
    }
}
