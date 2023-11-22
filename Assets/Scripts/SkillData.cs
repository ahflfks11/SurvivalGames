using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WeaponType
{
    ȿ������,
    ����,
    �����ʰ���,
    ���ʰ���,
    ���ΰ���,
    �Ʒ��ΰ���,
    ��������,
    ����������,
    ���ڸ�_�ٴܰ���
};

[System.Serializable]
public struct DB
{
    public WeaponType WeaponType;
    public float _Damage;
    [Range(0.1f, 20f)]
    public float Speed;
    [Range(1f, 10f)]
    public float Power;
    public bool _SpecialAttack;
    public float _attackRange;
    public LayerMask targetLayer;
}

public class SkillData : MonoBehaviour
{
    [SerializeField]
    DB Data;

    public MapManager.WeaponType _WeaponType;
    Vector3 _dir = Vector3.zero;
    Rigidbody2D _rigid;
    BoxCollider2D _col;
    Vector2 _WeaponDir;
    RaycastHit2D[] _targets;
    public DB Data1 { get => Data; set => Data = value; }

    public void DestoryPrefab()
    {
        MapManager.instance.WeaponManager[(int)_WeaponType].poolBlock.size -= 1;
        Destroy(this.gameObject);
    }

    private void OnEnable()
    {
        _rigid = transform.GetComponent<Rigidbody2D>();
        _col = transform.GetComponent<BoxCollider2D>();
        if (Data.WeaponType == WeaponType.���� || Data.WeaponType == WeaponType.�������� || Data.WeaponType == WeaponType.����������)
        {
            _dir = MapManager.instance._player.Scanner.nearestTarget.position;
            _WeaponDir = _dir - transform.position;
            _WeaponDir = _WeaponDir.normalized;

            transform.rotation = Quaternion.FromToRotation(Vector2.right, _WeaponDir);
            //transform.LookAt(_WeaponDir);
            _rigid.velocity = _WeaponDir.normalized * Data.Power;
        }
        else if (Data.WeaponType == WeaponType.�����ʰ���)
        {
            _rigid.velocity = Vector2.right * Data.Power;
        }
        else if (Data.WeaponType == WeaponType.���ʰ���)
        {
            _rigid.velocity = Vector2.left * Data.Power;
        }
        else if (Data.WeaponType == WeaponType.���ΰ���)
        {
            _rigid.velocity = Vector2.up * Data.Power;
        }
        else if (Data.WeaponType == WeaponType.�Ʒ��ΰ���)
        {
            _rigid.velocity = Vector2.down * Data.Power;
        }
        else if (Data.WeaponType == WeaponType.���ڸ�_�ٴܰ���)
        {
            StartCoroutine(Launcher());
            transform.SetParent(MapManager.instance._player.transform);
            transform.localPosition = Vector3.zero;
        }
    }

    IEnumerator Launcher()
    {
        while (true)
        {
            _targets = Physics2D.CircleCastAll(transform.position, Data._attackRange, Vector2.zero, 0, Data.targetLayer);
            for (int i = 0; i < _targets.Length; i++)
            {
                _targets[i].collider.GetComponent<Monster>().HitDamage(Data._Damage, false);
            }
            yield return new WaitForSeconds(0.2f);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Monster"))
            return;

        if (MapManager.instance.WeaponManager[(int)_WeaponType].poolBlock.size >= 1 && Data1.WeaponType == WeaponType.ȿ������)
            DestoryPrefab();
    }
}
