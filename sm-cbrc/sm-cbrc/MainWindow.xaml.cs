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
            dialog.Title = "Choose a file";

            dialog.ShowDialog();
            recipepath.Text = dialog.FileName;
        }

        private void recipepath_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                recipecontentjson.Text = File.ReadAllText(recipepath.Text);
            }
            catch { }
        }
    }
}
