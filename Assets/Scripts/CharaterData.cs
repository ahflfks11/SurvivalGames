using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct CharacterStat
{
    public string _ChracterName;
    public float Health;
    [Range(0, 20)]
    public float Speed;
}

public class CharaterData : MonoBehaviour
{
    [SerializeField]
    private CharacterStat _Stat;
    Animator _AniController;
    PlayerController _playerController;

    public CharacterStat Stat { get => _Stat; set => _Stat = value; }

    // Start is called before the first frame update
    void Start()
    {
        _AniController = transform.GetComponent<Animator>();
        _playerController = transform.GetComponentInParent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (_playerController.MoveDir.sqrMagnitude != 0)
        {
            if (_playerController.PosX < 0)
            {
                transform.rotation = Quaternion.Euler(0, -180, 0);
            }
            else
            {
                transform.rotation = Quaternion.Euler(0, 0, 0);
            }

            _AniController.SetBool("IsWalking", true);
        }
        else
        {
            _AniController.SetBool("IsWalking", false);
        }
    }
}
