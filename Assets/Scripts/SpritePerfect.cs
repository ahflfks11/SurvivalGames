using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpritePerfect : MonoBehaviour
{
    TitleManager _titleManager;

    // Start is called before the first frame update
    void Start()
    {
        if (_titleManager == null) _titleManager = GameObject.FindAnyObjectByType<TitleManager>();

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
        //안드로이드용
        if (Application.platform == RuntimePlatform.Android)
        {
            if (Input.touchCount > 0)
            {
                if (_titleManager != null)
                {
                    if (_titleManager.Start1)
                    {
                        _titleManager.SceneLoad(1);
                        _titleManager.Start1 = false;
                    }
                }
            }
        }
        else
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (_titleManager != null)
                {
                    if (_titleManager.Start1)
                    {
                        _titleManager.SceneLoad(1);
                        _titleManager.Start1 = false;
                    }
                }
            }
        }
    }
}
