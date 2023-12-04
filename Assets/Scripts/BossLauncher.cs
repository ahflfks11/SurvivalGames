using DG.Tweening;
using System;
using System.Collections;
using UnityEngine;

public class BossLauncher : MonoBehaviour
{
    [SerializeField]
    BossPattern.BossSkill[] _Pattern;

    [SerializeField]
    BossPattern.Pattern _NowPattern;
    SpriteRenderer _renderer;
    public string _Name;
    public float _Damage;
    public float Speed;
    public float _AttackRange;
    public float[] _CastCoolTime;
    Rigidbody2D _rigid;
    public Transform _TranPos;
    Animator _AniController;
    bool _IsPattern;
    bool _isSkilling;
    Coroutine _Corutine_BossPattern;
    Collider2D _myColider;
    float _durationTime = 0.2f;
    float _TempDurationTime = 0f;
    float _AttackSpeed = 1f;
    float _TempAttackSpeed;
    float _damage;
    [SerializeField]
    int _patternCount = -1;

    public float _RecentHP;
    WaitForFixedUpdate _CorutinTime;

    public float Damage { get => _damage; set => _damage = value; }

    private void Start()
    {
        _Pattern = MapManager.instance.BossList.Process_BossPattern(this.gameObject, _Name);
        _RecentHP = MapManager.instance.BossList.Boss_HP(this.gameObject, _Name);
        _damage = MapManager.instance.BossList.Boss_NormalDamage(this.gameObject, _Name);
        _CastCoolTime = new float[_Pattern.Length];
        _rigid = transform.GetComponent<Rigidbody2D>();
        _myColider = transform.GetComponent<Collider2D>();
        _AniController = transform.GetComponent<Animator>();
        _renderer = transform.GetComponent<SpriteRenderer>();
        _Corutine_BossPattern = StartCoroutine(PatternLauncher());
    }

    private void Update()
    {
        if (!_IsPattern)
        {
            _patternCount = MapManager.instance.BossList.Boss_PatternCount(this.gameObject, _Name, _RecentHP);

            for (int i = 0; i < _CastCoolTime.Length; i++)
            {
                if (_CastCoolTime[i] > 0)
                {
                    _CastCoolTime[i] -= Time.deltaTime;
                }
            }

            if (!_isSkilling)
            {
                int Rnd_Skill = UnityEngine.Random.Range(0, _patternCount + 1);
                Debug.Log(Rnd_Skill);
                if (_CastCoolTime[Rnd_Skill] <= 0)
                {
                    _isSkilling = true;
                    _NowPattern = (BossPattern.Pattern)Rnd_Skill;
                    _CastCoolTime[Rnd_Skill] = _Pattern[Rnd_Skill]._SkillCooltime;
                }
            }

            if (Vector3.Distance(MapManager.instance._player.transform.position, _TranPos.transform.position) <= _AttackRange)
            {
                _AniController.SetBool("Run", false);
                if (_AniController.GetCurrentAnimatorStateInfo(0).IsName("Idle") && _TempAttackSpeed <= 0)
                {
                    _AniController.SetTrigger("Attack");
                    _TempAttackSpeed = _AttackSpeed;
                }

               //_rigid.MovePosition(Vector2.zero);
            }
            else
            {
                if (_AniController.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
                {
                    _rigid.velocity = Vector2.zero;
                    return;
                }
                _AniController.SetBool("Run", true);

                Vector2 dirVec = MapManager.instance._player.transform.position - _TranPos.position;
                Vector2 nextVec = dirVec.normalized * Speed * Time.fixedDeltaTime;

                _rigid.MovePosition(_rigid.position + nextVec);
                _rigid.velocity = Vector2.zero;
            }
        }
    }

    private void LateUpdate()   
    {
        if (MapManager.instance._player.transform.position.x > _TranPos.position.x)
            _renderer.flipX = false;
        else
            _renderer.flipX = true;
    }

    private void FixedUpdate()
    {
        Math.Truncate(_RecentHP);
        if (_TempDurationTime > 0) _TempDurationTime -= Time.fixedDeltaTime;
        if (_TempAttackSpeed > 0) _TempAttackSpeed -= Time.deltaTime;

        if (_AniController.GetCurrentAnimatorStateInfo(0).IsName("Death"))
        {
            if (_AniController.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.95f)
            {
                Destroy(this.gameObject);
            }
        }
    }

    IEnumerator PatternLauncher()
    {
        while (true)
        {
            if (_isSkilling)
            {
                Debug.Log(_NowPattern);

                switch (_NowPattern)
                {
                    case BossPattern.Pattern.None:
                        yield return new WaitForSeconds(_Pattern[_patternCount].waitingTime);
                        _IsPattern = false;
                        _isSkilling = false;
                        break;
                    case BossPattern.Pattern.Teleporting:
                        _AniController.SetBool("Run", false);
                        _IsPattern = true;
                        _AniController.SetTrigger("Teleport");
                        _myColider.enabled = false;
                        yield return new WaitForSeconds(_Pattern[_patternCount].waitingTime);
                        Vector3 _dir = transform.position - _TranPos.position;
                        transform.position = MapManager.instance._player.transform.position + _dir;
                        _myColider.enabled = true;
                        _AniController.SetTrigger("Teleport_Attack");
                        yield return new WaitForSeconds(_Pattern[_patternCount]._durationTime);
                        _IsPattern = false;
                        _isSkilling = false;
                        Debug.Log(_NowPattern);
                        break;
                    case BossPattern.Pattern.Magic:
                        _AniController.SetBool("Run", false);
                        _IsPattern = true;
                        _AniController.SetBool("Pattern1_Ready", true);
                        yield return new WaitForSeconds(_Pattern[_patternCount].waitingTime);
                        _AniController.SetBool("Pattern1_Ready", false);
                        yield return new WaitForSeconds(_Pattern[_patternCount]._durationTime);
                        _IsPattern = false;
                        _isSkilling = false;
                        break;
                }
            }

            yield return null;
        }
    }

    public void HitDamage(float _dmg, bool _KnockBack)
    {
        _RecentHP -= _dmg;

        if (_RecentHP <= 0)
        {
            _AniController.SetTrigger("Death");
        }
    }

    public void StartPattern()
    {
        _Corutine_BossPattern = StartCoroutine(BossPatternLauncher(3f));
    }

    IEnumerator BossPatternLauncher(float _time)
    {
        _IsPattern = true;
        yield return new WaitForSeconds(_time);
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
