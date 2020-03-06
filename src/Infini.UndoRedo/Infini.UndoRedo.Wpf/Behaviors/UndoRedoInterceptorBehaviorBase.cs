using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using Infini.UndoRedo.Commands;
using Microsoft.Xaml.Behaviors;
using Microsoft.Xaml.Behaviors.Core;

namespace Infini.UndoRedo.Wpf.Behaviors
{
    /// <summary>
    /// Represents the base for a behavior which automatically
    /// add any property setter to the command processor.
    /// </summary>
    /// <typeparam name="TElement">The type of the attached object.</typeparam>
    public abstract class UndoRedoInterceptorBehaviorBase<TElement> : Behavior<TElement>
        where TElement : FrameworkElement
    {
        /// <summary>
        /// Dependency property for <see cref="CommandProcessor" />.
        /// </summary>
        public static readonly DependencyProperty CommandProcessorProperty = DependencyProperty.Register(
               nameof(CommandProcessor),
               typeof(ICommandProcessor),
               typeof(UndoRedoInterceptorBehaviorBase<TElement>));

        /// <summary>
        /// Dependency property for <see cref="ForceUpdateReverseCommand" />.
        /// </summary>
        public static readonly DependencyProperty ForceUpdateReverseCommandProperty = DependencyProperty.Register(
            nameof(ForceUpdateReverseCommand),
            typeof(ICommand),
            typeof(UndoRedoInterceptorBehaviorBase<TElement>));

        private RoutedEventHandler? eventHandler;

        /// <summary>
        /// Initializes a new instance of the <see cref="UndoRedoInterceptorBehaviorBase{TElement}"/> class.
        /// </summary>
        protected UndoRedoInterceptorBehaviorBase()
        {
            this.ForceUpdateReverseCommand = new ActionCommand(() => this.UpdateProperty());
        }

        /// <summary>
        /// Gets or sets the <seeaslo cref="ICommandProcessor" /> to be used.
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

        /// <summary>
        /// Gets the dependency properties which needs to be set.
        /// </summary>
        protected abstract IList<DependencyProperty> TargetProperties { get; }

        /// <summary>
        /// Gets the events which triggers the updating of the property.
        /// </summary>
        protected virtual IList<RoutedEvent> TriggeringEvents { get; } = new List<RoutedEvent> { UIElement.LostFocusEvent };

        /// <inheritdoc/>
        protected override void OnAttached()
        {
            base.OnAttached();
            this.eventHandler = new RoutedEventHandler((sender, eventArgs) => this.UpdateProperty());
            foreach (var routedEvent in this.TriggeringEvents)
            {
                this.AssociatedObject.AddHandler(routedEvent, this.eventHandler);
            }
        }

        /// <inheritdoc/>
        protected override void OnDetaching()
        {
            base.OnDetaching();
            foreach (var routedEvent in this.TriggeringEvents)
            {
                this.AssociatedObject.RemoveHandler(routedEvent, this.eventHandler);
            }
        }

        /// <summary>
        /// Updates the properties from the binding based on the <see cref="TargetProperties"/> property.
        /// </summary>
        protected virtual void UpdateProperty()
        {
            BindingExpression? propertyBinding = null;
            object? newValue = null;

            foreach (var property in this.TargetProperties)
            {
                propertyBinding = this.AssociatedObject.GetBindingExpression(property);
                if (propertyBinding != null)
                {
                    newValue = this.AssociatedObject.GetValue(property);
                    break;
                }
            }

            if (propertyBinding == null)
            {
                throw new ArgumentException($"Expected binding on {string.Join("or", this.TargetProperties.Select(property => "\"" + property.Name + "\""))} property.");
            }

            var command = new UndoRedoPropertySetCommand<object>(propertyBinding.ResolvedSource, propertyBinding.ResolvedSourcePropertyName, newValue);
            this.CommandProcessor.AddAndExecute(command);
        }
    }
}
