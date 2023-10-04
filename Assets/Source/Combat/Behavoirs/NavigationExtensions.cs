using UnityEngine.AI;
using UnityEngine;

namespace ElementalEngagement
{
    static class NavigationExtensions
    {
        public static void MoveTo(this NavMeshAgent @this, Vector3 position)
        {
            @this.SetDestination(position);
        }
    }
}
