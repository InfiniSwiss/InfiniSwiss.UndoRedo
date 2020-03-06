using System;

namespace InfiniSwiss.UndoRedo.Commands
{
    /// <summary>
    /// Represents a <seealso cref="IUndoRedoCommand"/> which uses actions to define <see cref="IUndoRedoCommand.Execute" /> and <see cref="IUndoRedoCommand.Revert" />.
    /// </summary>
    public class UndoRedoCommand : IUndoRedoCommand
    {
        private readonly Action executeAction;
        private readonly Action revertAction;

        /// <summary>
        /// Initializes a new instance of the <see cref="UndoRedoCommand"/> class with generic actions.
        /// </summary>
        /// <param name="executeAction">The action which can be used to execute the command.</param>
        /// <param name="revertAction">The action which can be used to revert the execution of the command.</param>
        public UndoRedoCommand(Action executeAction, Action revertAction)
        {
            this.executeAction = executeAction;
            this.revertAction = revertAction;
        }

        /// <inheritdoc/>
        public bool IsRedundant => false;

        /// <inheritdoc/>
        public void Execute()
        {
            this.executeAction();
        }

        /// <inheritdoc/>
        public void Revert()
        {
            this.revertAction();
        }
    }
}
