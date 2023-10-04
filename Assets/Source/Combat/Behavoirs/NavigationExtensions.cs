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

            if (!NavMesh.SamplePosition(position, out NavMeshHit hit, 1f, filter))
            {
                if (!NavMesh.Raycast(position, @this.transform.position, out hit, @this.areaMask))
                {
                    NavMesh.Raycast(hit.position, position, out hit, @this.areaMask);
                }

                NavMesh.SamplePosition(hit.position, out hit, 50f, filter);

                Debug.DrawLine(position, @this.transform.position, Color.cyan, 50f);
                Debug.DrawLine(hit.position, position, Color.yellow, 50f);
                Debug.DrawRay(hit.position, Vector3.up * 50, Color.cyan, 50f);
                @this.SetDestination(hit.position);
            }
            else
            {
                @this.SetDestination(position);
            }
        }
    }
}
