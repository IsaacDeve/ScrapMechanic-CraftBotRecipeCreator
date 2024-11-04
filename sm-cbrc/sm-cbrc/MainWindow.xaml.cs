using System;
using System.Collections.Generic;
using System.Linq;
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
using System.Text.Json;
using System.IO;
using static System.Net.Mime.MediaTypeNames;
using System.Text.Json.Serialization;

namespace sm_cbrc
{
    public class Ingredient
    {
        [JsonPropertyName("quantity")]
        public int Quantity { get; set; }

        [JsonPropertyName("itemId")]

        public string ItemId { get; set; }
    }

    public class Item
    {

        [JsonPropertyName("itemId")]
        public string ItemId { get; set; }

        [JsonPropertyName("quantity")]

        public int Quantity { get; set; }

        [JsonPropertyName("craftTime")]
        public int CraftTime { get; set; }

        [JsonPropertyName("ingredientList")]

        public List<Ingredient> IngredientList { get; set; } = new List<Ingredient>();
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
            // Menu item logic if needed
        }

        private void NewRecipe_Click(object sender, RoutedEventArgs e)
        {
            // New recipe logic if needed
        }

        private void OpenRecipe_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new Microsoft.Win32.OpenFileDialog();
            try
            {
                dialog.InitialDirectory = modsfolder;
            }
            catch
            {
                dialog.InitialDirectory = "C:\\";
            }
            dialog.Title = "Choose a recipe (json)";
            if (dialog.ShowDialog() == true)
            {
                recipepath.Text = dialog.FileName;
            }
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
                LoadRecipes(recipepath.Text);
            }
            catch
            {
                // Handle errors silently or log if needed
            }
        }

        void LoadRecipes(string filePath)
        {
            try
            {


                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };
                // Deserialize JSON file to List<Item>
                List<Item> items;
                using (StreamReader r = new StreamReader(filePath))
                {
                    string json = r.ReadToEnd();
                    items = JsonSerializer.Deserialize<List<Item>>(json, options);
                }

                // Clear any existing children in the parent container
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
                    itemPanel.Children.Add(new TextBox
                    {
                        Text = item.ItemId,
                        Width = 200,
                        Background = new SolidColorBrush(Color.FromRgb(46, 50, 66)),
                        Foreground = Brushes.White,
                        BorderBrush = Brushes.Gray
                    });

                    // Add Quantity field
                    itemPanel.Children.Add(new TextBlock { Text = "Quantity:", Foreground = Brushes.LightGray });
                    itemPanel.Children.Add(new TextBox
                    {
                        Text = item.Quantity.ToString(),
                        Width = 200,
                        Background = new SolidColorBrush(Color.FromRgb(46, 50, 66)),
                        Foreground = Brushes.White,
                        BorderBrush = Brushes.Gray
                    });

                    // Add CraftTime field
                    itemPanel.Children.Add(new TextBlock { Text = "Craft Time:", Foreground = Brushes.LightGray });
                    itemPanel.Children.Add(new TextBox
                    {
                        Text = item.CraftTime.ToString(),
                        Width = 200,
                        Background = new SolidColorBrush(Color.FromRgb(46, 50, 66)),
                        Foreground = Brushes.White,
                        BorderBrush = Brushes.Gray
                    });

                    // Add Ingredients header
                    itemPanel.Children.Add(new TextBlock { Text = "Ingredients:", Foreground = Brushes.LightGray, Margin = new Thickness(0, 10, 0, 5) });

                    // Create a sub-StackPanel for each Ingredient
                    foreach (var ingredient in item.IngredientList)
                    {
                        StackPanel ingredientPanel = new StackPanel
                        {
                            Orientation = Orientation.Horizontal,
                            Margin = new Thickness(5)
                        };

                        // Display ItemId of the ingredient
                        ingredientPanel.Children.Add(new TextBlock { Text = "ID:", Foreground = Brushes.LightGray, Width = 30 });
                        ingredientPanel.Children.Add(new TextBox
                        {
                            Text = ingredient.ItemId,
                            Width = 100,
                            Background = new SolidColorBrush(Color.FromRgb(46, 50, 66)),
                            Foreground = Brushes.White,
                            BorderBrush = Brushes.Gray,
                            Margin = new Thickness(0, 0, 5, 0)
                        });

                        // Display Quantity of the ingredient
                        ingredientPanel.Children.Add(new TextBlock { Text = "Qty:", Foreground = Brushes.LightGray, Width = 30 });
                        ingredientPanel.Children.Add(new TextBox
                        {
                            Text = ingredient.Quantity.ToString(),
                            Width = 100,
                            Background = new SolidColorBrush(Color.FromRgb(46, 50, 66)),
                            Foreground = Brushes.White,
                            BorderBrush = Brushes.Gray
                        });

                        // Add the ingredientPanel to the main itemPanel
                        itemPanel.Children.Add(ingredientPanel);
                    }

                    // Add the StackPanel to the parent container
                    recipeContainer.Children.Add(itemPanel);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error reading file: {ex.Message}");
            }
        }

        private void recipecontentjson_TextChanged(object sender, TextChangedEventArgs e)
        {
            // Logic for recipe content JSON changes if needed
        }

        private void saveBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Collect updated items from UI
                var updatedItems = new List<Item>();

                foreach (StackPanel itemPanel in recipeContainer.Children)
                {
                    var item = new Item
                    {
                        IngredientList = new List<Ingredient>()  // Initialize IngredientList here
                    };
                    Ingredient currentIngredient = null;  // Temporary ingredient variable to store each ingredient's data

                    // Traverse through each child element in itemPanel
                    foreach (var child in itemPanel.Children)
                    {
                        if (child is TextBox textBox)
                        {
                            var labelText = ((TextBlock)itemPanel.Children[itemPanel.Children.IndexOf(textBox) - 1]).Text;
                            switch (labelText)
                            {
                                case "Item ID:":
                                    item.ItemId = textBox.Text;
                                    break;
                                case "Quantity:":
                                    item.Quantity = int.TryParse(textBox.Text, out var quantity) ? quantity : 0;
                                    break;
                                case "Craft Time:":
                                    item.CraftTime = int.TryParse(textBox.Text, out var craftTime) ? craftTime : 0;
                                    break;
                                case "ID:":
                                    currentIngredient = new Ingredient { ItemId = textBox.Text };
                                    item.IngredientList.Add(currentIngredient);  // Add ingredient to list
                                    break;
                                case "Qty:":
                                    if (currentIngredient != null)
                                    {
                                        currentIngredient.Quantity = int.TryParse(textBox.Text, out var qty) ? qty : 0;
                                    }
                                    break;
                            }
                        }
                    }

                    updatedItems.Add(item);
                }

                // Serialize the updated items list to JSON and save it to the file
                var json = JsonSerializer.Serialize(updatedItems, new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText(recipepath.Text, json);

                MessageBox.Show("Changes saved successfully!");

                recipecontentjson.Text = File.ReadAllText(recipepath.Text);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving file: {ex.Message}");
            }
        }

        private void recipeAddBtn_Click(object sender, RoutedEventArgs e)
        {
            // Logic for adding a new recipe if needed
        }
    }
}
