using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ground_Item : MonoBehaviour
{
    public enum Item_Type
    {
        Healing = 0,
        Magnet
    }

    public Item_Type Data;
    public float _dmg;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player"))
            return;

        switch (Data)
        {
            case Item_Type.Healing:
                MapManager.instance.SpawnEffects(collision.gameObject, 0);
                collision.GetComponent<CharaterData>().Healing(_dmg);
                break;
            case Item_Type.Magnet:
                MapManager.instance._player.Scanner.Magnet();
                break;
        }

        GameObject[] ItemPool = GameObject.FindGameObjectsWithTag("Item_Pool");

        for (int i = 0; i < ItemPool.Length; i++)
        {
            if (this.gameObject.name.Contains(ItemPool[i].GetComponent<AP_Pool>().poolBlock.prefab.gameObject.name))
            {
                if (ItemPool[i].GetComponent<AP_Pool>().poolBlock.size > 0)
                    ItemPool[i].GetComponent<AP_Pool>().poolBlock.size -= 1;

                break;
            }
        }

        Destroy(this.gameObject);
    }
}
