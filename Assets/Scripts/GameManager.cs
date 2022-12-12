using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public float G;
    public float score;
    [Header("Platform")]
    public bool isTutorial = false;
    public bool isMobile = false;
    public bool showAds = false;

    [Header("Gameplay")]
    public bool startedGame = false;
    public bool canStartGame = true;
    public bool isDead = false;
    public bool orbitAssist = true;
    public bool rageMode = false;
    public bool hasRespawned = false;

    [Header("Multiplier Stats")]
    public float multiplier=1;
    public int multiplierPower=0;
    public float biggestCombo;
    public List<Color> muliplierColors = new List<Color>();

    [Header("Planet Stats")]
    public float planetsReached;
    public float numbPerfectOrbits;

    public List<PlanetBehavior> planets = new List<PlanetBehavior>();


    public void updateSavedData()
    {
        rageMode = !GlobalInfo.settings.lockOrbit;

        if (GlobalInfo.info == null) Debug.Log("Global Info Null");

        GlobalInfo.info.planetsReached += planetsReached;
        GlobalInfo.info.perfectOrbits += numbPerfectOrbits;
        GlobalInfo.info.gamesPlayed++;

        if (!rageMode )
        {
            if(!hasRespawned && score > GlobalInfo.info.highScore)
            {
                GlobalInfo.info.highScore = score;//set regular highscore
                if (score > GlobalInfo.info.highScoreRespawn)
                    GlobalInfo.info.highScoreRespawn = score;//if also greater than respawn highscore, set that too
            }
            else if(hasRespawned && score > GlobalInfo.info.highScoreRespawn)
            {
                 GlobalInfo.info.highScoreRespawn = score;//if also greater than respawn 
            }

        }else if(rageMode && score > GlobalInfo.info.highScoreHard)
        {
            GlobalInfo.info.highScoreHard = score;
        }
        if (biggestCombo > GlobalInfo.info.biggestCombo)
        {
            GlobalInfo.info.biggestCombo = biggestCombo;
        }
        if (planetsReached > GlobalInfo.info.planetsReachedOneGame)
        {
            GlobalInfo.info.planetsReachedOneGame = planetsReached;
        }
        GlobalInfo.saveStats();

        GetComponent<LeaderboardManager>().submitScores();
    }
}
