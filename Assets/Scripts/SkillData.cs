using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillData : MonoBehaviour
{
    public enum WeaponType
    {
        ȿ������ = 0,
        ����,
        �����ʰ���,
        ���ʰ���,
        ���ΰ���,
        �Ʒ��ΰ���,
        ��������,
        ����������,
        ���ڸ�_�ٴܰ���,
        ���ڸ�_Ư������
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
        public LayerMask targetLayer;
    }

    [SerializeField]
    DB Data;
    float _dmg = -1f;
    public MapManager.WeaponType _WeaponType;
    [SerializeField]
    int _level = 0;
    [SerializeField]
    bool _Passive;
    Vector3 _dir = Vector3.zero;
    Rigidbody2D _rigid;
    BoxCollider2D _col;
    Vector2 _WeaponDir;
    Vector3 _myScale;
    RaycastHit2D[] _targets;
    public DB Data1 { get => Data; set => Data = value; }
    public float Dmg { get => _dmg; set => _dmg = value; }
    public int Level { get => _level; set => _level = value; }
    public bool Passive { get => _Passive; set => _Passive = value; }

    public void DestoryPrefab()
    {
        MapManager.instance.WeaponManager[(int)_WeaponType].poolBlock.size -= 1;
        Destroy(this.gameObject);
    }

    private void Update()
    {
        for (int i = 0; i < MapManager.instance._MySkill.Count; i++)
        {
            if (MapManager.instance._MySkill[i]._weapon == _WeaponType)
            {
                _level = MapManager.instance._MySkill[i].currectLevel;
                Data._Damage = MapManager.instance._MySkill[i]._skills[MapManager.instance._MySkill[i].currectLevel]._Damage + MapManager.instance.CommonDamage;
                break;
            }
        }

        if (_Passive) return;

        transform.localScale = new Vector3(_myScale.x + MapManager.instance.MagicSize, _myScale.y + MapManager.instance.MagicSize, _myScale.z + MapManager.instance.MagicSize);
    }

    private void OnEnable()
    {
        if (_Passive) return;
        _myScale = transform.localScale;
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
            transform.SetParent(MapManager.instance._player.transform);
            transform.localPosition = Vector3.zero;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Monster") || _Passive)
            return;

        if (MapManager.instance.WeaponManager[(int)_WeaponType].poolBlock.size >= 1 && Data1.WeaponType == WeaponType.ȿ������)
            DestoryPrefab();
    }
}
