using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Random_Spawn : MonoBehaviour
{
    SkillData _data;
    public float _Min_RandomX, _Max_RandomX, _Min_RandomY, _Max_RandomY;

    // Start is called before the first frame update
    void Start()
    {
        _data = transform.GetComponent<SkillData>();
    }

    private void OnEnable()
    {
        transform.position = new Vector3(transform.position.x + Random.Range(_Min_RandomX, _Max_RandomX), transform.position.y + Random.Range(_Min_RandomY, _Max_RandomY), transform.position.z);
    }
}
