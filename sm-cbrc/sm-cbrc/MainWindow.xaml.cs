using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

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
        public string modsfolder = @"C:\Users";

        public MainWindow()
        {
            InitializeComponent();
            recipepath.Text = "";
        }

        private void ItemList_Click(object sender, RoutedEventArgs e)
        {
            AdditionalWindows adw = new AdditionalWindows();
            adw.OpenChoiceWin();
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
        }

        private void NewRecipe_Click(object sender, RoutedEventArgs e)
        {
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
                if (new FileInfo(dialog.FileName).Length == 0)
                {
                    File.WriteAllText(dialog.FileName, "[]");
                }
                recipepath.Text = dialog.FileName;
            }
        }

        private void recipepath_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateRecipes();
            bool isdir = File.Exists(recipepath.Text);
            saveBtn.IsEnabled = isdir;
            addItemBtn.IsEnabled = isdir;
        }

        public void UpdateRecipes()
        {
            try
            {
                recipecontentjson.Text = File.ReadAllText(recipepath.Text);
                LoadRecipes(recipepath.Text);
            }
            catch
            { }
        }

        void LoadRecipes(string filePath)
        {
            try
            {
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };

                List<Item> items;
                using (StreamReader r = new StreamReader(filePath))
                {
                    string json = r.ReadToEnd();
                    items = JsonSerializer.Deserialize<List<Item>>(json, options);
                }

                recipeContainer.Children.Clear();

                foreach (var item in items)
                {
                    StackPanel itemPanel = new StackPanel
                    {
                        Margin = new Thickness(10)
                    };
                    Button removeItemBtn = new Button
                    {
                        Content = "Remove Item",
                        Margin = new Thickness(0, 5, 0, 5)
                    };

                    removeItemBtn.Click += (s, ev) => recipeContainer.Children.Remove(itemPanel);
                    itemPanel.Children.Add(removeItemBtn);

                    var itemIdTextBox = new TextBox
                    {
                        Text = item.ItemId,
                        Width = 200,
                        Background = new SolidColorBrush(Color.FromRgb(46, 50, 66)),
                        Foreground = Brushes.White,
                        BorderBrush = Brushes.Gray
                    };
                    itemIdTextBox.ContextMenu = CreateContextMenu(itemIdTextBox);
                    itemPanel.Children.Add(new TextBlock { Text = "Item ID:", Foreground = Brushes.LightGray });
                    itemPanel.Children.Add(itemIdTextBox);

                    itemPanel.Children.Add(new TextBlock { Text = "Quantity:", Foreground = Brushes.LightGray });
                    itemPanel.Children.Add(new TextBox
                    {
                        Text = item.Quantity.ToString(),
                        Width = 200,
                        Background = new SolidColorBrush(Color.FromRgb(46, 50, 66)),
                        Foreground = Brushes.White,
                        BorderBrush = Brushes.Gray
                    });

                    itemPanel.Children.Add(new TextBlock { Text = "Craft Time:", Foreground = Brushes.LightGray });
                    itemPanel.Children.Add(new TextBox
                    {
                        Text = item.CraftTime.ToString(),
                        Width = 200,
                        Background = new SolidColorBrush(Color.FromRgb(46, 50, 66)),
                        Foreground = Brushes.White,
                        BorderBrush = Brushes.Gray
                    });

                    itemPanel.Children.Add(new TextBlock { Text = "Ingredients:", Foreground = Brushes.LightGray, Margin = new Thickness(0, 10, 0, 5) });
                    Button addIngredientBtn = new Button
                    {
                        Content = "Add Ingredient",
                        Margin = new Thickness(0, 5, 0, 5)
                    };
                    addIngredientBtn.Click += (s, ev) => AddIngredientBtn_Click(itemPanel);
                    itemPanel.Children.Add(addIngredientBtn);

                    foreach (var ingredient in item.IngredientList)
                    {
                        StackPanel ingredientPanel = new StackPanel
                        {
                            Orientation = Orientation.Horizontal,
                            Margin = new Thickness(5)
                        };
                        Button removeIngredientBtn = new Button
                        {
                            Content = "Remove",
                            Width = 100,
                            Margin = new Thickness(0, 0, 5, 0)
                        };
                        removeIngredientBtn.Click += (s, ev) => itemPanel.Children.Remove(ingredientPanel);
                        ingredientPanel.Children.Add(removeIngredientBtn);

                        var ingredientIdTextBox = new TextBox
                        {
                            Text = ingredient.ItemId,
                            Width = 100,
                            Background = new SolidColorBrush(Color.FromRgb(46, 50, 66)),
                            Foreground = Brushes.White,
                            BorderBrush = Brushes.Gray,
                            Margin = new Thickness(0, 0, 5, 0)
                        };
                        ingredientIdTextBox.ContextMenu = CreateContextMenu(ingredientIdTextBox);
                        ingredientPanel.Children.Add(new TextBlock { Text = "ID:", Foreground = Brushes.LightGray, Width = 30 });
                        ingredientPanel.Children.Add(ingredientIdTextBox);

                        ingredientPanel.Children.Add(new TextBlock { Text = "Qty:", Foreground = Brushes.LightGray, Width = 30 });
                        ingredientPanel.Children.Add(new TextBox
                        {
                            Text = ingredient.Quantity.ToString(),
                            Width = 100,
                            Background = new SolidColorBrush(Color.FromRgb(46, 50, 66)),
                            Foreground = Brushes.White,
                            BorderBrush = Brushes.Gray
                        });

                        itemPanel.Children.Add(ingredientPanel);
                    }

                    recipeContainer.Children.Add(itemPanel);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error reading file: {ex.Message}");
            }
        }

        private ContextMenu CreateContextMenu(TextBox targetTextBox)
        {
            AdditionalWindows adw = new AdditionalWindows();
            var contextMenu = new ContextMenu();
            var menuItem = new MenuItem { Header = "Choose Item ID" };
            menuItem.Click += (s, e) =>
            {
                string selectedItemId = adw.OpenChoiceWin();
                if (!string.IsNullOrEmpty(selectedItemId))
                {
                    targetTextBox.Text = selectedItemId;
                }
            };
            contextMenu.Items.Add(menuItem);
            return contextMenu;
        }

        private void recipecontentjson_TextChanged(object sender, TextChangedEventArgs e)
        {
        }

        private void SaveRecipe()
        {
            try
            {
                var updatedItems = new List<Item>();

                foreach (StackPanel itemPanel in recipeContainer.Children)
                {
                    var item = new Item
                    {
                        IngredientList = new List<Ingredient>()
                    };

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
                            }
                        }
                    }

                    foreach (var child in itemPanel.Children.OfType<StackPanel>())
                    {
                        var ingredientPanel = child;
                        var ingredient = new Ingredient();

                        foreach (var ingredientChild in ingredientPanel.Children)
                        {
                            if (ingredientChild is TextBox textBox)
                            {
                                var labelText = ((TextBlock)ingredientPanel.Children[ingredientPanel.Children.IndexOf(textBox) - 1]).Text;
                                switch (labelText)
                                {
                                    case "ID:":
                                        ingredient.ItemId = textBox.Text;
                                        break;
                                    case "Qty:":
                                        ingredient.Quantity = int.TryParse(textBox.Text, out var qty) ? qty : 0;
                                        break;
                                }
                            }
                        }

                        item.IngredientList.Add(ingredient);
                    }

                    updatedItems.Add(item);
                }

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

        private void saveBtn_Click(object sender, RoutedEventArgs e)
        {
            SaveRecipe();
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.S && (Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control)
            {
                SaveRecipe();
            }
        }

        private void SaveRecipe_Click(object sender, RoutedEventArgs e)
        {
            SaveRecipe();
        }

        private void AddItemBtn_Click(object sender, RoutedEventArgs e)
        {
            StackPanel itemPanel = new StackPanel
            {
                Margin = new Thickness(10)
            };
            Button removeItemBtn = new Button
            {
                Content = "Remove Item",
                Margin = new Thickness(0, 5, 0, 5)
            };
            removeItemBtn.Click += (s, ev) => recipeContainer.Children.Remove(itemPanel);
            itemPanel.Children.Add(removeItemBtn);

            var itemIdTextBox = new TextBox
            {
                Width = 200,
                Background = new SolidColorBrush(Color.FromRgb(46, 50, 66)),
                Foreground = Brushes.White,
                BorderBrush = Brushes.Gray
            };
            itemIdTextBox.ContextMenu = CreateContextMenu(itemIdTextBox);
            itemPanel.Children.Add(new TextBlock { Text = "Item ID:", Foreground = Brushes.LightGray });
            itemPanel.Children.Add(itemIdTextBox);

            itemPanel.Children.Add(new TextBlock { Text = "Quantity:", Foreground = Brushes.LightGray });
            itemPanel.Children.Add(new TextBox
            {
                Width = 200,
                Background = new SolidColorBrush(Color.FromRgb(46, 50, 66)),
                Foreground = Brushes.White,
                BorderBrush = Brushes.Gray
            });

            itemPanel.Children.Add(new TextBlock { Text = "Craft Time:", Foreground = Brushes.LightGray });
            itemPanel.Children.Add(new TextBox
            {
                Width = 200,
                Background = new SolidColorBrush(Color.FromRgb(46, 50, 66)),
                Foreground = Brushes.White,
                BorderBrush = Brushes.Gray
            });

            itemPanel.Children.Add(new TextBlock { Text = "Ingredients:", Foreground = Brushes.LightGray, Margin = new Thickness(0, 10, 0, 5) });
            Button addIngredientBtn = new Button
            {
                Content = "Add Ingredient",
                Margin = new Thickness(0, 5, 0, 5)
            };
            addIngredientBtn.Click += (s, ev) => AddIngredientBtn_Click(itemPanel);
            itemPanel.Children.Add(addIngredientBtn);

            recipeContainer.Children.Insert(0, itemPanel);
        }

        private void AddIngredientBtn_Click(StackPanel itemPanel)
        {
            StackPanel ingredientPanel = new StackPanel
            {
                Orientation = Orientation.Horizontal,
                Margin = new Thickness(5)
            };
            Button removeIngredientBtn = new Button
            {
                Content = "Remove",
                Width = 100,
                Margin = new Thickness(0, 0, 5, 0)
            };
            removeIngredientBtn.Click += (s, ev) => itemPanel.Children.Remove(ingredientPanel);
            ingredientPanel.Children.Add(removeIngredientBtn);

            var ingredientIdTextBox = new TextBox
            {
                Width = 100,
                Background = new SolidColorBrush(Color.FromRgb(46, 50, 66)),
                Foreground = Brushes.White,
                BorderBrush = Brushes.Gray,
                Margin = new Thickness(0, 0, 5, 0)
            };
            ingredientIdTextBox.ContextMenu = CreateContextMenu(ingredientIdTextBox);
            ingredientPanel.Children.Add(new TextBlock { Text = "ID:", Foreground = Brushes.LightGray, Width = 30 });
            ingredientPanel.Children.Add(ingredientIdTextBox);

            ingredientPanel.Children.Add(new TextBlock { Text = "Qty:", Foreground = Brushes.LightGray, Width = 30 });
            ingredientPanel.Children.Add(new TextBox
            {
                Width = 100,
                Background = new SolidColorBrush(Color.FromRgb(46, 50, 66)),
                Foreground = Brushes.White,
                BorderBrush = Brushes.Gray
            });

            itemPanel.Children.Insert(9, ingredientPanel);
        }
    }
}
