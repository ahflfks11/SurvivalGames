using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleMagic : MonoBehaviour
{
    float _Scale;
    int level;
    SkillData _skillData;

    private void OnEnable()
    {
        _skillData = transform.GetComponent<SkillData>();
    }

    private void Update()
    {
        for (int i = 0; i < MapManager.instance._MySkill.Count; i++)
        {
            if (MapManager.instance._MySkill[i]._weapon == _skillData._WeaponType)
            {
                transform.localScale = new Vector3((MapManager.instance._MySkill[i].currectLevel + 1),(MapManager.instance._MySkill[i].currectLevel + 1),(MapManager.instance._MySkill[i].currectLevel + 1));
                break;
            }
        }
    }
}
