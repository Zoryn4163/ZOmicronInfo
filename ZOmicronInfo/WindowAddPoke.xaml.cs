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

namespace ZOmicronInfo
{
    public partial class WindowAddPoke : Window
    {
        public const string PlaceholderFilter = "Filter by Pokemon ID or InternalName";
        public Pokemon SelectedPokemon { get; private set; }

        public WindowAddPoke()
        {
            SelectedPokemon = null;
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            txtbxFilter.Text = PlaceholderFilter;

            /*foreach (Pokemon p in Program.ContainedPokeData.AllPokemon)
            {
                //lstbxPokes.Items.Add(p);
            }*/

            var ls = lstbxPokes.Items.SourceCollection.OfType<Pokemon>().FirstOrDefault(x => x.Id == Program.LastId);

            if (ls != null)
            {
                lstbxPokes.ScrollIntoView(ls);
            }
        }

        private void txtbxFilter_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(txtbxFilter.Text))
                txtbxFilter.Text = PlaceholderFilter;
        }

        private void txtbxFilter_GotFocus(object sender, RoutedEventArgs e)
        {
            if (txtbxFilter.Text == PlaceholderFilter)
                txtbxFilter.Text = string.Empty;
        }

        private void txtbxFilter_TextChanged(object sender, TextChangedEventArgs e)
        {
            lstbxPokes.Items.Clear();
            if (txtbxFilter.Text == PlaceholderFilter || string.IsNullOrEmpty(txtbxFilter.Text))
            {
                foreach (Pokemon p in Program.ContainedPokeData.AllPokemon)
                {
                    lstbxPokes.Items.Add(p);
                }
                return;
            }


            foreach (Pokemon p in Program.ContainedPokeData.AllPokemon.Where(x => x.InternalName.Contains(txtbxFilter.Text.ToUpper()) || x.Id == txtbxFilter.Text.AsInt()))
            {
                lstbxPokes.Items.Add(p);
            }
        }

        private void btnSubmit_Click(object sender, RoutedEventArgs e)
        {
            if (lstbxPokes.SelectedItem is Pokemon)
            {
                SelectedPokemon = (Pokemon) lstbxPokes.SelectedItem;
                Close();
            }
            else
            {
                if (lstbxPokes.SelectedIndex >= 0)
                    MessageBox.Show("The specified item is not of type 'Pokemon'.", "Error");
            }

            Close();
        }

        private void lstbxPokes_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (lstbxPokes.SelectedItem != null)
                btnSubmit_Click(sender, null);
        }

        private void lstbxPokes_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Program.LastId = ((Pokemon) (lstbxPokes.SelectedItem)).Id;
        }
    }
}
