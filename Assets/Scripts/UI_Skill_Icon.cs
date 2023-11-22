using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public struct SkillType
{
    public string _SkillName;
    [Multiline(3)]
    public string _SkillComment;
}
public class UI_Skill_Icon : MonoBehaviour
{
    public MapManager.WeaponType _weaponType;
    public Text _TitleLabel;
    public Text _CommentLine;

    public void SettingText(string _Title, string Comment)
    {
        _TitleLabel.text = _Title;
        _CommentLine.text = Comment;
    }

    [SerializeField]
    private List<SkillType> _type;

    public List<SkillType> Type { get => _type; set => _type = value; }

    public void AddSkill()
    {
        MapManager.instance._player.AddSkill(_weaponType);
        MapManager.instance._uiManager.Panel();
    }
}
