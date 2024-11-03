using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Text.Json;
using System.IO;

namespace sm_cbrc
{
    public class Ingredient
    {
        public int Quantity { get; set; }
        public string ItemId { get; set; }
    }

    public class Item
    {
        public string ItemId { get; set; }
        public int Quantity { get; set; }
        public int CraftTime { get; set; }
        public List<Ingredient> IngredientList { get; set; }
    }
    public class ItemToVisual {
        public string ItemId { get; set; }
        public int Quantity { get; set; }
        public int CraftTime { get; set; }
        public List<Ingredient> IngredientList { get; set; }

    }


    public partial class MainWindow : Window
    {
        public string modsfolder = Directory.GetDirectories(System.IO.Path.Combine(Environment.GetFolderPath
            (Environment.SpecialFolder.ApplicationData), "Axolot Games", "Scrap Mechanic", "User"))
            .FirstOrDefault() + "\\Mods";

        string selectedPath = "";
        
        public MainWindow()
        {
            InitializeComponent();
            recipepath.Text = "";
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {

        }

        private void NewRecipe_Click(object sender, RoutedEventArgs e)
        {

        }

        private void OpenRecipe_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new OpenFileDialog();
            try
            {
                dialog.InitialDirectory = modsfolder;
            }
            catch
            {
                dialog.InitialDirectory = "C:\\";
            }
            dialog.Title = "Choose a recipe (json)";

            dialog.ShowDialog();
            recipepath.Text = dialog.FileName;

        }
        
        private void recipepath_TextChanged(object sender, TextChangedEventArgs e)
        {
           UpdateRecipes();

        }

        public void UpdateRecipes()
        {
            try
            {
                
                
                recipecontentjson.Text = File.ReadAllText(recipepath.Text);

                

            }
            catch { }
        }
        void LoadRecipes(string filePath)
        {
            try
            {
                // Deserialize JSON file to List<Item>
                List<Item> items;
                using (StreamReader r = new StreamReader(filePath))
                {
                    string json = r.ReadToEnd();
                    items = JsonSerializer.Deserialize<List<Item>>(json);
                }

                // Clear any existing children in the parent container (for example, a StackPanel)
                recipeContainer.Children.Clear();

                // Loop through each item and create input fields dynamically
                foreach (var item in items)
                {
                    // Create a new StackPanel for each item
                    StackPanel itemPanel = new StackPanel
                    {
                        Margin = new Thickness(10)
                    };

                    // Add ItemId field
                    itemPanel.Children.Add(new TextBlock { Text = "Item ID:", Foreground = Brushes.LightGray });
                    itemPanel.Children.Add(new System.Windows.Controls.TextBox
                    {
                        Text = item.ItemId,
                        Width = 200,
                        Background = new SolidColorBrush(Color.FromRgb(46, 50, 66)),
                        Foreground = Brushes.White,
                        BorderBrush = Brushes.Gray
                    });

                    // Add Quantity field
                    itemPanel.Children.Add(new TextBlock { Text = "Quantity:", Foreground = Brushes.LightGray });
                    itemPanel.Children.Add(new System.Windows.Controls.TextBox
                    {
                        Text = item.Quantity.ToString(),
                        Width = 200,
                        Background = new SolidColorBrush(Color.FromRgb(46, 50, 66)),
                        Foreground = Brushes.White,
                        BorderBrush = Brushes.Gray
                    });

                    // Add CraftTime field
                    itemPanel.Children.Add(new TextBlock { Text = "Craft Time:", Foreground = Brushes.LightGray });
                    itemPanel.Children.Add(new System.Windows.Controls.TextBox
                    {
                        Text = item.CraftTime.ToString(),
                        Width = 200,
                        Background = new SolidColorBrush(Color.FromRgb(46, 50, 66)),
                        Foreground = Brushes.White,
                        BorderBrush = Brushes.Gray
                    });

                    // Add Ingredients field (comma-separated)
                    itemPanel.Children.Add(new TextBlock { Text = "Ingredients (comma-separated IDs):", Foreground = Brushes.LightGray });
                    itemPanel.Children.Add(new System.Windows.Controls.TextBox
                    {
                        Text = string.Join(", ", item.IngredientList.Select(ing => ing.ItemId)),
                        Width = 200,
                        Background = new SolidColorBrush(Color.FromRgb(46, 50, 66)),
                        Foreground = Brushes.White,
                        BorderBrush = Brushes.Gray
                    });

                    // Add the StackPanel to the parent container
                    recipeContainer.Children.Add(itemPanel);
                }
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show($"Error reading file: {ex.Message}");
            }
        }

        private void AddNewRecipe()
        {

        }

        private void recipeAddBtn_Click(object sender, RoutedEventArgs e)
        {
            
        }
    }
}
