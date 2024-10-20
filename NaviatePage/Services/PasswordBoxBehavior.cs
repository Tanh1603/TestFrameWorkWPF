using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace NaviatePage.Services
{
    public class PasswordBoxBehavior : DependencyObject
    {
        // Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty BoundPassword =
            DependencyProperty.Register("BoundPassword", typeof(string), typeof(PasswordBoxBehavior), new FrameworkPropertyMetadata(string.Empty, OnBoundPasswordChanged));

        public static string GetBoundPassword(DependencyObject d)
        {
            var box = d as PasswordBox;
            if (box != null)
            {
                box.PasswordChanged -= PasswordChanged;
                box.PasswordChanged += PasswordChanged;
            }

            return (string)d.GetValue(BoundPassword);
        }

        public static void SetBoundPassword(DependencyObject d, string value)
        {
            d.SetValue(BoundPassword, value);
        }

        private static void OnBoundPasswordChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var box = d as PasswordBox;

            if (box != null)
            {
                box.Password = (string)e.NewValue;
            }
        }

        private static void PasswordChanged(object sender, RoutedEventArgs e)
        {
            var box = sender as PasswordBox;

            if (box != null)
            {
                SetBoundPassword(box, box.Password);
            }
        }
    }
}