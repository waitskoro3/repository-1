﻿using System;
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
using System.Windows.Shapes;
using WpfApp11.ViewModel;

namespace WpfApp11.View
{
    /// <summary>
    /// Логика взаимодействия для CookerV.xaml
    /// </summary>
    public partial class CookerV : Window
    {
        public CookerV()
        {
            InitializeComponent();
            DataContext = new CookerVM();
        }
    }
}
