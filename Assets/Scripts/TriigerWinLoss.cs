using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TriigerWinLoss : MonoBehaviour
{
    public GameObject WinLossScreen;
    public bool WinTrigger = false;
    public bool LoseTrigger = false;
    public string TriggerMessage = "Unspecified Trigger";
    private TMPro.TextMeshProUGUI txt;



    void Start()
    {
        //txt = GameObject.Find("Message").GetComponent<TMPro.TextMeshProUGUI>();
        txt = WinLossScreen.GetComponentInChildren<TMPro.TextMeshProUGUI>();
        WinLossScreen.SetActive(false);

        
    }

    private void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag("Player"))
        {
            if (WinTrigger)
            {
                txt.text = "You Win!";
            }
            else if (LoseTrigger)
            {
                txt.text = "You Drowned";
            }
            else
            {
                txt.text = TriggerMessage;
            }

            WinLossScreen.SetActive(true);
        }

    }
}
