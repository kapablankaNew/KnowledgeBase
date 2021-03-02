using KnowledgeBase.DAO;
using KnowledgeBase.DTO;
using KnowledgeBase.Entities;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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

namespace KnowledgeBase
{
    public partial class MainWindow : Window
    {
        private KnowledgeBaseDAO knowledgeBaseDAO { get; set; }

        private DataBaseDAO dataBaseDAO { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            knowledgeBaseDAO = new KnowledgeBaseDAO();
            knowledgeBaseDAO.loadData("../../resources/knowledgeBase.n3");
            dataBaseDAO = new DataBaseDAO();
            List<Process> highLevel = knowledgeBaseDAO.getHighLevel();
            foreach (Process process in highLevel)
            {
                TreeViewItem tree = getTree(process);
                KnowlegdeTree.Items.Add(tree);
            }
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
                states = dataBaseDAO.getParameterForTimeInterval(from, to, param);
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
    }
}