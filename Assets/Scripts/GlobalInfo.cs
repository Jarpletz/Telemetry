using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class GlobalInfo : MonoBehaviour
{
    [System.Serializable]
     public class Info{
         public double highScore;
         public double highScoreOld;
         public double highScoreHard;
         public double highScoreRespawn;
         public float gamesPlayed;
         public float planetsReached;
         public float planetsReachedOneGame;
         public float perfectOrbits;
         public float crashes;
         public float powerOutages;
         public float biggestCombo;
        public Info()
        {
            highScore = 0;
            highScoreOld = 0;
            highScoreHard = 0;
            planetsReached = 0;
            planetsReachedOneGame = 0;
            perfectOrbits = 0;
            crashes = 0;
            powerOutages = 0;
            gamesPlayed = 0;
            biggestCombo = 0;
        }
    }
     static public Info info= new Info();

    [System.Serializable]
     public class Settings
    {
        public float sfxVolume;
        public float musicVolume;
        public bool predictTrajectory;
        public bool lockOrbit;
        public bool hasDoneTutorial;
        public bool hasRequestedReview;
        public Settings()
        {
            sfxVolume = 1;
            musicVolume = 1;
            predictTrajectory = true;
            lockOrbit = true;
            hasDoneTutorial = false;
            hasRequestedReview = false;
        }
    }
     static public Settings settings = new Settings();
    private void Awake()
    {

        GameObject[] managers = GameObject.FindGameObjectsWithTag("Global Manager");
        if (managers.Length > 1)
        {
            Destroy(this.gameObject);
        }
        DontDestroyOnLoad(this.gameObject);
        loadStats();
        loadSettings();
        if (info == null)
        {
            info = new Info();
        }
        else if (info.highScoreOld == 0 && info.planetsReachedOneGame == 0)
        {
            info.highScoreOld = info.highScore;
            if (info.biggestCombo > 4)
            {
                info.highScore = 0;
                info.biggestCombo = 1;
            }
        }//code to switch HS data over to new system
    }

    static public void saveStats()
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/data.fun";
        FileStream stream = new FileStream(path, FileMode.Create);
        formatter.Serialize(stream, info);
        stream.Close();
    }
    public void loadStats()
    {
        string path = Application.persistentDataPath + "/data.fun";
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);
            info = formatter.Deserialize(stream) as Info;
            stream.Close();
        }
        else
        {
            Debug.LogError("Save file not found in " + path);
            return;
        }
    }

    static public void saveSettings()
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/settings.fun";
        FileStream stream = new FileStream(path, FileMode.Create);
        formatter.Serialize(stream, settings);
        stream.Close();
    }
    public void loadSettings()
    {
        string path = Application.persistentDataPath + "/settings.fun";
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);
            settings = formatter.Deserialize(stream) as Settings;
            stream.Close();
        }
        else
        {
            Debug.LogError("Save file not found in " + path);
            return;
        }
    }
}
