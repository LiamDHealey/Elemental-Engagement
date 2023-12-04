using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayDeathAnimation : MonoBehaviour
{

    [SerializeField] private Animator playDeath;

    public void PlayDeath()
    {
        playDeath = GetComponent<Animator>();
        playDeath.SetBool("IsDead", true);
    }
}
