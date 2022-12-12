using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialScreenTransition : MonoBehaviour
{
    
    public void nextScreen(int screenID)
    {
        GetComponent<Animator>().SetInteger("Screen", screenID);
    }
}
