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

namespace CharlieProject.View.Windows
{
	/// <summary>
	/// Interaction logic for Window1.xaml
	/// </summary>
	public partial class popup : Window
	{
		//Startup method for window
		public popup()
		{
			InitializeComponent();
		}

		//Coded by Martin.
		//A little click-event to close window and return to MainWindow.
		private void Close_Click(object sender, RoutedEventArgs e)
		{
			this.Close();
		}
	}
}
