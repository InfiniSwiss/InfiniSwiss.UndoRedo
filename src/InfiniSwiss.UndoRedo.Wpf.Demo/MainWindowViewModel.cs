using System;
using System.Collections.Generic;
using System.Windows.Input;
using InfiniSwiss.UndoRedo.Commands;
using Microsoft.Xaml.Behaviors.Core;

namespace InfiniSwiss.UndoRedo.Wpf.Demo
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
        public string? UndoRedoTextBoxStringValue
        {
            get => this.GetValue<string>();
            set => this.Set(value);
        }

        /// <summary>
        /// Gets or sets some string property to be bound in the view.
        /// </summary>
        public string? SimpleTextBoxStringValue
        {
            get => this.GetValue<string>();
            set => this.Set(value);
        }

        /// <summary>
        /// Gets or sets some string property to be bound in the view.
        /// </summary>
        public IList<string> ComboBoxOptions
        {
            get => this.GetValue<IList<string>>(new List<string>() { "Option 1", "Option 2", "Option 3" });
            set => this.Set(value);
        }

        /// <summary>
        /// Gets or sets some string property to be bound in the view.
        /// </summary>
        public string? UndoRedoNonEditableComboboxStringValue
        {
            get => this.GetValue<string>();
            set => this.Set(value);
        }

        /// <summary>
        /// Gets or sets some string property to be bound in the view.
        /// </summary>
        public string? SimpleNonEditableComboboxStringValue
        {
            get => this.GetValue<string>();
            set => this.Set(value);
        }

        /// <summary>
        /// Gets or sets some string property to be bound in the view.
        /// </summary>
        public string? UndoRedoEditableComboboxStringValue
        {
            get => this.GetValue<string>();
            set => this.Set(value);
        }

        /// <summary>
        /// Gets or sets some string property to be bound in the view.
        /// </summary>
        public string? SimpleEditableComboboxStringValue
        {
            get => this.GetValue<string>();
            set => this.Set(value);
        }

        /// <summary>
        /// Gets or sets some string property to be bound in the view.
        /// </summary>
        public bool? UndoRedoCheckBoxBooleanValue
        {
            get => this.GetValue<bool?>(null);
            set => this.Set(value);
        }

        /// <summary>
        /// Gets or sets some string property to be bound in the view.
        /// </summary>
        public bool? SimpleCheckBoxBooleanValue
        {
            get => this.GetValue<bool?>(null);
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
            this.CommandProcessor.AddAndExecute(new UndoRedoPropertySetCommand<MainWindowViewModel>(this, vm => vm.SomeCounter, this.SomeCounter + 1));
        }
    }
}
