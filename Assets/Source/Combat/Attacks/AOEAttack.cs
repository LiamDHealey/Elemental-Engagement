using ElementalEngagement.Player;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

namespace ElementalEngagement.Combat
{
    /// <summary>
    /// Deals damage to things in an area.
    /// </summary>
    public class AOEAttack : Attack
    {

        [Tooltip("The agent used to move this.")]
        [SerializeField] private Movement movement;

        [Tooltip("The collider in which the target must be within to take damage.")]
        [SerializeField] private BindableCollider attackRange;

        [Tooltip("The maximum number of things this can hit at once.")] [Min(1)]
        [SerializeField] private int maxTargets = 1;

        [Tooltip("Whether this can attack and move at the same time")]
        [SerializeField] private bool canAttackAndMove = false;

        [Tooltip("How long a unit will be stopped for after attacking.")]
        [SerializeField] private float stopDuration = 0.5f;

        private float timeRemainingToAttack;

        //Private tracker for waitBeforeDamage that can be set to true to start the cycle after 
        //the initial check
        private bool needsToWait;


        // Contains a list of all valid things for this aoe to hit.
        private List<Collider> validTargets = new List<Collider>();

        /// <summary>
        /// Binds events
        /// </summary>
        private void Awake()
        {
            needsToWait = waitBeforeDamage;
            timeRemainingToAttack = attackInterval;
            attackRange.onTriggerEnter.AddListener( collider => { TriggerEntered(collider); });
            attackRange.onTriggerExit.AddListener( collider => validTargets.Remove(collider));
        }

        /// <summary>
        /// Starts damaging other if not aligned and in range.
        /// </summary>
        /// <param name="other"></param>
        private void TriggerEntered(Collider other)
        {
            Health health = other.GetComponent<Health>();
            KnockbackReceiver knockbackReceiver = other.GetComponent<KnockbackReceiver>();
            Allegiance otherAllegiance = other.GetComponent<Allegiance>();

            if (health == null && knockbackReceiver == null)
                return;

            // If the target is aligned with this attack
            if (allegiance != null && otherAllegiance != null &&
                allegiance.faction == otherAllegiance.faction)
                return;

            validTargets.Add(other);
        }

        private void Update()
        {
            if (!enabled)
                return;
            if(validTargets.Count == 0)
            {
                if (timeRemainingToAttack > 0)
                    timeRemainingToAttack -= Time.deltaTime;
                else
                    timeRemainingToAttack = attackInterval;

                return;
            }

            if(needsToWait && timeRemainingToAttack > 0)
            {
                timeRemainingToAttack -= Time.deltaTime;
                return;
            }
            else if (timeRemainingToAttack <= 0 || !needsToWait)
            {
                for(int i = validTargets.Count - 1; i >= 0; i--)
                {
                    if (validTargets[i] == null)
                        validTargets.RemoveAt(i);
                }


                int maxIndex = Mathf.Min(maxTargets, validTargets.Count);

                if (maxIndex > 0)
                {
                    onAttackStart?.Invoke();
                }

                for (int i = 0; i < maxIndex; i++)
                {   
                    Health health = validTargets[i].GetComponent<Health>();
                    KnockbackReceiver knockbackReceiver = validTargets[i].GetComponent<KnockbackReceiver>();

                    Debug.Log("Attack Started for " + this.ToString() + this.transform.parent);

                    if (!canAttackAndMove)
                    {
                        CancelInvoke("AllowMovement");
                        movement?.PreventMovement(this);
                        Invoke("AllowMovement", stopDuration);
                    }

                    if (damageDelay > 0)
                    {
                        StartCoroutine(WaitForDamage(validTargets[i]));
                    }
                    else
                    {
                        health?.TakeDamage(damage);
                        knockbackReceiver?.ReceiveKnockback(knockback);
                        onAttackDamage?.Invoke();
                    }
                }

                timeRemainingToAttack = attackInterval;
                needsToWait = true;
            }
        }

        IEnumerator WaitForDamage(Collider other)
        {
            yield return new WaitForSeconds(damageDelay);

            if(other == null)
            {
                onAttackDamage?.Invoke();
                yield break;
            }

            Health health = other.GetComponent<Health>();
            KnockbackReceiver knockbackReceiver = other.GetComponent<KnockbackReceiver>();

            health?.TakeDamage(damage);
            knockbackReceiver?.ReceiveKnockback(knockback);
            onAttackDamage?.Invoke();
        }
        private void AllowMovement() => movement?.AllowMovement(this);
    }
}