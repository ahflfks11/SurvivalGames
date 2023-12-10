using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//캐릭터 정보
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
    bool _isRight = true;
    public CharacterStat Stat { get => _Stat; set => _Stat = value; }
    public bool IsAlive { get => _isAlive; set => _isAlive = value; }
    public bool IsRight { get => _isRight; set => _isRight = value; }

    // Start is called before the first frame update
    void Start()
    {
        _AniController = transform.GetComponent<Animator>();
        _playerController = transform.GetComponentInParent<PlayerController>();
        _rigid = MapManager.instance._player.GetComponent<Rigidbody2D>();
        _playerController._MaxHP = _Stat.Health;
        _playerController._RecentHP = _Stat.Health;
        IsAlive = true;
        _playerController.AddSkill(MapManager.WeaponType.WeaponAttack);
    }

    // Update is called once per frame
    void Update()
    {
        //죽었을 경우 아무것도 처리하지 않는다.
        if (_AniController.GetCurrentAnimatorStateInfo(0).IsName("Death"))
        {
            if (_AniController.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.95)
            {
                MapManager.instance._uiManager.DeathPanel();
            }
            return;
        }

        //공격받았을 경우 일정 텀을 두고 공격을 받도록 한다.
        if (_tempDurationTime > 0) _tempDurationTime -= Time.deltaTime;

        if (_playerController.MoveDir.sqrMagnitude != 0)
        {
            if (_playerController.PosX < 0)
            {
                transform.rotation = Quaternion.Euler(0, -180, 0);
                _isRight = false;
            }
            else
            {
                _isRight = true;
                transform.rotation = Quaternion.Euler(0, 0, 0);
            }

            _AniController.SetBool("IsWalking", true);
        }
        else
        {
            _AniController.SetBool("IsWalking", false);
        }
    }

    //체력 회복
    public void Healing(float _Heal)
    {
        if (_playerController._RecentHP + _Heal <= _playerController._MaxHP)
            _playerController._RecentHP += _Heal;
        else
            _playerController._RecentHP = _playerController._MaxHP;
    }

    //공격 받았을 시
    public void HitDamage(float _dmg)
    {
        _playerController._RecentHP -= _dmg;

        if (_playerController._RecentHP <= 0)
        {
            IsAlive = false;
            _AniController.SetTrigger("Death");
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!IsAlive)
            return;

        if (collision.gameObject.tag != "Monster")
            return;

        HitDamage(collision.gameObject.GetComponent<Monster>().Data._Damage);

        _tempDurationTime = _durationTime;
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.tag != "Monster")
            return;

        if (_tempDurationTime <= 0)
        {
            Monster _Monster = collision.gameObject.GetComponent<Monster>();
            if (_Monster.IsDead) return;
            HitDamage(_Monster.Data._Damage);

            _tempDurationTime = _durationTime;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Boss") && !collision.CompareTag("MonsterWeapon"))
            return;

        if (collision.CompareTag("Boss"))
        {
            if (collision.GetComponent<BossLauncher>().IsAttack)
            {
                HitDamage(collision.GetComponent<BossLauncher>().Damage);
                _tempDurationTime = _durationTime;
            }
        }
        else
        {
            HitDamage(collision.GetComponent<MonsterBullet>()._damage);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (!collision.CompareTag("Boss"))
            return;

        if (_tempDurationTime <= 0)
        {
            BossLauncher _Monster = collision.gameObject.GetComponent<BossLauncher>();
            HitDamage(_Monster.Damage);

            _tempDurationTime = _durationTime;
        }
    }
}
