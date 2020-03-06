using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace Infini.UndoRedo.Wpf.Behaviors
{
    /// <summary>
    /// Represents a behavior for the ComboBox Elements which automatically
    /// add any property setter to the command processor when element is defocused.
    /// </summary>
    public class UndoRedoComboBoxInterceptorBehavior : UndoRedoInterceptorBehaviorBase<ComboBox>
    {
        /// <inheritdoc/>
        protected override IList<DependencyProperty> TargetProperties => this.AssociatedObject.IsEditable
            ? new List<DependencyProperty>() { ComboBox.TextProperty }
            : new List<DependencyProperty>() { ComboBox.SelectedItemProperty, ComboBox.SelectedValueProperty };
    }
}
