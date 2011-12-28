﻿using System;
using System.Windows;

using Microsoft.Phone.Controls;

namespace Coding4Fun.Phone.TestApplication
{
    public partial class MainPage : PhoneApplicationPage
    {
        // Constructor
        public MainPage()
        {
            InitializeComponent();

            DataContext = this;
        }

        private void Slider_Click(object sender, RoutedEventArgs e)
        {
            NavigateTo("/Samples/Slider.xaml");
        }
        
        private void TimeSpan_Click(object sender, RoutedEventArgs e)
        {
            NavigateTo("/Samples/Timespan.xaml");
        }

        private void Overlays_Click(object sender, RoutedEventArgs e)
        {
            NavigateTo("/Samples/Overlays.xaml");
        }

        private void Prompts_Click(object sender, RoutedEventArgs e)
        {
            NavigateTo("/Samples/Prompts.xaml");
        }

        private void About_Click(object sender, RoutedEventArgs e)
        {
            NavigateTo("/Samples/AboutItems.xaml");
        }

        private void Memory_Click(object sender, RoutedEventArgs e)
        {
            NavigateTo("/Samples/Memory.xaml");
        }
        
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            NavigateTo("/Samples/ButtonControls.xaml");
        }

        private void Colors_Click(object sender, RoutedEventArgs e)
        {
            NavigateTo("/Samples/ColorControls.xaml");
        }

		private void MetroFlow_Click(object sender, RoutedEventArgs e)
        {
            NavigateTo("/Samples/MetroFlow.xaml");
        }
		
		private void NavigateTo(string page)
        {
            NavigationService.Navigate(new Uri(page, UriKind.Relative));
        }
    }
}