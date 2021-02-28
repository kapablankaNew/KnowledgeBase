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
            List<ObjectState> states = dataBaseDAO.getAllData();
            DataGridSensors_fill(states);
        }

        private void DataGridSensors_fill(Dictionary<DateTime, double> data)
        {
            DataGridSensors.Columns.Clear();
            DataGridSensors.ItemsSource = data;
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