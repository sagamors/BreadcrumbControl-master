﻿using System.Collections;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace BreadcrumbControl
{
    public class DropDownButton : ToggleButton
    {
        private ContextMenu _contextMenu;

        public static readonly DependencyProperty ItemsSourceProperty = DependencyProperty.Register(
            "ItemsSource", typeof (IEnumerable), typeof (DropDownButton), new PropertyMetadata(default(IEnumerable),
                (o, args) =>
                {
                    var control = (DropDownButton) o;
                    if (control.IsChecked != null && (control.IsChecked.Value && control.ContextMenu!=null))
                    {
                        control.ContextMenu.ItemsSource = control.ItemsSource;
                    }
                }));

        public IEnumerable ItemsSource
        {
            get { return (IEnumerable) GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); }
        }

        public static readonly DependencyProperty HeaderProperty = DependencyProperty.Register(
            "Header", typeof (object), typeof (DropDownButton), new PropertyMetadata(default(object)));

        public object Header
        {
            get { return (object) GetValue(HeaderProperty); }
            set { SetValue(HeaderProperty, value); }
        }

        public static readonly DependencyProperty SelectedValueProperty = DependencyProperty.Register(
            "SelectedValue", typeof (object), typeof (DropDownButton), new PropertyMetadata(default(object)));

        public object SelectedValue
        {
            get { return (object) GetValue(SelectedValueProperty); }
            set { SetValue(SelectedValueProperty, value); }
        }

        static DropDownButton()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(DropDownButton), new FrameworkPropertyMetadata(typeof(DropDownButton)));
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            _contextMenu = GetTemplateChild("PART_ContextMenu") as ContextMenu;
            if (_contextMenu != null)
            {
                _contextMenu.DataContext = this;
                _contextMenu.PlacementTarget = this;
                _contextMenu.Opened += ContextMenu_Opened;
                _contextMenu.Closed += ContextMenu_Closed;
            }

            Checked += DropDownButton_Checked;
            Unchecked += DropDownButton_Unchecked;
        }

        private void ContextMenu_Closed(object sender, RoutedEventArgs e)
        {
            ContextMenu menu = (ContextMenu)sender;
            ((DropDownButton)menu.PlacementTarget).IsChecked = false;
        }

        private void DropDownButton_Unchecked(object sender, RoutedEventArgs e)
        {
            if (ContextMenu == null) return;
            ContextMenu.IsOpen = false;
            IsChecked = false;
        }

        private void DropDownButton_Checked(object sender, RoutedEventArgs e)
        {
            if (ContextMenu == null) return;
            ((DropDownButton)ContextMenu.PlacementTarget).IsChecked = false;

            ContextMenu.PlacementTarget = this;
            ContextMenu.Placement = PlacementMode.Bottom;
            ContextMenu.ItemsSource = ItemsSource;
            ContextMenu.IsOpen = true;
        }

        private void ContextMenu_Opened(object sender, RoutedEventArgs e)
        {
            var contextMenu = ((ContextMenu)sender);
            foreach (var menu in contextMenu.Items)
            {
                var itemMenu = contextMenu.ItemContainerGenerator.ContainerFromItem(menu) as MenuItem;
                if (itemMenu == null) continue;
                itemMenu.Click -= Menu_Click;
                itemMenu.Click += Menu_Click;
            }
        }

        private void Menu_Click(object sender, RoutedEventArgs e)
        {
            MenuItem menuItem = sender as MenuItem;
            if (menuItem != null)
            {
                ContextMenu mainMenu;
/*                while (menuItem)
                {
                    
                }
                foreach (var VARIABLE in menuItem)
                {
                    
                }*/
                SelectedValue = menuItem.DataContext;
            }
        }
    }

}
