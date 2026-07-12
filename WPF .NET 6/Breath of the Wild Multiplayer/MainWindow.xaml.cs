using Breath_of_the_Wild_Multiplayer.MVVM.ViewModel;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Breath_of_the_Wild_Multiplayer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            CopyAppdataFiles();

            if(Properties.Settings.Default.bcmlLocation == "")
            {
                string Local = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);

                if (File.Exists($"{Local}/bcml/settings.json"))
                {
                    Properties.Settings.Default.bcmlLocation = $"{Local}/bcml/settings.json";
                    Properties.Settings.Default.Save();
                }
            }

            InitializeComponent();

            var viewModel = DataContext as MainViewModel;

            if(viewModel != null )
            {
                viewModel.Window = this;
            }
        }

        protected void CloseClick(object sender, RoutedEventArgs e)
        {
            Close();
        }

        protected void MinimizeClick(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        protected void titleBarDrag(object sender, MouseButtonEventArgs e)
        {
            if (Mouse.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }

        private void CopyAppdataFiles()
        {
            string AppdataFolder = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\BOTWM";
            List<string> Resources = Assembly.GetExecutingAssembly().GetManifestResourceNames().Where(resource => resource.Contains("AppdataFiles")).ToList();

            if (!Directory.Exists(AppdataFolder))
                Directory.CreateDirectory(AppdataFolder);

            foreach (string resource in Resources)
            {
                Stream s = Assembly.GetExecutingAssembly().GetManifestResourceStream(resource);
                string output = $"{AppdataFolder}\\{resource.Replace("Breath_of_the_Wild_Multiplayer.AppdataFiles.", "")}";
                using (FileStream AppdataFile = new FileStream(output, FileMode.Create))
                {
                    byte[] b = new byte[s.Length + 1];
                    s.Read(b, 0, Convert.ToInt32(s.Length));
                    AppdataFile.Write(b, 0, Convert.ToInt32(b.Length - 1));
                }
            }
        }
    }
}
