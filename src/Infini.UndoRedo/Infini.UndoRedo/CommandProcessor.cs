using System.Collections.Generic;
using Infini.UndoRedo.Commands;

namespace Infini.UndoRedo
{
    /// <summary>
    /// Represents the command processor.
    /// </summary>
    public class CommandProcessor : ICommandProcessor
    {
        private readonly List<IUndoRedoCommand> commands;
        private Direction direction;
        private int commandIndex;

        /// <summary>
        /// Initializes a new instance of the <see cref="CommandProcessor"/> class with an empty list of commands.
        /// </summary>
        public CommandProcessor()
        {
            this.commands = new List<IUndoRedoCommand>();
            this.direction = Direction.Forward;
            this.commandIndex = 0;
        }

        /// <inheritdoc/>
        public void AddAndExecute(IUndoRedoCommand command, bool ignoreRedundant = true)
        {
            if (ignoreRedundant && command.IsRedundant)
            {
                return;
            }

            if (this.commands.Count == 0)
            {
                this.commands.Add(command);
                this.commandIndex = 0;
            }
            else
            {
                if (this.direction == Direction.Reverse)
                {
                    this.commands[this.commandIndex] = command;
                    this.RemoveCommandsAfterCurrentIndex();
                }
                else
                {
                    this.RemoveCommandsAfterCurrentIndex();
                    this.commands.Add(command);
                    this.commandIndex = this.commands.Count - 1;
                }
            }

            command.Execute();
            this.direction = Direction.Forward;
        }

        /// <inheritdoc/>
        public void Redo()
        {
            if (this.commands.Count == 0)
            {
                return;
            }

            var command = this.commands[this.commandIndex];
            if (this.direction == Direction.Reverse)
            {
                command.Execute();
            }
            else if (this.direction == Direction.Forward && this.commandIndex < this.commands.Count - 1)
            {
                this.commandIndex++;
                this.commands[this.commandIndex].Execute();
            }

            this.direction = Direction.Forward;
        }

        /// <inheritdoc/>
        public void Undo()
        {
            if (this.commands.Count == 0)
            {
                return;
            }

            var command = this.commands[this.commandIndex];
            if (this.direction == Direction.Forward)
            {
                command.Revert();
            }
            else if (this.direction == Direction.Reverse && this.commandIndex > 0)
            {
                this.commandIndex--;
                this.commands[this.commandIndex].Revert();
            }

            this.direction = Direction.Reverse;
        }

        private void RemoveCommandsAfterCurrentIndex()
        {
            if (this.commandIndex < this.commands.Count)
            {
                this.commands.RemoveRange(this.commandIndex + 1, this.commands.Count - this.commandIndex - 1);
            }
        }
    }
}
