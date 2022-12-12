using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CloudOnce;
public class LeaderboardManager : MonoBehaviour
{
    public static LeaderboardManager instance;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void submitScores()
    {
        Leaderboards.HighscoreNoRespawns.SubmitScore((int)GlobalInfo.info.highScore);
        Leaderboards.HighscoreRespawn.SubmitScore((int)GlobalInfo.info.highScoreRespawn);
        Leaderboards.PlanetsReached.SubmitScore((int)GlobalInfo.info.planetsReachedOneGame);

    }
}
