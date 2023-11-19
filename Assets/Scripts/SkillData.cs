using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WeaponType
{
    효과없음,
    관통,
    오른쪽공격,
    왼쪽공격,
    위로공격,
    아래로공격,
    갈래공격,
    세갈래공격
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
}

public class SkillData : MonoBehaviour
{
    [SerializeField]
    DB Data;

    public MapManager.WeaponType _WeaponType;
    Vector3 _dir = Vector3.zero;
    Rigidbody2D _rigid;
    Vector3 _WeaponDir;
    public DB Data1 { get => Data; set => Data = value; }

    public void DestoryPrefab()
    {
        MapManager.instance.WeaponManager[(int)_WeaponType].poolBlock.size -= 1;
        Destroy(this.gameObject);
    }

    private void OnEnable()
    {
        _rigid = transform.GetComponent<Rigidbody2D>();
        if (Data.WeaponType == WeaponType.관통 || Data.WeaponType == WeaponType.갈래공격 || Data.WeaponType == WeaponType.세갈래공격)
        {
            _dir = MapManager.instance._player.Scanner.nearestTarget.position;
            _WeaponDir = _dir - transform.position;
            _WeaponDir = _WeaponDir.normalized;

            transform.rotation = Quaternion.FromToRotation(Vector3.up, _WeaponDir);
            _rigid.velocity = _WeaponDir.normalized * Data.Power;
        }
        else if (Data.WeaponType == WeaponType.오른쪽공격)
        {
            _rigid.velocity = Vector2.right * Data.Power;
        }
        else if (Data.WeaponType == WeaponType.왼쪽공격)
        {
            _rigid.velocity = Vector2.left * Data.Power;
        }
        else if (Data.WeaponType == WeaponType.위로공격)
        {
            _rigid.velocity = Vector2.up * Data.Power;
        }
        else if (Data.WeaponType == WeaponType.아래로공격)
        {
            _rigid.velocity = Vector2.down * Data.Power;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Monster"))
            return;

        if (MapManager.instance.WeaponManager[(int)_WeaponType].poolBlock.size >= 1 && Data1.WeaponType == WeaponType.효과없음)
            DestoryPrefab();
    }
}
