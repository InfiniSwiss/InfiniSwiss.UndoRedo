namespace Infini.UndoRedo
{
    /// <summary>
    /// Represents the direction to execute a <see cref="Commands.IUndoRedoCommand" /> command.
    /// </summary>
    internal enum Direction
    {
        /// <summary>
        /// Forward Direction
        /// </summary>
        Forward = 0,

        /// <summary>
        /// Reverse Direction
        /// </summary>
        Reverse = 1,
    }
}
