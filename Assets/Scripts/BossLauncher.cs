using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossLauncher : MonoBehaviour
{
    [SerializeField]
    BossPattern.Pattern[] _Pattern;

    [SerializeField]
    BossPattern.Pattern _NowPattern;
    public string _Name;
    public float Speed;
    Rigidbody2D _rigid;
    Animator _AniController;
    DOTweenPath _path;
    private void Start()
    {
        _Pattern = MapManager.instance.BossList.Process_BossPattern(this.gameObject, _Name);
        _rigid = transform.GetComponent<Rigidbody2D>();
        _AniController = transform.GetComponent<Animator>();
        _path = transform.GetComponent<DOTweenPath>();
        DOTween.Init(false, true, LogBehaviour.ErrorsOnly);
        _path.wps.Add(transform.position);
        RePosition();
    }

    public void RePosition()
    {
        _path.wps.Add(MapManager.instance._player.transform.position);
        _path.DOPlay();
        Debug.Log(_path.wps[1]);
    }

    public void Running()
    {
        _AniController.Play("Run", 0, 1f);
    }

    public void RunningStop()
    {
        _AniController.Play("Idle", 0, 1f);
        RePosition();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.X) && _Pattern != null)
        {
            DOTween.PlayForward(this.gameObject, _Pattern[Random.Range(0, _Pattern.Length)].ToString());
        }
    }

    private void LateUpdate()
    {
        _rigid.velocity = Vector2.zero;
    }
}
