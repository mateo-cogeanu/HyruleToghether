using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Breath_of_the_Wild_Multiplayer.MVVM.View
{
    /// <summary>
    /// Interaction logic for SettingsPanelView.xaml
    /// </summary>
    public partial class SettingsPanelView : UserControl
    {
        public SettingsPanelView()
        {
            InitializeComponent();
        }

        private void Hyperlink_RequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            //Process.Start(new ProcessStartInfo(e.Uri.AbsoluteUri));
            Process.Start(new ProcessStartInfo(e.Uri.AbsoluteUri) { UseShellExecute = true });
            e.Handled = true;
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (((TextBox)sender).Text == "")
            {
                ((TextBox)sender).Text = Properties.Settings.Default.playerName;
            }
            else
            {
                Properties.Settings.Default.playerName = ((TextBox)sender).Text;
                Properties.Settings.Default.Save();
            }
        }
    }
}
