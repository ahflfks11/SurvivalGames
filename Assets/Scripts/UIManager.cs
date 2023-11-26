using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public UI_Skill_Icon _UISkillIcon;
    public Transform _UIContents;
    public GameObject _UIPanel;
    public Text _TimerText;
    public Text _EXPText;
    public Slider _HPUI;
    public Slider _EXPUI;

    public void Panel()
    {
        if (!MapManager.instance._player.CharacterData.IsAlive)
            return;

        if (_UIPanel.activeSelf)
        {
            Time.timeScale = 1f;

            UI_Skill_Icon[] _Find_UI_Icon = GameObject.FindObjectsOfType<UI_Skill_Icon>();

            for (int i = 0; i < _Find_UI_Icon.Length; i++)
                Destroy(_Find_UI_Icon[i].gameObject);

            _UIPanel.SetActive(false);
        }
        else
        {
            Time.timeScale = 0f;
            _UIPanel.SetActive(true);
            int _UICount = 0;
            List<int> _tempUIList = new List<int>();

            while (true)
            {
                double sum = 0;

                for (int j = 0; j < MapManager.instance._skillList.Count; j++)
                {
                    if (!MapManager.instance._skillList[j]._Non_Active)
                        sum += MapManager.instance._skillList[j]._Rarelity;
                }

                sum *= Random.value;
                bool _uiPossible = true;

                for (int j = 0; j < MapManager.instance._skillList.Count; j++)
                {
                    if (!MapManager.instance._skillList[j]._Non_Active)
                    {
                        sum -= MapManager.instance._skillList[j]._Rarelity;

                        if (sum <= 0)
                        {
                            for (int k = 0; k < _tempUIList.Count; k++)
                            {
                                if (_tempUIList[k] == j)
                                    _uiPossible = false;
                            }

                            if (_uiPossible)
                            {
                                int _Result_Number = 0;
                                for (int i = 0; i < MapManager.instance._MySkill.Count; i++)
                                {
                                    if (MapManager.instance._MySkill[i]._weapon == MapManager.instance._skillList[j]._skillPrefab.GetComponent<SkillData>()._WeaponType)
                                    {
                                        _Result_Number = MapManager.instance._MySkill[i].currectLevel + 1;
                                    }
                                }

                                GameObject _UI_Icon = Instantiate(_UISkillIcon.gameObject, Vector3.zero, Quaternion.identity);
                                _UI_Icon.transform.SetParent(_UIContents);
                                _UI_Icon.transform.position = Vector3.zero;
                                _UI_Icon.transform.localScale = new Vector3(1f, 1f, 1f);

                                UI_Skill_Icon _Contents_UI = _UI_Icon.GetComponent<UI_Skill_Icon>();
                                _Contents_UI.GetComponent<Image>().sprite = MapManager.instance._skillList[j]._Icon;
                                _Contents_UI._weaponType = MapManager.instance._skillList[j]._skillPrefab.GetComponent<SkillData>()._WeaponType;
                                _Contents_UI._TitleLabel.text = MapManager.instance._skillList[j]._skill[_Result_Number]._SkillName;
                                _Contents_UI._CommentLine.text = MapManager.instance._skillList[j]._skill[_Result_Number]._SkillComment;
                                _UICount++;
                                _tempUIList.Add(j);
                                break;
                            }
                        }
                    }
                }

                if (_UICount > 2)
                    break;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        _EXPText.text = MapManager.instance._player._RecentEXP + " / " + MapManager.instance._player._ExpList[MapManager.instance._player._level];
        _EXPUI.value = MapManager.instance._player._RecentEXP / MapManager.instance._player._ExpList[MapManager.instance._player._level];
        _HPUI.value = MapManager.instance._player._RecentHP / MapManager.instance._player._MaxHP;
        _TimerText.text = string.Format("{0:D2}:{1:D2}", MapManager.instance.Min, MapManager.instance.Sec);
    }
}
