using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using UnityEngine;

namespace ElementalEngagement.Player
{
    /// <summary>
    /// A single command that this can execute.
    /// </summary>
    [RequireComponent(typeof(Selectable))]
    public abstract class CommandReceiver : MonoBehaviour
    {
        [Tooltip("The priority of this command. Used to determine which command is run when multiple are followable. Command with the largest priority will be executed.")]
        public int commandPriority;

        // Whether or not this is currently executing a command
        public bool commandInProgress { get; protected set; } = false;

        /// <summary>
        /// Tests if a command is followable.
        /// </summary>
        /// <param name="hitUnderCursor"> The hit result from under the cursor. </param>
        /// <returns> True if this is able to perform the given command. </returns>
        public abstract bool CanExecuteCommand(RaycastHit hitUnderCursor);

        /// <summary>
        /// Performs the given command a command from the selection manager. 
        /// </summary>
        /// <param name="hitUnderCursor"> The hit result from under the cursor. </param>
        /// <param name="selectedObjects"> The other selected objects. </param>
        /// <param name="isAltCommand"> Whether or not this should execute the alternate version of this command (if it exists). </param>
        public abstract void ExecuteCommand(RaycastHit hitUnderCursor, ReadOnlyCollection<Selectable> selectedObjects, bool isAltCommand);

        /// <summary>
        /// Cancels any command this is currently performing.
        /// </summary>
        public abstract void CancelCommand();
        protected virtual List<Vector3> GetDestinations(RaycastHit hit, ReadOnlyCollection<Selectable> selectedObjects, float unitWidth = 5f) 
        { 
            return selectedObjects.Select(_ => hit.point).ToList(); 
        }
        protected Vector3 SelectDestination(RaycastHit hit, ReadOnlyCollection<Selectable> selectedObjects)
        {
            List<Vector3> destinations = GetDestinations(hit, selectedObjects);
            Vector3 selectedDestination = destinations[0];

            foreach (Selectable otherUnit in selectedObjects.OrderByDescending(selected => Vector3.Distance(selected.transform.position, hit.point)))
            {
                selectedDestination = destinations[0];
                foreach (Vector3 destination in destinations)
                {
                    if (Vector3.Distance(otherUnit.transform.position, destination)
                        <
                        Vector3.Distance(otherUnit.transform.position, selectedDestination))
                    {
                        selectedDestination = destination;
                    }
                }

                destinations.Remove(selectedDestination);
                if (otherUnit.gameObject == gameObject)
                    break;
            }
            return selectedDestination;
        }
    }
}