using KnowledgeBase.DAO;
using KnowledgeBase.Entities;
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
            MessageBox.Show(from.ToString());
            List<ObjectState> states = dataBaseDAO.getDataForTimeInterval(from, to);
            Dictionary<DateTime, double> values = new Dictionary<DateTime, double>();
            foreach (var state in states)
            {
                values[state.measureTime] = state.Tnv;
            }
            DataGridSensors_fill(values, Parameter.Tnv);
        }

        private void DataGridSensors_fill(Dictionary<DateTime, double> data, Parameter param)
        {
            DataGridSensors.Columns.Clear();
            DataGridSensors.ItemsSource = data;
            DataGridSensors.Columns[0].Header = "Measure time";
            DataGridSensors.Columns[1].Header = param.ToString();
        }

        private void DataGridSensors_fill(List<ObjectState> states)
        {
            DataGridSensors.Columns.Clear();
            DataGridSensors.ItemsSource = states;
        }

        private void DataGridSensors_Loaded(object sender, RoutedEventArgs e)
        {
            DataGridSensors.Columns.Clear();
        }
    }
}