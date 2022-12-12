using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class UI_Manager : MonoBehaviour
{
    [SerializeField] float scoreLerpSpeed;
    public TextMeshProUGUI scoreText;
    [SerializeField] Slider powerSlider;
    [SerializeField] Gradient powerGradient;
    [SerializeField] Slider fuelSlider;
    [SerializeField] Gradient fuelGradient;
    [SerializeField] TextMeshProUGUI hsText;

    GameManager gm;
    RocketFuel rocketFuel;

    float displayedScore;

    private void Start()
    {
        gm = GameObject.FindWithTag("GameController").GetComponent<GameManager>();
        rocketFuel = GameObject.FindWithTag("Player").GetComponent<RocketFuel>();
        if (gm.isMobile) GetComponent<Animator>().SetBool("IsMobile", true);
        if (!GlobalInfo.settings.hasDoneTutorial)
        {
            GlobalInfo.settings.hasDoneTutorial = true;
            GlobalInfo.saveSettings();
            GetComponent<SceneLoader>().loadScene(1);
        }
    }

    private void Update()
    {
        //score text
        displayedScore = Mathf.Lerp(displayedScore, gm.score, scoreLerpSpeed * Time.deltaTime) ;
        scoreText.text =displayedScore.ToString("0");

        //slider colors
        fuelSlider.fillRect.GetComponent<Image>().color = fuelGradient.Evaluate(fuelSlider.value / 100f);
        powerSlider.fillRect.GetComponent<Image>().color = powerGradient.Evaluate(powerSlider.value / 100f);

        //slider values
        fuelSlider.value = rocketFuel.fuel;
        powerSlider.value = rocketFuel.power;

        //highscore text
        if (gm.score > GlobalInfo.info.highScore)
        {
            hsText.text = "HS: " + gm.score.ToString("0");
        }else hsText.text = "HS: " + GlobalInfo.info.highScore.ToString("0");
    }

}
