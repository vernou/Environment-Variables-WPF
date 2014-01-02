﻿using EnvironmentVariablesWPF.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
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

namespace EnvironmentVariablesWPF
{
    /// <summary>
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// Default Constructor
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
            WindowsIdentity user = WindowsIdentity.GetCurrent();
            WindowsPrincipal principal = new WindowsPrincipal(user);
            if (principal.IsInRole(WindowsBuiltInRole.Administrator) == true)
            {
                StaticEnvironment.AdminPrivilege = true;
            }
            SearchEngine.selectedUserVariables = new ListSelectedVariables(StaticEnvironment.listVariablesUser);
            SearchEngine.selectedMachineVariables = new ListSelectedVariables(StaticEnvironment.listVariablesMachine);
            UserVariablesView.Display(SearchEngine.selectedUserVariables, false);
            MachineVariablesView.Display(SearchEngine.selectedMachineVariables, !StaticEnvironment.AdminPrivilege);
        }

        /// <summary>
        /// Send the modifications to registre.
        /// </summary>
        private void Apply_Click(object sender, RoutedEventArgs e)
        {
            StaticEnvironment.ApplyToRegistre();
        }

        /// <summary>
        /// Before close this window, verify if some modifications are occured.
        /// If true, then ask to user if he want save this modificaitons.
        /// </summary>
        private void Window_Closed(object sender, EventArgs e)
        {
            if(StaticEnvironment.SomeModificationIsOccured())
                if (MessageBox.Show("Before exit this programm, do you want apply this modification to registre?", 
                    "Some modification will be lose", MessageBoxButton.YesNo,MessageBoxImage.Warning) == MessageBoxResult.Yes)
                    StaticEnvironment.ApplyToRegistre();
        }

        /// <summary>
        /// Real time to search engine. Each modified text drag along a new search.
        /// </summary>
        private void TextBoxSearchEngine_TextChanged(object sender, TextChangedEventArgs e)
        {
            var textBox = sender as TextBox;
            if (textBox != null)
            {
                SearchEngine.Select(textBox.Text);
            }
        }
    }
}
