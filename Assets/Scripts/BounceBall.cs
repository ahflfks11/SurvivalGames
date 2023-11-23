using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BounceBall : MonoBehaviour
{
    [SerializeField][Range(500f, 1000f)]
    float speed = 500f;
    Rigidbody2D _rigid;

    // Start is called before the first frame update
    void Start()
    {
        _rigid = transform.GetComponent<Rigidbody2D>();
        float RandomX = Random.Range(-1f, 1f);
        float RandomY = Random.Range(-1f, 1f);
        Vector2 dir = new Vector2(RandomX, RandomY).normalized;
        _rigid.AddForce(dir * speed);
    }
    void OnCollisionEnter(Collision collision)
    {
        ExcecuteReBounding(collision);
    }


    void ExcecuteReBounding(Collision collision)
    {
        ContactPoint cp = collision.GetContact(0);
        Vector3 dir = transform.position - cp.point; // 접촉지점에서부터 탄위치 의 방향
        _rigid.AddForce((dir).normalized * 300f);
    }
}
