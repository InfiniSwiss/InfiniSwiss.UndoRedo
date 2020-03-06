using System.Collections.Generic;

namespace Infini.UndoRedo.Commands
{
    /// <summary>
    /// Represents a <seealso cref="IUndoRedoCommand"/> which is used to add an item of type <see cref="TState"  /> to a collection.
    /// </summary>
    /// <typeparam name="TItem">The type of the item to be added.</typeparam>
    public class UndoRedoCollectionAddCommand<TItem> : IUndoRedoCommand
    {
        private readonly ICollection<TItem> collection;
        private readonly TItem item;

        /// <summary>
        /// Initializes a new instance of the <see cref="UndoRedoCollectionAddCommand{TItem}"/> class.
        /// </summary>
        /// <param name="collection">The targeted collection.</param>
        /// <param name="item">The item to be added.</param>
        public UndoRedoCollectionAddCommand(ICollection<TItem> collection, TItem item)
        {
            this.collection = collection;
            this.item = item;
        }

        /// <inheritdoc/>
        public bool IsRedundant => false;

        /// <inheritdoc/>
        public void Execute()
        {
            this.collection.Add(this.item);
        }

        /// <inheritdoc/>
        public void Revert()
        {
            this.collection.Remove(this.item);
        }
    }
}
