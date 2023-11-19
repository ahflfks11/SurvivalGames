using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
public static class Extensions
{
    public static T[] RemoveAt<T>(this T[] source, int index)
    {
        return source.Where((_, i) => i != index).ToArray();
    }
}

public class PlayerController : MonoBehaviour
{
    VariableJoystick _Joystick;
    float _posX;
    float _posY;
    [SerializeField][Range(0,30)]
    float _speed;
    Vector2 moveDir;

    Scanner _scanner;

    Rigidbody2D _rigid;
    public Transform[] _SpawnPoint;
    
    public Vector2 MoveDir { get => moveDir; set => moveDir = value; }
    public float PosX { get => _posX; set => _posX = value; }
    public Scanner Scanner { get => _scanner; set => _scanner = value; }
    public Rigidbody2D Rigid { get => _rigid; set => _rigid = value; }

    // Start is called before the first frame update
    void Start()
    {
        _Joystick = GameObject.FindAnyObjectByType<VariableJoystick>();
        _scanner = GameObject.FindAnyObjectByType<Scanner>();
        _rigid = transform.GetComponentInChildren<Rigidbody2D>();
        _SpawnPoint = GameObject.Find("Spawner").GetComponentsInChildren<Transform>();
        _SpawnPoint = _SpawnPoint.RemoveAt<Transform>(0);
    }

    // Update is called once per frame
    void Update()
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            PosX = _Joystick.Horizontal;
            _posY = _Joystick.Vertical;
        }
        else
        {
            _posX = Input.GetAxis("Horizontal");
            _posY = Input.GetAxis("Vertical");
        }


        MoveDir = new Vector2(PosX, _posY);

        if (MoveDir.sqrMagnitude != 0)
        {
            transform.Translate(MoveDir * _speed * Time.deltaTime, Space.World);
        }
    }
}
