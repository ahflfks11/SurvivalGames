using System.Collections.Generic;
using UnityEngine;

public class BossPattern : MonoBehaviour
{
    public enum Pattern
    {
        None = 0,
        Running
    }

    [System.Serializable]
    public struct BossSkill
    {
        public string _BossName;
        public Pattern[] _BossPattern;
    }

    [SerializeField]
    List<BossSkill> _patternList;

    public List<BossSkill> PatternList { get => _patternList; set => _patternList = value; }

    public Pattern[] Process_BossPattern(GameObject _bossObject, string BossName)
    {
        foreach (BossSkill _skill in _patternList)
        {
            if(_skill._BossName == BossName)
            {
                return _skill._BossPattern;
            }
        }

        return null;
    }
}
