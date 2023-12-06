using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpritePerfect : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        SpriteRenderer spriteRenderer = gameObject.GetComponent<SpriteRenderer>();

        if (spriteRenderer == null)
            return;

        gameObject.transform.localScale = new Vector3(1, 1, 1);

        float _width = spriteRenderer.bounds.size.x;
        float _height = spriteRenderer.bounds.size.y;

        float worldScreenHeight = (float)(Camera.main.orthographicSize * 2.0);
        float worldScreenWidth = worldScreenHeight / Screen.height * Screen.width;


        gameObject.transform.localScale = new Vector3(worldScreenWidth / _width, worldScreenHeight / _height, 1);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
