using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Item : MonoBehaviour
{
    public MapManager.ItemType _ItemType;

    public void GetCoin()
    {
        if(Vector3.Distance(transform.position, MapManager.instance._player.transform.position) <= 0.01f)
        {
            if (MapManager.instance.ItemList[(int)_ItemType].poolBlock.size >= 1)
                MapManager.instance.ItemList[(int)_ItemType].poolBlock.size -= 1;
            Destroy(this.gameObject);
            return;
        }

        if (Vector3.Distance(transform.position, MapManager.instance._player.transform.position) <= MapManager.instance._player.Scanner.itemScanRange)
        {
            transform.position = Vector3.MoveTowards(transform.position, MapManager.instance._player.transform.position, MapManager.instance._player.Scanner.DropSpeed * Time.deltaTime);
        }
    }

    private void LateUpdate()
    {
        GetCoin();
    }
}
