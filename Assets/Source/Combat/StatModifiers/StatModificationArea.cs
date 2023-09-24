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
        [SerializeField] protected Player.Allegiance allegiance;
    }
}