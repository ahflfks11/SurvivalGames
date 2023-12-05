using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterBullet : MonoBehaviour
{
    float _Speed = 3f;
    public int _Number = -1;

    private void Update()
    {

        switch (_Number)
        {
            case 0:
                //9시
                transform.Translate(new Vector2(-1, 0) * _Speed * Time.deltaTime, Space.World);
                break; 
            case 1:
                //11시
                transform.Translate(new Vector2(-1, 1) * _Speed * Time.deltaTime, Space.World);
                break;
            case 2:
                //12시
                transform.Translate(new Vector2(0, 1) * _Speed * Time.deltaTime, Space.World);
                break;
            case 3:
                //1시
                transform.Translate(new Vector2(1, 1) * _Speed * Time.deltaTime, Space.World);
                break;
            case 4:
                //3시
                transform.Translate(new Vector2(1, 0) * _Speed * Time.deltaTime, Space.World);
                break;
            case 5:
                //5시
                transform.Translate(new Vector2(1, -1) * _Speed * Time.deltaTime, Space.World);
                break;
            case 6:
                //6시
                transform.Translate(new Vector2(0, -1) * _Speed * Time.deltaTime, Space.World);
                break;
            case 7:
                transform.Translate(new Vector2(-1, -1) * _Speed * Time.deltaTime, Space.World);
                break;
        }
    }
}
