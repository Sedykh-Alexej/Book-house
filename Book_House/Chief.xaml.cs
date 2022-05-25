﻿using System;
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

namespace Book_House
{
    /// <summary>
    /// Логика взаимодействия для Chief.xaml
    /// </summary>
    public partial class Chief : Page
    {
        public Chief()
        {
            InitializeComponent();
            txtRab.Content = "Приветствуем Вас, на рабочем месте, " + Manager.IFO + "!";
        }

        private void Calculator(object sender, RoutedEventArgs e)
        {
            Manager.Forma.Navigate(new Calculator2());
        }

        private void Блокнот(object sender, RoutedEventArgs e)
        {
            Process.Start("c:/Windows/System32/notepad.exe");
        }

        private void ГрафикРаб(object sender, RoutedEventArgs e)
        {
            Manager.Forma.Navigate(new График_работы());
        }

        private void Браузер(object sender, RoutedEventArgs e)
        {
            Process.Start("C:/Program Files/Google/Chrome/Application/chrome.exe");
        }

        private void EditParol(object sender, RoutedEventArgs e)
        {
            Manager.Forma.Navigate(new Изменение_пароля());
        }

        private void Exit(object sender, RoutedEventArgs e)
        {
            Manager.Forma.Navigate(new Authorization());
        }

        private void Editstyle(object sender, RoutedEventArgs e)
        {
            Manager.Forma.Navigate(new Стили());
        }

        private void Sotrudniki(object sender, RoutedEventArgs e)
        {
            Manager.Forma.Navigate(new Sotrudniki());
        }

        private void Otchet(object sender, RoutedEventArgs e)
        {
            Manager.Forma.Navigate(new Rent2());
        }
    }
}
