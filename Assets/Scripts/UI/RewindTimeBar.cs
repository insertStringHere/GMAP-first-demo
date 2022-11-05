using Palmmedia.ReportGenerator.Core;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// A simple script to synchronize the amount of time the player has
/// left to use in the current <see cref="ZController"/> with an indication
/// bar at the top of the screen.
/// </summary>
public class RewindTimeBar : MonoBehaviour
{
    /// <summary>
    /// The <see cref="ZController"/> that the bar is working off of.
    /// </summary>
    public ZController zc;
    
    /// <summary>
    /// The backdrop image and mask of the bar.
    /// </summary>
    [SerializeField] private Image backgroundBar;
    /// <summary>
    /// The actual progress bar itself.
    /// </summary>
    [SerializeField] private Image progressBar;

    /// <summary>
    /// Attempt to set <see cref="zc"/>, <see cref="backgroundBar"/>, and
    /// <see cref="progressBar"/> from the scene if they have not been set
    /// already.
    /// </summary>
    void Start()
    {
        zc = zc == null ? FindObjectOfType<ZController>() : zc;
        backgroundBar = backgroundBar == null ? GetComponentsInChildren<Image>().FirstOrDefault(i => i.name == "Background") : backgroundBar;
        progressBar = progressBar == null ? GetComponentsInChildren<Image>().FirstOrDefault(i => i.name == "Bar") : progressBar;
    }

    /// <summary>
    /// Sets the location of the top right corner of the <see cref="progressBar"/> based on the percentage
    /// of time remaining within the <see cref="ZController"/> for any given level. Hides the bars if <see cref="ZController.timeAllowance"/>
    /// is set to zero.
    /// </summary>
    void Update()
    {
        if (zc.timeAllowance == 0) {
            backgroundBar.enabled = progressBar.enabled = false;
        } else {
            backgroundBar.enabled = progressBar.enabled = true;

            float timePercentage = 1 - zc.timeRemaining / zc.timeAllowance;

            progressBar.rectTransform.localPosition = new Vector3(((backgroundBar.rectTransform.offsetMax.x - backgroundBar.rectTransform.offsetMin.x) * timePercentage), 0, 0); 

        }
            
    }
}
