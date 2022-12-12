using UnityEngine;
using UnityEngine.Advertisements;

public class initializeAds : MonoBehaviour
{
    [SerializeField] string IOS_ID;
    [SerializeField] string Android_ID;

    string ID;

    private void Awake()
    {
        if (!GameObject.FindWithTag("GameController").GetComponent<GameManager>().showAds)
        {
            Destroy(gameObject);
        }
        ID = (Application.platform == RuntimePlatform.IPhonePlayer)
            ? IOS_ID
            : Android_ID;

        Advertisement.Initialize(ID);
    }
}
