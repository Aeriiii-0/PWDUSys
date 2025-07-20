using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Social_Blade_Dashboard
{
    public partial class DashboardUSC : UserControl
    {
        public DashboardUSC()
        {
            InitializeComponent();
            Loaded += DashboardUSC_Loaded;
        }

        private void DashboardUSC_Loaded(object sender, RoutedEventArgs e)
        {
            InitializeDashboard();
        }

        #region Dashboard Methods

        private void InitializeDashboard()
        {
            LoadDashboardData();
            SetupChartContainer();
        }

        private void LoadDashboardData()
        {
            // x:Name attributes pag cconnect na
        }

        private void SetupChartContainer()
        {
           
        }

        private void RefreshDashboardData()
        {
            
        }

        #endregion

        #region Helper Methods


        #endregion
    }
}