using ElementalEngagement.Combat;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;


namespace ElementalEngagement.Player
{
    /// <summary>
    /// Responsible for handling ability related input, spawning ability effect, setting their aliment (if applicable), and handling ability cooldowns.
    /// </summary>
    [RequireComponent(typeof(Allegiance))]
    public class AbilityManager : MonoBehaviour
    {
        [Tooltip("Called when the selected ability changes.")]
        public UnityEvent<int> onSelectedAbilityChanged;

        [Tooltip("Called when an ability was played. Passes the ability index")]
        public UnityEvent<int> onAbilityPlayed;

        [Tooltip("Called when an ability comes off cooldown. Passes the ability index.")]
        public UnityEvent<int> onAbilityCooldownExpired;

        // The currently selected ability
        public int selectedAbilityIndex { get; private set; }


        // The current abilities' remaining cooldown times in seconds.
        public ReadOnlyDictionary<Ability, float> abilityCooldowns;
    }
}