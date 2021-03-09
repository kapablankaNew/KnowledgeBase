using KnowledgeBase.DAO;
using KnowledgeBase.DTO;
using KnowledgeBase.Entities;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;


namespace KnowledgeBase
{
    public partial class MainWindow : Window
    {
        private KnowledgeBaseDAO knowledgeBaseDAO { get; set; }

        private DataBaseDAO mainDataBaseDAO { get; set; }

        private DataBaseDAO additionalDataBaseDAO { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            knowledgeBaseDAO = new KnowledgeBaseDAO();
            knowledgeBaseDAO.loadData("../../resources/knowledgeBase.n3");
            List<Process> highLevel = knowledgeBaseDAO.getHighLevel();
            foreach (Process process in highLevel)
            {
                TreeViewItem tree = getTree(process);
                KnowlegdeTree.Items.Add(tree);
            }
            IniManager manager = new IniManager("../../configuration.ini");
            var props = manager.getAllKeys("databaseMain");
            try
            {
                var URL = props.Select(pr => pr + "=" + manager.ReadINI("databaseMain", pr)).
                Aggregate((current, next) => current + ";" + next);
                mainDataBaseDAO = new DataBaseDAO(URL);
                mainDataBaseDAO.getDataForTimeInterval(new DateTime(), new DateTime());
                props = manager.getAllKeys("databaseAdditional");
                URL = props.Select(pr => pr + "=" + manager.ReadINI("databaseAdditional", pr)).
                Aggregate((current, next) => current + ";" + next);
                additionalDataBaseDAO = new DataBaseDAO(URL);
                additionalDataBaseDAO.getDataForTimeInterval(new DateTime(), new DateTime());
                additionalDataBaseDAO.dataBaseUpdate += listenNotification;
            }
            catch (InvalidOperationException)
            {
                MessageBox.Show("File not found! Please, try again");
                buttonGetData.IsEnabled = false;
            }
            //catch (NpgsqlException)
            //{
            //    MessageBox.Show("Attention! Error when connecting to the database!"
            //        + "Please, check parameters in the configuration file "
            //        + "(configuration.ini) and try again");
            //    buttonGetData.IsEnabled = false;
            //}
        }

        private void TreeViewItem_Selected(object sender, RoutedEventArgs e)
        {
            TreeViewItem item = (TreeViewItem)sender;
            if (item.IsSelected == true)
            {
                Process proc = (Process)item.Header;
                TextBoxInfo.Text = proc.getData();
            }
        }

        private TreeViewItem getTree(Process process)
        {
            TreeViewItem result = new TreeViewItem();
            List<Process> children = knowledgeBaseDAO.getChildren(process);
            foreach (Process proc in children)
            {
                result.Items.Add(getTree(proc));
            }
            result.Header = process;
            result.Selected += TreeViewItem_Selected;
            return result;
        }

        private void buttonReconnect_Click(object sender, RoutedEventArgs e)
        {
            IniManager manager = new IniManager("../../configuration.ini");
            var props = manager.getAllKeys("databaseMain");
            var URL = props.Select(pr => pr + "=" + manager.ReadINI("databaseMain", pr)).
                Aggregate((current, next) => current + ";" + next);           
            try
            {
                mainDataBaseDAO = new DataBaseDAO(URL);
                mainDataBaseDAO.getDataForTimeInterval(new DateTime(), new DateTime());
                buttonGetData.IsEnabled = true;
                URL = props.Select(pr => pr + "=" + manager.ReadINI("databaseAdditional", pr)).
                Aggregate((current, next) => current + ";" + next);
                additionalDataBaseDAO = new DataBaseDAO(URL);
                additionalDataBaseDAO.getDataForTimeInterval(new DateTime(), new DateTime());
                additionalDataBaseDAO.dataBaseUpdate += listenNotification;
                MessageBox.Show("Reconnect successful!");
            }
            catch (InvalidOperationException)
            {
                MessageBox.Show("File not found! Please, try again");
                buttonGetData.IsEnabled = false;
            }
            catch (NpgsqlException)
            {
                buttonGetData.IsEnabled = false;
                MessageBox.Show("Reconnect failed! Please, try again!");
            }
        }

        private void buttonGetData_Click(object sender, RoutedEventArgs e)
        {
            if (Picker_From.Value == null || Picker_To.Value == null)
            {
                MessageBox.Show("Please, enter correct date values");
                return;
            }
            DateTime from = (DateTime)Picker_From.Value;
            DateTime to = (DateTime)Picker_To.Value;
            if (from > to)
            {
                MessageBox.Show("Value 'from' must be less or equals than 'to'");
                return;
            }
            string parameterName;
            try
            {
                parameterName = ((TextBlock)ComboBoxParameter.SelectedItem).Text;
            }
            catch (NullReferenceException)
            {
                MessageBox.Show("Please, select parameter!");
                return;
            }
            Parameter param = (Parameter)Enum.Parse(typeof(Parameter), parameterName);
            List<ObjectStateDTO> states;
            try
            {
                states = mainDataBaseDAO.getParameterForTimeInterval(from, to, param);
            }
            catch (NpgsqlException ea)
            {
                MessageBox.Show("Attention! Error when working with database! " + ea.Message);
                return;
            }
            if (states.Count == 0)
            {
                MessageBox.Show("No data available for selecting time interval!");
                return;
            }
            DataGridSensors_fill(states);
        }

        private void DataGridSensors_fill(List<ObjectStateDTO> states)
        {
            DataGridSensors.Columns.Clear();
            DataGridSensors.ItemsSource = states;
            DataGridSensors.Columns[0].Header = "Measure time";
            DataGridSensors.Columns[1].Header = states[0].parameterName.ToString();
            DataGridSensors.Columns.Remove(DataGridSensors.Columns[2]);
        }

        private void DataGridSensors_Loaded(object sender, RoutedEventArgs e)
        {
            DataGridSensors.Columns.Clear();
        }

        public void listenNotification(object sender, NpgsqlNotificationEventArgs e)
        {
            MessageBox.Show(e.Payload);
        }
    }
}