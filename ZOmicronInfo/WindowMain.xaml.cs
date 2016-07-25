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
    public partial class WindowMain : Window
    {
        public WindowLog LogWindow { get; private set; }

        public WindowMain()
        {
            LogWindow = new WindowLog();
            LogWindow.Opacity = 0;
            LogWindow.Show();
            LogWindow.Hide();
            LogWindow.Opacity = 1;
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            tcMain.Items.Clear();
            Program.FreeConsole();
            FocusManager.SetIsFocusScope(this, true);
            FocusManager.SetFocusedElement(this, this);
        }

        private void btnViewLog_Click(object sender, RoutedEventArgs e)
        {
            LogWindow.Show();
        }

        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            Environment.Exit(0);
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Environment.Exit(0);
        }

        private void btnAddPokemon_Click(object sender, RoutedEventArgs e)
        {
            WindowAddPoke w = new WindowAddPoke();
            w.Owner = this;
            w.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            w.ShowDialog();
            Pokemon p = w.SelectedPokemon;
            if (p != null)
            {
                PokeTab pt = new PokeTab(p);
                tcMain.Items.Add(pt);
                tcMain.SelectedItem = pt;
            }
        }

        private void btnCloseTab_Click(object sender, RoutedEventArgs e)
        {
            if (tcMain.SelectedItem != null)
                tcMain.Items.Remove(tcMain.SelectedItem);
        }

        private void btnCloseAllTabs_Click(object sender, RoutedEventArgs e)
        {
            tcMain.Items.Clear();
        }
    }

    public sealed class PokeTab : TabItem
    {
        public PokeTab(Pokemon p)
        {
            MouseDown += PokeTab_MouseDown;
            ContextMenu = new ContextMenu();
            MenuItem cmClose = new MenuItem();
            cmClose.Header = "_Close";
            cmClose.Click += (sender, args) => RemoveFromTabControl();
            ContextMenu.Items.Add(cmClose);

            Header = p.ToString();

            Grid g = new Grid();
            g.Margin = new Thickness(0, 0, 0, 0);
            g.Children.Add(new PokeDataControl(p));

            AddChild(g);
        }

        private void PokeTab_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.MiddleButton == MouseButtonState.Pressed)
            {
                RemoveFromTabControl();
            }
        }

        public void RemoveFromTabControl()
        {
            if (Program.MainForm.tcMain.Items.Contains(this))
                Program.MainForm.tcMain.Items.Remove(this);
        }
    }
}
