using ElementalEngagement.Utilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class AnnouncerQueue : MonoBehaviour
{
    public static Queue<SoundEffectManager> announcersActive;

    private float currentTimeRemaining;
    private bool currentlyPlaying;

    private void Update()
    {
        if(!currentlyPlaying && announcersActive.Count > 0)
        {
            SoundEffectManager activeAnnouncer = announcersActive.Dequeue();
            float length = activeAnnouncer.PlayRandomSound();
            currentTimeRemaining = length;
        }
        else if(currentlyPlaying && currentTimeRemaining > 0)
        {
            currentTimeRemaining -= Time.deltaTime;
        }
        else
        {
            currentlyPlaying = false;
        }
    }

    public static void addAnnouncer(SoundEffectManager manager)
    {
        announcersActive.Enqueue(manager);
    }
}
