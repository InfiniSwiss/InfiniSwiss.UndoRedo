using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Infini.UndoRedo.Commands;
using Microsoft.Xaml.Behaviors;
using Microsoft.Xaml.Behaviors.Core;

namespace Infini.UndoRedo.Wpf
{
    /// <summary>
    /// Represents a behavior for the TextBox Elements which automatically
    /// add any property setter to the command processor when element is defocused.
    /// </summary>
    public class UndoRedoTextBoxInterceptorBehavior : Behavior<TextBox>
    {
        /// <summary>
        /// Dependency property for <see cref="CommandProcessor" />.
        /// </summary>
        public static readonly DependencyProperty CommandProcessorProperty = DependencyProperty.Register(
            nameof(CommandProcessor),
            typeof(ICommandProcessor),
            typeof(UndoRedoTextBoxInterceptorBehavior));

        /// <summary>
        /// Dependency property for <see cref="ForceUpdateReverseCommand" />.
        /// </summary>
        public static readonly DependencyProperty ForceUpdateReverseCommandProperty = DependencyProperty.Register(
            nameof(ForceUpdateReverseCommand),
            typeof(ICommand),
            typeof(UndoRedoTextBoxInterceptorBehavior));

        private string? oldValue;

        /// <summary>
        /// Initializes a new instance of the <see cref="UndoRedoTextBoxInterceptorBehavior"/> class.
        /// </summary>
        public UndoRedoTextBoxInterceptorBehavior()
        {
            this.ForceUpdateReverseCommand = new ActionCommand(() => this.UpdateProperty());
        }

        /// <summary>
        /// Gets or sets the <see cref="ICommandProcessor" /> to be used.
        /// </summary>
        public ICommandProcessor CommandProcessor
        {
            get => (ICommandProcessor)this.GetValue(CommandProcessorProperty);
            set => this.SetValue(CommandProcessorProperty, value);
        }

        /// <summary>
        /// Gets or sets the command for update the binding and to not wait until the element is defocused.
        /// </summary>
        public ICommand ForceUpdateReverseCommand
        {
            get => (ICommand)this.GetValue(ForceUpdateReverseCommandProperty);
            set => this.SetValue(ForceUpdateReverseCommandProperty, value);
        }

        /// <inheritdoc/>
        protected override void OnAttached()
        {
            base.OnAttached();
            this.AssociatedObject.GotFocus += this.OnGotFocus;
            this.AssociatedObject.LostFocus += this.OnLostFocus;
        }

        private void OnGotFocus(object sender, RoutedEventArgs e)
        {
            this.oldValue = this.AssociatedObject.Text;
        }

        private void OnLostFocus(object sender, RoutedEventArgs e)
        {
            this.UpdateProperty();
        }

        private void UpdateProperty()
        {
            var newValue = this.AssociatedObject.Text;
            var textBinding = this.AssociatedObject.GetBindingExpression(TextBox.TextProperty);

            if (textBinding == null)
            {
                throw new ArgumentException("Expected binding on Text property. Was null");
            }

            if (newValue != this.oldValue)
            {
                var command = new UndoRedoSetPropertyCommand<object>(textBinding.ResolvedSource, textBinding.ResolvedSourcePropertyName, newValue);
                this.CommandProcessor.AddAndExecute(command);
            }
        }
    }
}
