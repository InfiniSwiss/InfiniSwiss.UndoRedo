using System;
using System.Windows.Input;
using Infini.UndoRedo.Commands;
using Microsoft.Xaml.Behaviors.Core;

namespace Infini.UndoRedo.Wpf.Demo
{
    /// <summary>
    /// Represents the view model for <seealso cref="MainWindow"/>.
    /// </summary>
    public class MainWindowViewModel : ViewModelBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MainWindowViewModel"/> class.
        /// </summary>
        public MainWindowViewModel()
        {
            this.CommandProcessor = new CommandProcessor();
            this.UndoCommand = new ActionCommand(() => this.CommandProcessor.Undo());
            this.RedoCommand = new ActionCommand(() => this.CommandProcessor.Redo());
            this.AddCounterCommand = new ActionCommand(this.IncreaseTheCounter);
        }

        /// <summary>
        /// Gets the command for undo the last executed command.
        /// </summary>
        public ICommand RedoCommand { get; }

        /// <summary>
        /// Gets the command for undo the last executed command.
        /// </summary>
        public ICommand UndoCommand { get; }

        /// <summary>
        /// Gets the command for increasing the counter.
        /// </summary>
        public ICommand AddCounterCommand { get; }

        /// <summary>
        /// Gets or sets some string property to be bound in the view.
        /// </summary>
        public string? SomeStringValue
        {
            get => this.GetValue<string>();
            set => this.Set(value);
        }

        /// <summary>
        /// Gets or sets some string property to be bound in the view.
        /// </summary>
        public string? SomeOtherStringValue
        {
            get => this.GetValue<string>();
            set => this.Set(value);
        }

        /// <summary>
        /// Gets or sets some string property to be bound in the view.
        /// </summary>
        public int SomeCounter
        {
            get => this.GetValue(1);
            set => this.Set(value);
        }

        /// <summary>
        /// Gets the command processor for the current view model.
        /// </summary>
        public ICommandProcessor CommandProcessor { get; }

        private void IncreaseTheCounter()
        {
            this.CommandProcessor.AddAndExecute(new UndoRedoSetPropertyCommand<MainWindowViewModel>(this, vm => vm.SomeCounter, this.SomeCounter + 1));
        }
    }
}
