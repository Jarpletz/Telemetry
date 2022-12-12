using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class DeathUI : MonoBehaviour
{
    [SerializeField] GameObject deathScreen;

    [Header("Death Messages")]
    [SerializeField] string[] outOfBounds;
    [SerializeField] string[] outOfPower;
    [SerializeField] string[] hitPlanet;
    public enum deathCauses { outOfBounds, outOfPower, hitPlanet };
    public deathCauses causeOfDeath;
    [SerializeField] TextMeshProUGUI causeOfDeathText;

    [Header("Score")]
    [SerializeField] float percentOfScoreDisplayed;
    [SerializeField] string scorePrefix;
    [SerializeField] TextMeshProUGUI scoreText;
    float score;
    [Header("Highscore")]
    [SerializeField]float hsShakeDuration;
    [SerializeField]float hsShakeMagnitude;

    Animator animator;
    GameManager gm;
    screenShake shake;
    bool hasDied=false;
    float previousScoreDisplayed;

    private void Start()
    {
        animator = GetComponent<Animator>();
        shake = Camera.main.GetComponent<screenShake>();
        gm = GameObject.FindWithTag("GameController").GetComponent<GameManager>();

    }
    private void Update()
    {
        if (hasDied)
        {
            score= GameObject.FindWithTag("GameController").GetComponent<GameManager>().score *percentOfScoreDisplayed;
            if(percentOfScoreDisplayed==0) scoreText.text = scorePrefix;
            else scoreText.text = scorePrefix + score.ToString("0");

            if (Mathf.Round(score) > previousScoreDisplayed)
            {
                previousScoreDisplayed = Mathf.Round(score);
                GetComponent<soundManager>().playSound("Tick");
            }
        }
    }


    public void onDeath()
    {
        deathScreen.SetActive(true);
        hasDied = true;

        switch (causeOfDeath)
        {
            case deathCauses.outOfBounds:
                causeOfDeathText.text = outOfBounds[Random.Range(0,outOfBounds.Length)];
                break;
            case deathCauses.outOfPower:
                causeOfDeathText.text = outOfPower[Random.Range(0, outOfPower.Length)];
                break;
            case deathCauses.hitPlanet:
                causeOfDeathText.text = hitPlanet[Random.Range(0, hitPlanet.Length)];
                break;
        }

        GetComponent<MultiplierUI>().onDeath();//stop multiplier particles (if any)

        animator.SetBool("Dead", true);
        if (gm.score > GlobalInfo.info.highScore&& !gm.hasRespawned)
        {
            animator.SetBool("Highscore", true);
        }
        if(gm.hasRespawned && gm.score > GlobalInfo.info.highScoreRespawn)
        {
            animator.SetBool("RespawnHS", true);
        }

    }
    public void triggerRestartAnimation()
    {
        animator.SetBool("Restart", true);
    }
    
    public void onRespawn()
    {
        animator.SetBool("Dead", false);
    }

    public void restartGame()
    {
        Interstitial_Ad_Manager ads = GameObject.FindWithTag("Ads").GetComponent<Interstitial_Ad_Manager>();
        if (GlobalInfo.info.gamesPlayed > 0 && GlobalInfo.info.gamesPlayed %ads.adFrequency  == 0)
        {//every 5th time the restart button is pressed, play an ad.
            
            ads.ShowAd();
        }
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void highscoreScreenShake()
    {
        StartCoroutine(shake.ScreenShake(hsShakeDuration, hsShakeMagnitude));
    }
}
