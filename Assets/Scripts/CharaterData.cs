using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharaterData : MonoBehaviour
{
    Animator _AniController;
    PlayerController _playerController;

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
