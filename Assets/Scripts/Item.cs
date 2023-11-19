using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Item : MonoBehaviour
{
    public MapManager.ItemType _ItemType;
    public float _exp;
    public void GetCoin()
    {
        if(Vector3.Distance(transform.position, MapManager.instance._player.transform.position) <= 0.1f)
        {
            if (MapManager.instance.ItemList[(int)_ItemType].poolBlock.size >= 1)
                MapManager.instance.ItemList[(int)_ItemType].poolBlock.size -= 1;
            MapManager.instance._player._RecentEXP += _exp;
            Destroy(this.gameObject);
            return;
        }

        if (Vector3.Distance(transform.position, MapManager.instance._player.transform.position) <= MapManager.instance._player.Scanner.itemScanRange)
        {
            transform.position = Vector3.MoveTowards(transform.position, MapManager.instance._player.transform.position, MapManager.instance._player.Scanner.DropSpeed * Time.deltaTime);
        }
    }

    private void Update()
    {
        GetCoin();
    }
}
