﻿// Copyright (c) Microsoft Corporation
// The Microsoft Corporation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Windows;
using Microsoft.PowerToys.Settings.UI.Library.Telemetry.Events;
using Microsoft.PowerToys.Telemetry;

namespace PowerToys.Settings
{
    /// <summary>
    /// Interaction logic for App.xaml.
    /// </summary>
    public partial class App : Application
    {
        private MainWindow settingsWindow;

        public bool ShowOobe { get; set; }

        public void OpenSettingsWindow(Type type)
        {
            if (settingsWindow == null)
            {
                settingsWindow = new MainWindow();
            }

            settingsWindow.Show();
            settingsWindow.NavigateToSection(type);
        }

        private void InitHiddenSettingsWindow()
        {
            settingsWindow = new MainWindow();

            // To avoid visual flickering, show the window with a size of 0,0
            // and don't show it in the taskbar
            var originalHight = settingsWindow.Height;
            var originalWidth = settingsWindow.Width;
            settingsWindow.Height = 0;
            settingsWindow.Width = 0;
            settingsWindow.ShowInTaskbar = false;

            settingsWindow.Show();
            settingsWindow.Hide();

            settingsWindow.Height = originalHight;
            settingsWindow.Width = originalWidth;
            settingsWindow.ShowInTaskbar = true;
        }

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            if (!ShowOobe)
            {
                settingsWindow = new MainWindow();
                settingsWindow.Show();
            }
            else
            {
                PowerToysTelemetry.Log.WriteEvent(new OobeStartedEvent());

                // Create the Settings window so that it's fully initialized and
                // it will be ready to receive the notification if the user opens
                // the Settings from the tray icon.
                InitHiddenSettingsWindow();

                OobeWindow oobeWindow = new OobeWindow();
                oobeWindow.Show();
            }
        }
    }
}
