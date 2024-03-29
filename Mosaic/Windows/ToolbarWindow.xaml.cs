﻿// 
// ToolbarWindows
// Modified: 29-mar-2019
//

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Interop;
using Screen = System.Windows.Forms.Screen;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Microsoft.Win32;
using Mosaic.Base;
using Mosaic.Controls;
using Mosaic.Core;

namespace Mosaic.Windows
{
    /// <summary>
    /// Interaction logic for ToolbarWindow.xaml
    /// </summary>
    public partial class ToolbarWindow : Window, IToolbarWindow
    {
        //private readonly int totalWorkingAreaWidth = 0;
        public event EventHandler OpeningToolbar;
        public event EventHandler ClosingToolbar;

        public bool IsOpened { get; set; }

        // ToolbarWindow
        public ToolbarWindow()
        {
            // если не App.Settings.IsExclusiveMode или App.Settings.ShowTaskbar - истина
            // то есть App.Settings.IsExclusiveMode - ложь
            if (!App.Settings.IsExclusiveMode || App.Settings.ShowTaskbar)
            {
                this.Height = SystemParameters.WorkArea.Height;
                this.Top = SystemParameters.WorkArea.Top;
                this.Opacity = 1;
            }
            else
            {
                this.Height = SystemParameters.PrimaryScreenHeight;
                this.Top = 0;
                this.Opacity = 1;
            }

            InitializeComponent();
        }

        // Потестировать это место!
        private void WindowLoaded(object sender, RoutedEventArgs e)
        {
            this.Left = SystemParameters.PrimaryScreenWidth - 1;

            IsOpened = false;  // состояние -- закрыто :)

            // overround -- bugfix    
            // решение проблемы с незакрытием до конца тулбара (на 2 мониторе оставались артефакты)

            OpenToolbar();  // ME : быстренько откроем тулбар...
            CloseToolbar(); // ME : ...и сразу же закроем :)


        }

        private void ToolbarMouseLeave(object sender, MouseEventArgs e)
        {
            if (!App.Settings.ShowMenuButton)
                CloseToolbar();
        }

        private void WindowMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            foreach (var window in App.Current.Windows)
            {
                if (window.GetType() == typeof(HubWindow))
                    continue;
                ((Window)window).Show();
                ((Window)window).Activate();
            }

            //if (App.Current.MainWindow != null)
            //{
            //    App.Current.MainWindow.Show();
            //    App.Current.MainWindow.Activate();
            //}

            //if (App.Settings.ShowMenuButton)
            //    return;
            if (!IsOpened)
            {
                OpenToolbar();
            }
        }

        // ******* Open Toolbar *******
        public void OpenToolbar()
        {
            OpeningToolbar(null, EventArgs.Empty); // открытие тулбара -- очистка / зануление

            var s = Resources["ToolbarOpenAnim"] as Storyboard; // порождение обьекта а-ля Storyboard

            // инициализация "дочерних" обьектов сториборда
            ((DoubleAnimation)s.Children[0]).To = SystemParameters.PrimaryScreenWidth - this.Width;

            s.Begin(); // применяет анимации

            // тулбар открыт -- да!
            IsOpened = true;

            // цикл по дочерним обьектам тулбара
            for (int i = 0; i < Toolbar.Children.Count; i++)
            {
                // если проходим первую половину "дочек"...
                if (i < Toolbar.Children.Count / 2)
                {
                    ((ToolbarItem)Toolbar.Children[i]).FadeIn(Toolbar.Children.Count - i);
                }
                else
                {
                    // если проходим вторую половину "дочек"...
                    ((ToolbarItem)Toolbar.Children[i]).FadeIn(i);
                }
            }
        }

        // Закрывание тулбара
        public void CloseToolbar()
        {
            ClosingToolbar(null, EventArgs.Empty);
            var s = Resources["ToolbarCloseAnim"] as Storyboard;
            ((DoubleAnimation)s.Children[0]).To = SystemParameters.PrimaryScreenWidth - 1;
            s.Begin();
            IsOpened = false; // состояние -- закрыто :)
        }

        private void ExitButtonMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            Application.Current.MainWindow.Close();
        }

        private void SettingsButtonMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            CloseToolbar();
            App.ShowOptions();
        }

        private void WidgetsButtonMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            var widgetsToolbar = new WidgetsToolbarWindow();
            widgetsToolbar.Show();
            widgetsToolbar.OpenToolbar();
        }

        private void PinButtonMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            var pinToolbar = new PinToolbarWindow();
            pinToolbar.Show();
            pinToolbar.OpenToolbar();
            //CloseToolbar();
            //var addressWindow = new AddressBarWindow();
            //addressWindow.ShowDialog();
            //if (string.IsNullOrEmpty(addressWindow.AddressBox.Text))
            //    return;
            //var widget = App.WidgetManager.CreateWidget(addressWindow.AddressBox.Text);
            //App.WidgetManager.LoadWidget(widget);
        }

        private void PinAppButtonMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            CloseToolbar();
            var dialog = new OpenFileDialog();
            dialog.Filter = "Executable files|*.exe";
            if (!(bool)dialog.ShowDialog())
                return;
            var widget = App.WidgetManager.CreateWidget(dialog.FileName);
            App.WidgetManager.LoadWidget(widget);
        }

        private void PeopleButtonMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            CloseToolbar();
            var peopleHub = new PeopleHub();
            peopleHub.Show();
        }

        private void WindowPreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.F && Keyboard.IsKeyDown(Key.LeftCtrl))
            {
                if (!IsOpened)
                    OpenToolbar();
                else
                    CloseToolbar();
            }
        }
    }
}
