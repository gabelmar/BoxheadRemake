using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyUI : MonoBehaviour
{
    [SerializeField]
    private Slider slider;

    [SerializeField]
    private Canvas canvas;

    private HealthSystem health;
    // Start is called before the first frame update
    void Start()
    {
        health = GetComponent<HealthSystem>();
        health.OnDamageTaken += UpdateSliderValue;
    }

    public void UpdateSliderValue(int newHP)
    {
        slider.value = (float)newHP / (float)health.maxHP;
    }
}
