using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class tipText : MonoBehaviour
{
    [SerializeField] List<string> tips = new List<string>();

    // Start is called before the first frame update
    void Start()
    {
        string tipUsed = tips[Random.Range(0, tips.Count)];
        GetComponent<TextMeshProUGUI>().text = tipUsed;
    }
}
