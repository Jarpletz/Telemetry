using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StatsUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI statsText;
    [SerializeField] TextMeshProUGUI statsNameText;

    string text;
    Animator animator;
    GameManager gm;

    private void Start()
    {
        animator = GetComponent<Animator>();
        gm = GameObject.FindWithTag("GameController").GetComponent<GameManager>();
        refreshStats();
    }
    private void Update()
    {
        statsText.fontSize = statsNameText.fontSize;
    }
    void refreshStats()
    {
        text = "";
        //set values for stats screen. MUST be in this order.

        text += GlobalInfo.info.highScore.ToString("0") + '\n';      //highscore
        text += GlobalInfo.info.highScoreOld.ToString("0") + '\n';   //highscore old system
        text += GlobalInfo.info.highScoreHard.ToString("0") + '\n';  //highscore rage mode
        text += GlobalInfo.info.gamesPlayed.ToString("0") + '\n';    //games played

        text += '\n';

        text += GlobalInfo.info.planetsReached.ToString("0") + '\n'; //planets reached
        text += GlobalInfo.info.planetsReachedOneGame.ToString("0") + '\n'; //planets reached one game
        text += GlobalInfo.info.perfectOrbits.ToString("0") + '\n';  //perfect orbits
        text += "x "+ GlobalInfo.info.biggestCombo.ToString("0") + '\n';   //biggest combo

        text += '\n';

        text += GlobalInfo.info.crashes.ToString("0") + '\n';        //crashes
        text += GlobalInfo.info.powerOutages.ToString("0") + '\n';   //power outages

        statsText.text = text;
    }
    public void showStats()
    {
        animator.SetBool("ShowStats", true);
        gm.canStartGame = false;
           }
    public void showSettings()
    {
        animator.SetBool("ShowSettings", true);
        gm.canStartGame = false;

    }
    public void showTutorial()
    {
        animator.SetBool("Tutorial", true);
    }
    public void showCredits()
    {
        animator.SetBool("ShowCredits", true);
        gm.canStartGame = false;
    }
    public void hideStats()
    {
        animator.SetBool("ShowStats", false);
        Invoke("ResetCanStartGame", 0.5f);
    }
    public void hideSettings()
    {
        animator.SetBool("ShowSettings", false);
        Invoke("ResetCanStartGame", 0.5f);
    }
    public void hideCredits()
    {
        animator.SetBool("ShowCredits", false);
        Invoke("ResetCanStartGame", 0.5f);
    }
    public void resetStats()
    {
        GlobalInfo.info = new GlobalInfo.Info();
        GlobalInfo.saveStats();
        refreshStats();
    }

    private void ResetCanStartGame()
    {
        gm.canStartGame = true;
    }
}
