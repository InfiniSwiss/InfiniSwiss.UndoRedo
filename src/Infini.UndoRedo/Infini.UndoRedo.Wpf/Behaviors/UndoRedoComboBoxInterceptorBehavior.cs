using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using Infini.UndoRedo.Commands;
using Microsoft.Xaml.Behaviors;

namespace Infini.UndoRedo.Wpf.Behaviors
{
    /// <summary>
    /// Represents a behavior for the ComboBox Elements which automatically
    /// add any property setter to the command processor when element is defocused.
    /// </summary>
    public class UndoRedoComboBoxInterceptorBehavior : Behavior<ComboBox>
    {
        /// <summary>
        /// Dependency property for <see cref="CommandProcessor" />.
        /// </summary>
        public static readonly DependencyProperty CommandProcessorProperty = DependencyProperty.Register(
               "CommandProcessor",
               typeof(ICommandProcessor),
               typeof(UndoRedoComboBoxInterceptorBehavior));

        private int focusCounter = 0;

        /// <summary>
        /// Gets or sets the <seeaslo cref="ICommandProcessor" /> to be used.
        /// </summary>
        public ICommandProcessor CommandProcessor
        {
            get => (ICommandProcessor)this.GetValue(CommandProcessorProperty);
            set => this.SetValue(CommandProcessorProperty, value);
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
            this.focusCounter++;
        }

        private void OnLostFocus(object sender, RoutedEventArgs e)
        {
            // For some unknown reason when clicking on the Combobox "OnLostFocus" is triggered before the "GotFocus" and therefore
            // an Undo command is already registered with the "old" value as null, causing to set the value to null on Undo even if nothing was changed
            // This counter is used to avoid that
            if (this.focusCounter <= 0)
            {
                this.focusCounter = 0;
                return;
            }

            var newValue = this.AssociatedObject.Text;
            var targetProperty = this.AssociatedObject.IsEditable ? ComboBox.TextProperty : ComboBox.SelectedValueProperty;
            var propertyBinding = this.AssociatedObject.GetBindingExpression(targetProperty);

            if (propertyBinding == null)
            {
                throw new ArgumentException($"Expected binding on {targetProperty.Name} property. Was null");
            }

            var command = new UndoRedoPropertySetCommand<object>(propertyBinding.ResolvedSource, propertyBinding.ResolvedSourcePropertyName, newValue);
            this.CommandProcessor.AddAndExecute(command);

            this.focusCounter--;
        }
    }
}
