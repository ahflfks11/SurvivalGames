using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleManager : MonoBehaviour
{
    [SerializeField]
    bool _Start;
    public float _startWaitTime = 1.5f;
    public bool Start1 { get => _Start; set => _Start = value; }

    private void Start()
    {
        Time.timeScale = 1f;
        StartCoroutine(TitleInit());
    }

    public void SceneLoad(int number)
    {
        SceneManager.LoadScene(number);
    }

    public void StartGame()
    {
        if (Start1)
            SceneManager.LoadScene(1);
    }

    IEnumerator TitleInit()
    {
        yield return new WaitForSeconds(_startWaitTime);
        Start1 = true;
    }
}
