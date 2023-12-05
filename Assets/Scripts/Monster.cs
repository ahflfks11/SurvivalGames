using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct Data
{
    public float _HP;
    public MapManager.MonsterType _MonsterType;
    [Range(1f, 20f)]
    public float _Damage;
    public float _Speed;
    public float _Resistance; // 저항도, 낮을수록 덜 밀림.
    public int SpawnLevel;
    public int SpawnLimitLevel; //특정 레벨 이상부터는 스폰이 되지 않는다.
    public int exp;
    public string _PopupText;
}
public class Monster : MonoBehaviour
{
    [SerializeField]
    Data _Data;

    bool Knowback;
    bool _isDead;
    float _RecentHP;
    float _durationTime = 0.2f;
    float _TempDurationTime = 0f;
    Rigidbody2D _rigid;
    Animator _AniController;
    WaitForFixedUpdate _CorutinTime;
    AudioSource _myAudio;

    public Data Data { get => _Data; set => _Data = value; }
    public bool IsDead { get => _isDead; set => _isDead = value; }

    private void Awake()
    {
        _CorutinTime = new WaitForFixedUpdate();
    }

    private void OnEnable()
    {
        _RecentHP = Data._HP;
        _rigid = transform.GetComponent<Rigidbody2D>();
        _AniController = transform.GetComponent<Animator>();
        _myAudio = transform.GetComponent<AudioSource>();
        //Coin
        MF_AutoPool.InitializeSpawn(MapManager.instance.items[0].gameObject, 0, 0);
    }

    private void FixedUpdate()
    {
        if (_TempDurationTime > 0) _TempDurationTime -= Time.fixedDeltaTime;

        if (_AniController.GetCurrentAnimatorStateInfo(0).IsName("Death"))
        {
            IsDead = true;
            if (_AniController.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.95f)
            {
                MapManager.instance.MobManager[(int)Data._MonsterType].poolBlock.size -= 1;
                GameObject _Coin = MF_AutoPool.Spawn(MapManager.instance.items[0].gameObject, transform.position, Quaternion.identity);
                _Coin.GetComponent<Item>()._exp = _Data.exp;
                Destroy(this.gameObject);
            }
            else
            {
                StartCoroutine(KnockBack());
                return;
            }
        }
        else if (_AniController.GetCurrentAnimatorStateInfo(0).IsName("Hit") && _Data._Resistance > 0)
        {
            StartCoroutine(KnockBack());
            return;
        }

        Vector2 dirVec = MapManager.instance._player.Rigid.position - _rigid.position;
        Vector2 nextVec = dirVec.normalized * Data._Speed * Time.fixedDeltaTime;
        _rigid.MovePosition(_rigid.position + nextVec);
        _rigid.velocity = Vector2.zero;
    }

    private void LateUpdate()
    {
        Math.Truncate(_RecentHP);
        if (MapManager.instance._player.Rigid.position.x > _rigid.position.x)
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
        else
            transform.rotation = Quaternion.Euler(new Vector3(0, -180f, 0));
    }

    public void SFX(int _number)
    {
        if (!_myAudio.isPlaying)
            _myAudio.PlayOneShot(MapManager.instance._vfx[_number]);
    }

    IEnumerator KnockBack()
    {
        yield return _CorutinTime;
        Vector3 playerPos = MapManager.instance._player.transform.position;
        Vector3 dirVec = transform.position - playerPos;
        _rigid.AddForce(dirVec.normalized * Data._Resistance, ForceMode2D.Impulse);
    }

    public void HitDamage(float _dmg, bool _KnockBack)
    {
        _RecentHP -= _dmg;

        if (_RecentHP > 0)
        {
            _AniController.SetTrigger("Hit");
        }
        else
        {
            //SFX(0);
            _AniController.SetTrigger("Death");
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Weapon"))
            return;

        if (collision.GetComponent<SkillData>().Data1._SpecialAttack)
            return;

        HitDamage(collision.GetComponent<SkillData>().ResultDamage, true);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (!collision.CompareTag("Weapon"))
            return;

        if (!collision.GetComponent<SkillData>().Data1._SpecialAttack)
            return;

        if (_TempDurationTime <= 0)
        {
            HitDamage(collision.GetComponent<SkillData>().ResultDamage, false);
            _TempDurationTime = _durationTime;
        }
    }
}
