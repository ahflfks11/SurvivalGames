using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct CharacterStat
{
    public string _ChracterName;
    public float Health;
    [Range(0, 20)]
    public float Speed;
}

public class CharaterData : MonoBehaviour
{
    [SerializeField]
    private CharacterStat _Stat;
    Animator _AniController;
    PlayerController _playerController;
    Rigidbody2D _rigid;

    float _durationTime = 0.5f;
    float _tempDurationTime = 0f;
    bool _isAlive;
    public CharacterStat Stat { get => _Stat; set => _Stat = value; }
    public bool IsAlive { get => _isAlive; set => _isAlive = value; }

    // Start is called before the first frame update
    void Start()
    {
        _AniController = transform.GetComponent<Animator>();
        _playerController = transform.GetComponentInParent<PlayerController>();
        _rigid = MapManager.instance._player.GetComponent<Rigidbody2D>();
        _playerController._MaxHP = _Stat.Health;
        _playerController._RecentHP = _Stat.Health;
        IsAlive = true;
        _playerController.AddSkill(MapManager.WeaponType.NormalAttack);
    }

    // Update is called once per frame
    void Update()
    {
        if (_AniController.GetCurrentAnimatorStateInfo(0).IsName("Death"))
        {
            if (_AniController.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.95)
            {
                MapManager.instance.LoadMap();
            }
            return;
        }

        if (_tempDurationTime > 0) _tempDurationTime -= Time.deltaTime;

        if (_playerController.MoveDir.sqrMagnitude != 0)
        {
            if (_playerController.PosX < 0)
            {
                transform.rotation = Quaternion.Euler(0, -180, 0);
            }
            else
            {
                transform.rotation = Quaternion.Euler(0, 0, 0);
            }

            _AniController.SetBool("IsWalking", true);
        }
        else
        {
            _AniController.SetBool("IsWalking", false);
        }
    }

    public void HitDamage(float _dmg)
    {
        _playerController._RecentHP -= _dmg;

        if (_playerController._RecentHP <= 0)
        {
            IsAlive = false;
            _AniController.SetTrigger("Death");
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (!IsAlive)
            return;

        if (collision.gameObject.tag != "Monster")
            return;

        if (_tempDurationTime <= 0)
        {
            HitDamage(collision.gameObject.GetComponent<Monster>().Data._Damage);
            _tempDurationTime = _durationTime;
        }
    }
}
