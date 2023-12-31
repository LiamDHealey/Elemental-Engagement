    using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace ElementalEngagement.Utilities
{
    /// <summary>
    /// Exposes certain functionality to the animator.
    /// 
    /// Edit: Added this test comment by Christian Hartman
    /// </summary>
    public class AnimatorFunctionality : MonoBehaviour
    {
        [Tooltip("The animator this is helping.")]
        [SerializeField] private List<Animator> animators = null;

        [Tooltip("The name of the animator parameter used to check if the animator is moving.")]
        [SerializeField] private string movingParameterName = "IsMoving";

        [Tooltip("The minimum distance this needs to move to be consider moving.")] [Min(0)]
        public float flipThreshold = 0.3f;

        public bool invertFlipping = false;

        [Range(0, 1)]
        public float maxRandomOffsetAmount = 0.3f;

        // Whether or not this is currently moving
        public bool isMoving { get; private set; } = false;

        // The last position this was at.
        private Vector3 lastPosition;

        private List<SpriteRenderer> spriteRenderers;

        private Movement movement;

        /// <summary>
        /// Destroys this component's game object
        /// </summary>
        public void DestroySelf()
        {
            Destroy(gameObject);
        }
        /// <summary>
        /// Destroys this component's game object
        /// </summary>
        public void DelayedDestroySelf(float delay)
        {
            Destroy(gameObject, delay);
        }

        /// <summary>
        /// Destroys the parent game object of this.
        /// </summary>
        public void DestroyParent()
        {
            Destroy(transform.parent.gameObject);
        }

        /// <summary>
        /// Destroys the parent game object of this.
        /// </summary>
        public void DelayedDestroyParent(float delay)
        {
            Destroy(transform.parent.gameObject, delay);
        }

        /// <summary>
        /// Initialized last position.
        /// </summary>
        private void Start()
        {
            movement = GetComponentInParent<Movement>();
            lastPosition = transform.position;
            spriteRenderers = animators.Select(a => a.GetComponent<SpriteRenderer>()).ToList();
            foreach (Animator animator in animators)
            {
                animator.SetFloat("RandomOffset", Random.Range(0, maxRandomOffsetAmount));
            }
        }

        /// <summary>
        /// Updates moving state
        /// </summary>
        public void FixedUpdate()
        {
            if (animators.Count <= 0 || animators[0] == null)
                return;

            bool moved = movement.canMove && !movement.destinationReached;
            
            if (moved != isMoving)
            {
                isMoving = moved;
                foreach (Animator animator in animators)
                {
                    animator?.SetBool(movingParameterName, isMoving);
                }
            }
            
            if (isMoving && Mathf.Abs(movement.moveDirection.x) > flipThreshold)
            {
                bool flipped = movement.moveDirection.x < 0;
                foreach (SpriteRenderer renderer in spriteRenderers)
                {
                    renderer.material.SetInt("_Flip", flipped != invertFlipping ? 1 : 0);
                } 
            }

            lastPosition  = transform.position;
        }
        private void LateUpdate()
        {
            foreach (SpriteRenderer renderer in spriteRenderers)
            {
                renderer.material.SetVector("_TextureRect", new Vector4(renderer.sprite.textureRect.min.x / renderer.sprite.texture.width,
                                                                        renderer.sprite.textureRect.min.y / renderer.sprite.texture.height,
                                                                        renderer.sprite.textureRect.max.x / renderer.sprite.texture.width,
                                                                        renderer.sprite.textureRect.max.y / renderer.sprite.texture.height));
            }
        }
        public void SetTrigger(string triggerName)
        {
            foreach (Animator animator in animators)
            {
                animator?.SetTrigger(triggerName);
            }
        }

        public void ResetTrigger(string triggerName)
        {
            foreach (Animator animator in animators)
            {
                animator?.ResetTrigger(triggerName);
            }
        }
    }
}