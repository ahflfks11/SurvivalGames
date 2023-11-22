using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct SkillType
{
    public MapManager.WeaponType _weaponType;
    public string _SkillName;
    public string _SkillComment;
    public string _Level;
}
public class UI_Skill_Icon : MonoBehaviour
{
    [SerializeField]
    private SkillType _type;

    public SkillType Type { get => _type; set => _type = value; }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
