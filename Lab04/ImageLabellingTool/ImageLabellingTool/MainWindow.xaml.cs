using Microsoft.Win32;
using System.Collections.ObjectModel;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ImageLabellingTool
{
    using SIO = System.IO;

    public partial class MainWindow : Window
    {
        public static readonly Regex Regex = new(@"\.(jpg|jpeg|png|bmp|gif)$", RegexOptions.IgnoreCase);
        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
            FileMiniaturesListView.ItemsSource = FileItems;
            ChosenItemsListBox.ItemsSource = ChosenFileItems;
        }

        private void ArrowButtonClick(object sender, RoutedEventArgs e)
        {
            var selectedItems = FileMiniaturesListView.SelectedItems.Cast<FileItem>().ToList();
            foreach (var item in selectedItems)
            {
                if (Directory.Exists(item.Path))
                {
                    foreach (var file in Directory.GetFiles(item.Path))
                        if (Regex.IsMatch(SIO.Path.GetExtension(file)))
                            ChosenFileItems.Add(new FileItem(file));
                }
                else
                {
                    ChosenFileItems.Add(item);
                }
            }
        }

        private void DeleteButtonClick(object sender, RoutedEventArgs e)
        {
            var selectedItems = ChosenItemsListBox.SelectedItems.Cast<FileItem>().ToList();
            foreach (var item in selectedItems)
                ChosenFileItems.Remove(item);
        }

        private void LoadDatasetClick(object sender, RoutedEventArgs e)
        {
            var dialog = new OpenFolderDialog();
            if (dialog.ShowDialog() == true)
            {
                var path = dialog.FolderName;
                PopulateFileItems(path);
            }
        }
        private void PopulateFileItems(string path)
        {
            FileItems.Clear();
            FileItems.Add(new FileItem(SIO.Path.Combine(path, "..")));
            foreach (var file in Directory.GetFiles(path))
                if (Regex.IsMatch(SIO.Path.GetExtension(file)))
                    FileItems.Add(new FileItem(file));
            foreach (var directory in Directory.GetDirectories(path))
                FileItems.Add(new FileItem(directory));
        }

        private void FileMiniaturesListViewMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (FileMiniaturesListView.SelectedItem != null)
            {
                var selected = (FileItem)FileMiniaturesListView.SelectedItem;
                if (Directory.Exists(selected.Path))
                    PopulateFileItems(selected.Path);
            }
        }

        private void StartLabellingClick(object sender, RoutedEventArgs e)
        {
            var dialog = new LabellingWindow(this, ChosenFileItems);
            Opacity = 0.4;
            dialog.ShowDialog();
            Opacity = 1.0;
        }

        public ObservableCollection<FileItem> FileItems { get; set; } = [];
        public ObservableCollection<FileItem> ChosenFileItems { get; set; } = [];
    }
}