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
    public float _Resistance; // ���׵�, �������� �� �и�.
    public int SpawnLevel;
    public int SpawnLimitLevel; //Ư�� ���� �̻���ʹ� ������ ���� �ʴ´�.
}
public class Monster : MonoBehaviour
{
    [SerializeField]
    Data _Data;

    float _RecentHP;

    Rigidbody2D _target;
    Rigidbody2D _rigid;
    Animator _AniController;
    WaitForFixedUpdate _CorutinTime;

    public Data Data { get => _Data; set => _Data = value; }

    private void Awake()
    {
        _CorutinTime = new WaitForFixedUpdate();
    }

    private void OnEnable()
    {
        _RecentHP = Data._HP + MapManager.instance.Min;
        _rigid = transform.GetComponent<Rigidbody2D>();
        _AniController = transform.GetComponent<Animator>();
        //Coin
        MF_AutoPool.InitializeSpawn(MapManager.instance.items[0].gameObject, 0, 0);
    }

    private void FixedUpdate()
    {
        if (_AniController.GetCurrentAnimatorStateInfo(0).IsName("Hit"))
        {
            return;
        }
        else if (_AniController.GetCurrentAnimatorStateInfo(0).IsName("Death"))
        {
            if (_AniController.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.95f)
            {
                MapManager.instance.MobManager[(int)Data._MonsterType].poolBlock.size -= 1;
                GameObject _Coin = MF_AutoPool.Spawn(MapManager.instance.items[0].gameObject, transform.position, Quaternion.identity);
                Destroy(this.gameObject);
            }
            else
                return;
        }

        Vector2 dirVec = MapManager.instance._player.Rigid.position - _rigid.position;
        Vector2 nextVec = dirVec.normalized * Data._Speed * Time.fixedDeltaTime;
        _rigid.MovePosition(_rigid.position + nextVec);
        _rigid.velocity = Vector2.zero;
    }

    private void LateUpdate()
    {
        if (MapManager.instance._player.Rigid.position.x > _rigid.position.x)
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
        else
            transform.rotation = Quaternion.Euler(new Vector3(0, -180f, 0));
    }

    IEnumerator KnockBack()
    {
        yield return _CorutinTime;
        Vector3 playerPos = MapManager.instance._player.transform.position;
        Vector3 dirVec = transform.position - playerPos;
        _rigid.AddForce(dirVec.normalized * Data._Resistance, ForceMode2D.Impulse);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Weapon"))
            return;

        _RecentHP -= collision.GetComponent<SkillData>().Data1._Damage;

        if (_RecentHP > 0)
        {
            _AniController.SetTrigger("Hit");
            StartCoroutine(KnockBack());
        }
        else
        {
            _AniController.SetTrigger("Death");
        }
    }
}
