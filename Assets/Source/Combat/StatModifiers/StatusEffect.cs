using ElementalEngagement.Player;
using ElementalEngagement;
using ElementalEngagement.Utilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ElementalEngagement.Favor;

namespace ElementalEngagement.Combat
{
    public class StatusEffect : MonoBehaviour
    {

        [Tooltip("How long this status effect will last")]
        [SerializeField] Lifetime lifetime;

        [Tooltip("The area this effects things in.")]
        [SerializeField] protected SphereCollider? area;

        [Tooltip("The allegiance of this, buffs only thing that align with this. Leave null to buff anything. Leave allegiance category unaligned to ignore that category.")]
        [SerializeField] protected Allegiance allegiance;

        [Tooltip("Whether this effects allies or enemies.")]
        [SerializeField] protected bool affectsEnemies = false;

        [Tooltip("Determines whether or not this is a single target effect")]
        [SerializeField] protected bool singleTarget = false;


        protected bool CanModify(Collider collider)
        {
            Allegiance colliderAllegiance = collider.GetComponent<Allegiance>();
            return ((affectsEnemies != (allegiance.faction == colliderAllegiance?.faction)) || allegiance.faction == Faction.Unaligned)
                && (allegiance.god == colliderAllegiance?.god || allegiance.god == MinorGod.Unaligned);
        }
    }
}
