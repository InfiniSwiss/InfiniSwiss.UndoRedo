using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace Infini.UndoRedo.Wpf.Behaviors
{
    /// <summary>
    /// Represents a behavior for the TextBox Elements which automatically
    /// add any property setter to the command processor when element is defocused.
    /// </summary>
    public class UndoRedoTextBoxInterceptorBehavior : UndoRedoInterceptorBehaviorBase<TextBox>
    {
        /// <inheritdoc/>
        protected override IList<DependencyProperty> TargetProperties => new List<DependencyProperty> { TextBox.TextProperty };
    }
}
