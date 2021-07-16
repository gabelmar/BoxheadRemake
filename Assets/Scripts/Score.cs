using System;
using TMPro;
using UnityEngine;

public class Score : MonoBehaviour
{
    public float multiplierDecayTime = 3f;
    [SerializeField]
    private TextMeshProUGUI scoreText;
    [SerializeField]
    private TextMeshProUGUI multiplierText;
    [SerializeField]
    private int maxMultiplier = 20;

    private Color multiplierTextColor;

    private Vector3 defaultScale;

    private string multiplierTextString = "x{0}";

    private int score = 0;
    private int multiplier = 1;

    private float multiplierDecayCountdown;

    public static event Action<int> OnMultiplierIncreased;

    public int CurrentMultiplier => multiplier;
    // Start is called before the first frame update
    void Start()
    {
        scoreText.text = score.ToString();
        multiplierTextColor = multiplierText.color;
        defaultScale = multiplierText.transform.localScale;
        ResetMultiplierCountdown();
        UpdateMultiplierText();
        multiplierText.color = Color.white;                     // Start on White color for multiplier == 1
    }

    private void Update()
    {
        if (multiplier == 1)
            return;

        multiplierDecayCountdown -= Time.deltaTime;
        if (multiplierDecayCountdown < 0f)
            multiplierDecayCountdown = 0f;

        UpdateMultiplierTextColor();
        if (multiplierDecayCountdown == 0f) 
        {
            ResetMultiplierCountdown();
            multiplier--;
            UpdateMultiplierText();
        }
    }

    public void Increase(int amount) 
    {
        if (multiplier == maxMultiplier) 
        {
            ResetMultiplierCountdown();
            score += amount * multiplier;
            scoreText.text = score.ToString();
            return;
        } 
        multiplier++;
        score += amount * multiplier;
        scoreText.text = score.ToString();
        ResetMultiplierCountdown();
        OnMultiplierIncreased?.Invoke(multiplier);
        UpdateMultiplierText();
    }

    private void UpdateMultiplierText()
    {
        multiplierText.text = string.Format(multiplierTextString, multiplier);
        if(multiplier == 1)
            multiplierText.color = Color.white;
        else
            multiplierText.color = multiplierTextColor;
    }

    private void UpdateMultiplierTextColor() 
    {
        float percentage = 1 - (multiplierDecayCountdown / multiplierDecayTime);
        multiplierText.color = new Color(multiplierTextColor.r,
                                         multiplierTextColor.g + (multiplierTextColor.r - multiplierTextColor.g) * percentage,
                                         multiplierTextColor.b + (multiplierTextColor.r - multiplierTextColor.b) * percentage);

        float descendingPercentage = 1f - percentage;
        multiplierText.transform.localScale = defaultScale + Vector3.one*( 1 * descendingPercentage);
    }

    /// <summary>
    /// Calculate the new decay time of the multiplier countdown. The higher the multiplier, 
    /// the faster you loose it, ie. the shorter the decayCountdown time.
    /// Linear relation. multipliyer == 1 -> nothing gets subtracted from the decay time. 
    /// multiplier == maxMultiplier -> 99% (in case of maxMultiplier == 100) of the decay time get subtracted.
    /// </summary>
    private void ResetMultiplierCountdown() 
    {
        multiplierDecayCountdown = multiplierDecayTime - ((float)(multiplier - 1) / maxMultiplier * multiplierDecayTime);
    }
}
