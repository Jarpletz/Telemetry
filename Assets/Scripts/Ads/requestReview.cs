using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.iOS;

public class requestReview : MonoBehaviour
{
    GameManager gm;

    private void Start()
    {
#if UNITY_IOS && !UNITY_EDITOR
        gm = GameObject.FindWithTag("GameController").GetComponent<GameManager>();

        if ((GlobalInfo.info.gamesPlayed > 8 && !GlobalInfo.settings.hasRequestedReview && GlobalInfo.info.gamesPlayed % GetComponent<Interstitial_Ad_Manager>().adFrequency != 0 )|| GlobalInfo.info.gamesPlayed == 26) 
        {
            UnityEngine.iOS.Device.RequestStoreReview();
            GlobalInfo.settings.hasRequestedReview = true;
        }
#endif
    }

}
