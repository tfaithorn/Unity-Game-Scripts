using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LoadingScreen : MonoBehaviour
{
    [SerializeField]
    private Slider progressBar;

    [SerializeField]
    private TextMeshProUGUI tipOfTheDay;

    void Start()
    {
        DontDestroyOnLoad(transform.gameObject);
    }

    public void SetProgress(float value)
    {
        progressBar.value = value;
    }

    public void Init()
    {
        Time.timeScale = 0;
    }

    public void Finish()
    {
        Time.timeScale = 1;
    }
}
