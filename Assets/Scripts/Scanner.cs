using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scanner : MonoBehaviour
{
    public float scanRange;
    public float itemScanRange;
    public float DropSpeed;
    float _tempCurTime = 0;
    float _CurTime = 3f;
    float _tempItemScanRange;
    public LayerMask targetLayer;
    public LayerMask bossLayer;
    public LayerMask itemLayer;
    public RaycastHit2D[] targets;
    public RaycastHit2D[] ItemScan;
    public Transform nearestTarget;

    private void OnEnable()
    {
        _tempItemScanRange = itemScanRange;
    }

    private void Update()
    {
        if (_tempCurTime > 0)
        {
            _tempCurTime -= Time.deltaTime;
        }
        else
        {
            itemScanRange = _tempItemScanRange;
        }
    }

    private void LateUpdate()
    {
        targets = Physics2D.CircleCastAll(transform.position, scanRange, Vector2.zero, 0, LayerMask.GetMask("Enermy", "Boss"));
        ItemScan = Physics2D.CircleCastAll(transform.position, itemScanRange, Vector2.zero, 0, itemLayer);
        nearestTarget = GetNearest();
        //if (ItemScan.Length != 0)
            //GetCoin();
    }

    public void MagnetUp(float _value)
    {
        _tempItemScanRange += _value;
    }

    public void Magnet()
    {
        _tempItemScanRange = itemScanRange;
        _tempCurTime = _CurTime;
        itemScanRange = 15f;
    }

    public void GetCoin()
    {
        foreach(RaycastHit2D item in ItemScan)
        {
            item.transform.position = Vector3.MoveTowards(item.transform.position, transform.position, DropSpeed * Time.deltaTime);
        }
    }

    Transform GetNearest()
    {
        Transform result = null;
        float diff = 100;

        foreach (RaycastHit2D target in targets)
        {
            Vector3 myPos = transform.position;
            Vector3 targetPos = target.transform.position;
            float curDiff = Vector3.Distance(myPos, targetPos);
            if (curDiff < diff)
            {
                diff = curDiff;
                result = target.transform;
            }
        }

        return result;
    }
}
