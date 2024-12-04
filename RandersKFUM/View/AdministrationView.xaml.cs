﻿using RandersKFUM.ViewModel;
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
using RandersKFUM.Utilities;
using RandersKFUM.ViewModel;

namespace RandersKFUM.View
{
    /// <summary>
    /// Interaction logic for AdministrationView.xaml
    /// </summary>
    public partial class AdministrationView : Page
    {

        public AdministrationView()
        {
            InitializeComponent();
            DataContext = new AdministrationViewModel();

        }
    }
}
