using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField]
    private Slider slider;

    [SerializeField]
    protected HealthSystem health;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        health.OnDamageTaken += UpdateSliderValue;
        health.OnDeath += Hide;
    }

    protected void UpdateSliderValue(int newHP)
    {
        slider.value = (float)newHP / (float)health.maxHP;
    }

    private void Hide(int hp) 
    {
        slider.gameObject.SetActive(false);
    }

}
