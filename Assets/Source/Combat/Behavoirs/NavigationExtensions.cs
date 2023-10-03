using UnityEngine.AI;
using UnityEngine;

namespace ElementalEngagement
{
    static class NavigationExtensions
    {
        public static void MoveTo(this NavMeshAgent @this, Vector3 position)
        {
            position.y = @this.transform.position.y;
            NavMeshQueryFilter filter = new NavMeshQueryFilter
            {
                areaMask = @this.areaMask,
                agentTypeID = NavMesh.GetSettingsByIndex(2).agentTypeID,
            };

            if (!NavMesh.SamplePosition(position, out NavMeshHit hit, 0f, filter))
            {
                if (!NavMesh.SamplePosition(position + (@this.transform.position - position).normalized, out hit, 100f, filter))
                    throw new System.Exception("No closest point found.");
                //Debug.DrawRay(hit.position, Vector3.up * 50, Color.yellow, 1f);
                @this.SetDestination(hit.position);
            }
            else
            {
                @this.SetDestination(position);
            }
        }
    }
}
