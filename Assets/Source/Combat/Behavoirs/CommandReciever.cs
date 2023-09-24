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
        public abstract void ExecuteCommand(RaycastHit hitUnderCursor);

        /// <summary>
        /// Cancels any command this is currently performing.
        /// </summary>
        public abstract void CancelCommand();
    }
}