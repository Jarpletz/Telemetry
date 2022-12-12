using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class MultiplierUI : MonoBehaviour
{
    [SerializeField] float decreaseRate;
    [SerializeField] RectTransform sliderBar;
    [SerializeField] Image sliderImage;
    [SerializeField] TextMeshProUGUI text;
    [SerializeField] ParticleSystem multiplierParticles;

    int multiplierDisplayed;
    float barValue=1;

    GameManager gm;

    private void Start()
    {
        gm = GameObject.FindWithTag("GameController").GetComponent<GameManager>();
        multiplierDisplayed = (int)gm.multiplier;
        endMultiplier();
    }
    private void Update()
    {
        if (gm.multiplier > gm.biggestCombo) gm.biggestCombo = gm.multiplier;

        if (gm.multiplier <= 1)
        {
            gm.multiplier = 1;
    
            return;
        }

        sliderBar.anchorMax =new Vector2( barValue / 2 + 0.5f,1);
        sliderBar.anchorMin = new Vector2(0.5f-barValue/2, 0);
        barValue -= Time.deltaTime*decreaseRate;

        if (barValue <= 0)
        {
            endMultiplier();
            return;
        }

        if (gm.multiplier > multiplierDisplayed)
        {
            multiplierDisplayed = (int)gm.multiplier;
            ChangeValues();
        }


    }

    void ChangeValues()
    {
        text.enabled = true;
        sliderImage.enabled = true;

        while (gm.multiplierPower - 1 >= gm.muliplierColors.Count)
        {
            gm.muliplierColors.Add(Random.ColorHSV(0f, 1f, 0.6f, 1f, 0.9f, 1f));
        }

        sliderImage.color = gm.muliplierColors[gm.multiplierPower - 1];
        text.color= gm.muliplierColors[gm.multiplierPower - 1];


         var main = multiplierParticles.main;
         main.startColor = gm.muliplierColors[gm.multiplierPower - 1];
         main.startColor = gm.muliplierColors[gm.multiplierPower - 1];
        // Debug.Log("HI");


        barValue = 1;
        text.text = "x " + multiplierDisplayed.ToString("0");
        if(gm.multiplierPower > 1) multiplierParticles.Play();
        else multiplierParticles.Stop();

    }
    void endMultiplier()
    {
        gm.multiplierPower = 0;
        gm.multiplier = 1;
        multiplierDisplayed = 1;
        text.enabled = false;
        sliderImage.enabled = false;
        multiplierParticles.Stop();
        barValue = 1;
    }
    public void onDeath()
    {
        multiplierParticles.Stop();
    }
}
