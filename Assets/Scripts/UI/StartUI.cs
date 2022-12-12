using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class StartUI : MonoBehaviour
{
    GameManager gm;
    private void Start()
    {
        gm = GameObject.FindWithTag("GameController").GetComponent<GameManager>();
    }

    public void startGame()
    {
        gm.startedGame = true;
        GetComponent<Animator>().SetBool("hasStarted", true);
    }
}
