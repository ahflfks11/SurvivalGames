using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossLauncher : MonoBehaviour
{
    [SerializeField]
    BossPattern.Pattern[] _Pattern;
    public string _Name;

    private void Start()
    {
        _Pattern = MapManager.instance.BossList.Process_BossPattern(this.gameObject, _Name);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.X) && _Pattern != null)
        {
            DOTween.PlayForward(this.gameObject, _Pattern[Random.Range(0, _Pattern.Length)].ToString());
        }
    }
}
