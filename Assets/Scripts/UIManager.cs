using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public Text _TimerText;
    public Text _EXPText;
    public Slider _HPUI;
    public Slider _EXPUI;

    // Update is called once per frame
    void Update()
    {
        _EXPText.text = MapManager.instance._player._RecentEXP + " / " + MapManager.instance._player._ExpList[MapManager.instance._player._level];
        _EXPUI.value = MapManager.instance._player._RecentEXP / MapManager.instance._player._ExpList[MapManager.instance._player._level];
        _HPUI.value = MapManager.instance._player._RecentHP / MapManager.instance._player._MaxHP;
        _TimerText.text = string.Format("{0:D2}:{1:D2}", MapManager.instance.Min, MapManager.instance.Sec);
    }
}
