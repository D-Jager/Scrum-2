using UnityEngine;
using TMPro;
using UnityEditor.EditorTools;
using System;

public class TimerScript : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private bool Timer;
    [SerializeField] private float CountedTime;
    [SerializeField] private TMP_Text DisplayText;
    public GameObject TestPanel;
    void Start()
    {
        CountedTime = 0;
        Timer = true;
        TestPanel.SetActive(false);
    }


    void FixedUpdate()
    {
        if (Timer == true)
        {
            CountedTime = CountedTime + Time.deltaTime;
        }
        DisplayText.text = CountedTime.ToString();
        TimeSpan timing = TimeSpan.FromSeconds(CountedTime);
        DisplayText.text = timing.Minutes.ToString() + ":" + timing.Seconds.ToString() + ":" + timing.Milliseconds.ToString();
    }
    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Timer = false;
            Debug.Log("Collision");
            TestPanel.SetActive(true);
            Time.timeScale = 0f;
        }
    }
}
