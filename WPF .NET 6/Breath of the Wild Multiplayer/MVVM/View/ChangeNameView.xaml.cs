using System;
using System.Collections.Generic;
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
    /// Lógica de interacción para ChangeNameView.xaml
    /// </summary>
    public partial class ChangeNameView : UserControl
    {
        public ChangeNameView()
        {
            InitializeComponent();
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
