using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows;
using Prism.Commands;

namespace AnalyzeInterference.Views.Behaviors
{
    public static class DoubleClickBehavior
    {
        public static readonly DependencyProperty DoubleClickCommandProperty = DependencyProperty.RegisterAttached(
                "DoubleClickCommand",
                typeof(DelegateCommand),
                typeof(DoubleClickBehavior),
                new UIPropertyMetadata(DoubleClickCommandChanged)
            );


        public static ICommand GetDoubleClickCommand(DependencyObject target)
        {
            return target.GetValue(DoubleClickCommandProperty) as DelegateCommand<object>;
        }

        public static void SetDoubleClickCommand(DependencyObject target, DelegateCommand<object> value)
        {
            target.SetValue(DoubleClickCommandProperty, value);
        }


        //private static void DoubleClickCommandChanged(DependencyObject target, DependencyPropertyChangedEventArgs e)
        //{
        //    if (target is Control control)
        //    {
        //        control.MouseDoubleClick += Control_MouseDoubleClick;
        //    }
        //}
        private static void DoubleClickCommandChanged(DependencyObject target, DependencyPropertyChangedEventArgs e)
        {
            var control = target as Control;
            if (control != null)
            {
                if ((e.NewValue != null) && (e.OldValue == null))
                {
                    control.MouseDoubleClick += Control_MouseDoubleClick;
                }
                else if ((e.NewValue == null) && (e.OldValue != null))
                {
                    control.MouseDoubleClick -= Control_MouseDoubleClick;
                }
            }
        }

        private static void Control_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Control control = sender as Control;

            // DataGridRow から DataContext (行のデータ) を取得します。
            var dataContext = control.DataContext;

            var command = GetDoubleClickCommand(control) as DelegateCommand<object>;
            if (command != null)
            {
                if (command.CanExecute(dataContext))
                {
                    command.Execute(dataContext);
                }
            }
        }


    }

}
