using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeRotation : MonoBehaviour
{
    [SerializeField]
    SpriteRenderer[] _spriteRenderer;
    BoxCollider2D _col;
    private void Start()
    {
        _spriteRenderer = transform.GetComponentsInChildren<SpriteRenderer>();
        _col = transform.GetComponent<BoxCollider2D>();
    }

    private void OnEnable()
    {
        
    }

    private void Update()
    {
        if (MapManager.instance._player.CharacterData.IsRight)
        {
            for (int i = 0; i < _spriteRenderer.Length; i++)
            {
                if (i > 0)
                {
                    _spriteRenderer[i].transform.localPosition = Vector3.right;
                    _col.offset = new Vector2(1.474852f, 0.06667602f);
                    _col.size = new Vector2(2.996877f, 4.495024f);
                    _spriteRenderer[i].flipX = true;
                    
                }
            }
        }
        else
        {
            for (int i = 0; i < _spriteRenderer.Length; i++)
            {
                if (i > 0)
                {
                    _spriteRenderer[i].transform.localPosition = Vector3.left;
                    _col.offset = new Vector2(-0.712515f, 0.8773994f);
                    _col.size = new Vector2(1.514283f, 2.407949f);
                    _spriteRenderer[i].flipX = false;
                }
            }
        }
    }
}
