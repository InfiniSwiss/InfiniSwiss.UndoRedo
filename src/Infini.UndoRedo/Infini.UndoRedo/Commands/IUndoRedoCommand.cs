namespace Infini.UndoRedo.Commands
{
    /// <summary>
    /// Represents a command which can be executed or reverted.
    /// </summary>
    public interface IUndoRedoCommand
    {
        /// <summary>
        /// Gets a value indicating whether this command is redundant or not.
        /// </summary>
        bool IsRedundant { get; }

        /// <summary>
        /// Executes the command forward.
        /// </summary>
        void Execute();

        /// <summary>
        /// Reverts the command after execution and the state will be again as it was before Execute.
        /// </summary>
        void Revert();
    }
}
