using ElementalEngagement.Combat;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

namespace ElementalEngagement.Favor
{
    /// <summary>
    /// Tracks the favor of all minor god for all player, and how that favor unlocks new things.
    /// </summary>
    public class FavorManager : MonoBehaviour
    {
        [Tooltip("Called when the favor changes for a player.")]
        public static UnityEvent<Player.Faction, MinorGod> onFavorChanged = new UnityEvent<Player.Faction, MinorGod>();


        [Tooltip("How each god's favor unlocks things.")]
        [SerializeField] private FavorProgressionSettings _progressionSettings;
        public static ReadOnlyDictionary<MinorGod, FavorProgressionSettings.GodProgressionSettings> progressionSettings { get => instance._progressionSettings.godProgressionSettings; }

        // The abilities that are currently unlocked.
        public static ReadOnlyCollection<Ability> unlockedAbilities { get => instance.tempUnlockedAbilities.AsReadOnly(); }
        // TODO: replace this with an actual implementation based off of progression settings and favor
        public List<Ability> tempUnlockedAbilities;

        // Stores the favor each god shows towards each player faction.
        [Tooltip("How much favor each god has for each player's faction.")]
        [SerializeField] private Dictionary<Tuple<Player.Faction, MinorGod>, float> _factionToFavor = new Dictionary<Tuple<Player.Faction, MinorGod>, float>();
        public static ReadOnlyDictionary<Tuple<Player.Faction, MinorGod>, float> factionToFavor { get => new ReadOnlyDictionary<Tuple<Player.Faction, MinorGod>,float>(instance._factionToFavor); }


        // Tracks the singleton instance of this.
        private static FavorManager instance;



        /// <summary>
        /// Initializes singleton.
        /// </summary>
        private void Awake()
        {
            instance = this;
            Tuple<Player.Faction, MinorGod> p1fire = new Tuple<Player.Faction, MinorGod>(Player.Faction.PlayerOne, MinorGod.Fire);
            Tuple<Player.Faction, MinorGod> p1water = new Tuple<Player.Faction, MinorGod>(Player.Faction.PlayerOne, MinorGod.Water);
            Tuple<Player.Faction, MinorGod> p1earth = new Tuple<Player.Faction, MinorGod>(Player.Faction.PlayerOne, MinorGod.Earth);
            Tuple<Player.Faction, MinorGod> p1human= new Tuple<Player.Faction, MinorGod>(Player.Faction.PlayerOne, MinorGod.Human);

            Tuple<Player.Faction, MinorGod> p2fire = new Tuple<Player.Faction, MinorGod>(Player.Faction.PlayerTwo, MinorGod.Fire);
            Tuple<Player.Faction, MinorGod> p2water = new Tuple<Player.Faction, MinorGod>(Player.Faction.PlayerTwo, MinorGod.Water);
            Tuple<Player.Faction, MinorGod> p2earth = new Tuple<Player.Faction, MinorGod>(Player.Faction.PlayerTwo, MinorGod.Earth);
            Tuple<Player.Faction, MinorGod> p2human = new Tuple<Player.Faction, MinorGod>(Player.Faction.PlayerTwo, MinorGod.Human);

            _factionToFavor.Add(p1fire, 0);
            _factionToFavor.Add(p1water, 0);
            _factionToFavor.Add(p1earth, 0);
            _factionToFavor.Add(p1human, 1);

            _factionToFavor.Add(p2fire, 0);
            _factionToFavor.Add(p2water, 0);
            _factionToFavor.Add(p2earth, 0);
            _factionToFavor.Add(p2human, 0);
        }

        /// <summary>
        /// Adds an amount to the favor this god has towards a player.
        /// </summary>
        /// <param name="allegiance"> The faction to add favor for. </param>
        /// <param name="god"> The god to add favor for. </param>
        /// <param name="deltaFavor"> The amount of favor to add. </param>
        public static void ModifyFavor(Player.Faction allegiance, MinorGod god, float deltaFavor)
        {
            Dictionary<Tuple<Player.Faction, MinorGod>, float> godToFavor = new Dictionary<Tuple<Player.Faction, MinorGod>, float>(FavorManager.factionToFavor);
            //This tuple is used as the key to search through the godToFavor variable
            Tuple<Player.Faction, MinorGod> godToFavorKey = new Tuple<Player.Faction, MinorGod>(allegiance, god);
            godToFavor[godToFavorKey] += deltaFavor;
        }
    }
}
