using ElementalEngagement.Favor;
using ElementalEngagement.Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ElementalEngagement.Combat
{
    /// <summary>
    /// Used to buff/debuff something in an area.
    /// </summary>
    public abstract class StatModificationArea : MonoBehaviour
    {
        [Tooltip("The area this effects things in.")]
        [SerializeField] protected BindableCollider area;

        [Tooltip("The allegiance of this, buffs only thing that align with this. Leave null to buff anything. Leave allegiance category unaligned to ignore that category.")]
        [SerializeField] protected Allegiance allegiance;


        protected bool CanModify(Collider collider)
        {
            Allegiance colliderAllegiance = collider.GetComponent<Allegiance>();
            return (allegiance.faction == colliderAllegiance?.faction || allegiance.faction == Faction.Unaligned)
                && (allegiance.god == colliderAllegiance?.god || allegiance.god == MinorGod.Unaligned);
        }
    }
}