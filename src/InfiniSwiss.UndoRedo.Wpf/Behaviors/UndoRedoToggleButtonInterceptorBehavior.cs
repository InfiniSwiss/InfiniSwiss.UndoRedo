using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls.Primitives;

namespace InfiniSwiss.UndoRedo.Wpf.Behaviors
{
    /// <summary>
    /// Represents a behavior for the ToggleButton Elements which automatically
    /// add any property setter to the command processor when element is clicked.
    /// </summary>
    public class UndoRedoToggleButtonInterceptorBehavior : UndoRedoInterceptorBehaviorBase<ToggleButton>
    {
        /// <inheritdoc/>
        protected override IList<RoutedEvent> TriggeringEvents => new List<RoutedEvent> { ToggleButton.ClickEvent };

        /// <inheritdoc/>
        protected override IList<DependencyProperty> TargetProperties => new List<DependencyProperty> { ToggleButton.IsCheckedProperty };
    }
}
