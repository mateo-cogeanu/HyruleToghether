using Breath_of_the_Wild_Multiplayer.Source_files;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Breath_of_the_Wild_Multiplayer.CustomControls
{
    public class optionsLabel : Label
    {

        public static readonly DependencyProperty leftButtonProperty = DependencyProperty.Register(
            nameof(leftButton), typeof(RelayCommand), typeof(optionsLabel));

        public RelayCommand leftButton
        {
            get { return (RelayCommand)GetValue(leftButtonProperty);}
            set { SetValue(leftButtonProperty, value); }
        }


        public static readonly DependencyProperty rightButtonProperty = DependencyProperty.Register(
            nameof(rightButton), typeof(RelayCommand), typeof(optionsLabel));

        public RelayCommand rightButton
        {
            get { return (RelayCommand)GetValue(rightButtonProperty); }
            set { SetValue(rightButtonProperty, value); }
        }



        public static readonly DependencyProperty movingLeftProperty = DependencyProperty.Register(
            nameof(movingLeft), typeof(bool), typeof(optionsLabel));

        public bool movingLeft
        {
            get { return (bool)GetValue(movingLeftProperty); }
            set { SetValue(movingLeftProperty, value); }
        }



        public static readonly DependencyProperty movingRightProperty = DependencyProperty.Register(
            nameof(movingRight), typeof(bool), typeof(optionsLabel));

        public bool movingRight
        {
            get { return (bool)GetValue(movingRightProperty); }
            set { SetValue(movingRightProperty, value); }
        }

    }
}
