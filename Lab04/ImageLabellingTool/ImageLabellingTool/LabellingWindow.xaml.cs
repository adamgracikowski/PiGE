using Microsoft.Win32;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace ImageLabellingTool
{
    public partial class LabellingWindow : Window, INotifyPropertyChanged
    {

        public ObservableCollection<FileItemLabelling> FileItemLabellings { get; set; } = [];
        public ObservableCollection<Labelling> Labellings { get; set; } = [];

        public bool HasLabel;
        public Point Point;
        public Rectangle? Rectangle;
        public SolidColorBrush? ColorBrush;

        public LabellingWindow(Window owner, ObservableCollection<FileItem> fileItems)
        {
            Owner = owner;
            InitializeComponent();

            foreach (var item in fileItems)
                FileItemLabellings.Add(new FileItemLabelling(item));
            FileItemsListView.ItemsSource = FileItemLabellings;
            FileItemsListView.SelectedIndex = 0;
            LabelsListView.ItemsSource = Labellings;
            DataContext = this;
        }


        private void NextImageButtonClick(object sender, RoutedEventArgs e)
        {
            FileItemsListView.SelectedIndex++;
        }

        private void PreviousImageButtonClick(object sender, RoutedEventArgs e)
        {
            FileItemsListView.SelectedIndex--;
        }

        private string? _directoryPath;
        public string? DirectoryPath
        {
            get => _directoryPath;
            set
            {
                _directoryPath = value;
                OnPropertyChanged(nameof(DirectoryPath));
            }
        }
        private void ChooseFolderButtonClick(object sender, RoutedEventArgs e)
        {
            var dialog = new OpenFolderDialog();
            if (dialog.ShowDialog() == true)
                DirectoryPath = dialog.FolderName;
        }
        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void FinishLabellingClick(object sender, RoutedEventArgs e)
        {
            foreach (var labelling in Labellings)
                ExportManager.ExportLabelling(labelling, MainCanvas, DirectoryPath);
            Close();
        }

        private void AddButtonClick(object sender, RoutedEventArgs e)
        {
            var color = ColorGenerator.NextColor();
            var labelText = NewLabelEntryTextBox.Text;
            NewLabelEntryTextBox.Clear();
            var labelEntry = new LabelEntry(labelText, new SolidColorBrush(color));
            Labellings.Add(new Labelling(labelEntry));
            LabelsListView.SelectedIndex = LabelsListView.Items.Count - 1;

            var fileItemLabelling = LabelsListView.SelectedItem as FileItemLabelling;
            fileItemLabelling?.LabellingDictionary.TryAdd(labelEntry, []);
        }

        private void RemoveMenuItemClick(object sender, RoutedEventArgs e)
        {
            if (LabelsListView.SelectedItem is not Labelling labelling) return;
            foreach (var fileItemLabelling in FileItemLabellings)
                fileItemLabelling.LabellingDictionary.Remove(labelling.Label);
            foreach (var rectanglePoint in labelling.Rectangles)
                MainCanvas.Children.Remove(rectanglePoint.Rectangle);
            Labellings.Remove(labelling);

            if (LabelsListView.Items.Count > 0)
                LabelsListView.SelectedIndex = 0;
        }
        private void MainCanvasMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (LabelsListView.SelectedItem is null)
            {
                MessageBox.Show("Add label first!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            ColorBrush = (LabelsListView.SelectedItem as Labelling)?.Label.Color;
            Point = e.GetPosition(MainCanvas);

            Rectangle = new Rectangle
            {
                Stroke = ColorBrush,
                StrokeThickness = 2,
                Fill = Brushes.Transparent
            };

            Canvas.SetLeft(Rectangle, Point.X);
            Canvas.SetTop(Rectangle, Point.Y);
            MainCanvas.Children.Add(Rectangle);
        }
        private void MainCanvasMouseMove(object sender, MouseEventArgs e)
        {
            if (Rectangle == null)
                return;

            var point = e.GetPosition(MainCanvas);

            var x = Math.Min(point.X, Point.X);
            var y = Math.Min(point.Y, Point.Y);

            var width = Math.Max(point.X, Point.X) - x;
            var height = Math.Max(point.Y, Point.Y) - y;

            Rectangle.Width = width;
            Rectangle.Height = height;

            Canvas.SetLeft(Rectangle, x);
            Canvas.SetTop(Rectangle, y);
        }
        private void DrawRectangle(Rectangle rectangle, Point point)
        {
            Canvas.SetLeft(rectangle, point.X);
            Canvas.SetTop(rectangle, point.Y);
            MainCanvas.Children.Add(rectangle);
        }
        private void MainCanvasMouseUp(object sender, MouseButtonEventArgs e)
        {
            if (LabelsListView.SelectedItem is not Labelling labelling || Rectangle is null) return;
            if (FileItemsListView.SelectedItem is not FileItemLabelling fileItemLabelling) return;
            var rectanglePoint = new RectanglePoint(Rectangle, Point, fileItemLabelling);
            labelling.Rectangles.Add(rectanglePoint);

            if (fileItemLabelling.LabellingDictionary.TryGetValue(labelling.Label, out var labelEntry))
            {
                labelEntry.Add(rectanglePoint);
            }
            else
            {
                fileItemLabelling.LabellingDictionary.TryAdd(labelling.Label, [rectanglePoint]);
            }
            Rectangle = null;
        }
        private void FileItemsListViewSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            MainCanvas.Children.Clear();
            if (FileItemsListView.SelectedItem is not FileItemLabelling fileItemLabelling) return;
            foreach (var (_, rectangles) in fileItemLabelling.LabellingDictionary)
                foreach (var rectangle in rectangles)
                    DrawRectangle(rectangle.Rectangle, rectangle.Point);
        }
        private void LabellingTextBoxKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                if (sender is not TextBox textBox) return;
                textBox.IsReadOnly = true;
            }
        }
        private void LabellingTextBoxLostFocus(object sender, RoutedEventArgs e)
        {
            if (sender is not TextBox textBox) return;
            textBox.IsReadOnly = true;
        }
        private void EditMenuItemClick(object sender, RoutedEventArgs e)
        {
            if (sender is not MenuItem menuItem) return;
            if (menuItem.Parent is not ContextMenu contextMenu) return;
            if (contextMenu.PlacementTarget is not Border border) return;

            var textBox = VisualTreeFinder.FindVisualChild<TextBox>(border);

            if (textBox is not null)
            {
                textBox.IsReadOnly = false;
                textBox.Focus();
                textBox.SelectAll();
            }
        }
    }
}