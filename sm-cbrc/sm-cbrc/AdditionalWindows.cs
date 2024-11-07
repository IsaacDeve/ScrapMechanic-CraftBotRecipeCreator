using System;
using System.IO;
using System.Text.Json;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Linq;

namespace sm_cbrc
{
    class Adw_Item
    {
        // hi im sigma //
        public string Id { get; set; }
        public string Name { get; set; }
        private Image imageControl;
        private Label labelControl;
        private TextBox textBoxControl;
        private Button actionButton;

        public event Action<string> OnSelect;

        public Adw_Item()
        {
            
        }

        public Adw_Item(string adw_item_id, string adw_item_name)
        {
            Id = adw_item_id;
            Name = adw_item_name;
            InitializeControls();
        }

        private void InitializeControls()
        {
            string imagePath = $"{AppDomain.CurrentDomain.BaseDirectory}\\icons\\{Name}_icon.png";
            if (!File.Exists(imagePath))
            {
                imagePath = $"{AppDomain.CurrentDomain.BaseDirectory}\\icons\\blank_icon.png";
            }
            imageControl = new Image
            {
                Width = 100,
                Height = 100,
                Margin = new Thickness(5),
                Source = new BitmapImage(new Uri(imagePath, UriKind.Absolute)),
                HorizontalAlignment = HorizontalAlignment.Left
            };

            labelControl = new Label
            {
                Content = Name,
                Foreground = new SolidColorBrush(Colors.LightGray),
                Margin = new Thickness(5),
                HorizontalAlignment = HorizontalAlignment.Left
            };

            actionButton = new Button
            {
                Content = "Choose",
                Margin = new Thickness(5),
                HorizontalAlignment = HorizontalAlignment.Left
            };
            actionButton.Click += (sender, e) => OnSelect?.Invoke(Id);

            textBoxControl = new TextBox
            {
                Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#2e3242")),
                BorderThickness = new Thickness(0),
                Foreground = new SolidColorBrush(Colors.LightGray),
                Text = Id,
                Margin = new Thickness(5),
                HorizontalAlignment = HorizontalAlignment.Left
            };
        }

        public void AddControlsToPanel(Panel panel)
        {
            InitializeControls();
            panel.Children.Add(imageControl);
            panel.Children.Add(labelControl);
            panel.Children.Add(actionButton);
            panel.Children.Add(textBoxControl);
        }
    }

    internal class AdditionalWindows
    {
        private string chosenId = "";

        public string OpenChoiceWin()
        {
            Window win = new Window
            {
                Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#282C39")),
                Width = 500,
                Height = 700
            };

            TextBox searchBox = new TextBox
            {
                Margin = new Thickness(5),
                Background = new SolidColorBrush(Colors.White),
                Foreground = new SolidColorBrush(Colors.Black),
                Width = 480
            };

            ScrollViewer scrollViewer = new ScrollViewer
            {
                VerticalScrollBarVisibility = ScrollBarVisibility.Auto
            };

            StackPanel panel = new StackPanel
            {
                HorizontalAlignment = HorizontalAlignment.Left
            };

            string jsonPath = $"{AppDomain.CurrentDomain.BaseDirectory}\\craftables\\craftables.json";
            string jsonString = File.ReadAllText(jsonPath);
            List<Adw_Item> items = JsonSerializer.Deserialize<List<Adw_Item>>(jsonString);

            void DisplayItems(string filter = "")
            {
                panel.Children.Clear();
                foreach (var item in items)
                {
                    if (string.IsNullOrEmpty(filter) || item.Name.IndexOf(filter, StringComparison.OrdinalIgnoreCase) >= 0)
                    {
                        item.OnSelect -= id => { chosenId = id; win.Close(); };
                        item.OnSelect += id => { chosenId = id; win.Close(); };
                        item.AddControlsToPanel(panel);
                    }
                }
            }

            DisplayItems();

            searchBox.TextChanged += (sender, e) =>
            {
                DisplayItems(searchBox.Text);
            };

            StackPanel mainPanel = new StackPanel();
            mainPanel.Children.Add(searchBox);
            mainPanel.Children.Add(scrollViewer);

            scrollViewer.Content = panel;

            win.Content = mainPanel;
            win.ShowDialog();
            return chosenId;
        }
    }
}
