using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
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
using WpfAnimatedGif;
using Image = System.Windows.Controls.Image;

namespace ZOmicronInfo
{
    /// <summary>
    /// Interaction logic for PokeDataControl.xaml
    /// </summary>
    public partial class PokeDataControl : UserControl
    {
        public Pokemon Poke { get; private set; }

        public PokeDataControl(Pokemon p)
        {
            Poke = p;
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            txtbxName.Text = Poke.Name;
            txtbxInternal.Text = Poke.InternalName;
            txtbxId.Text = Poke.Id.ToString();
            txtbxType1.Text = Poke.Type1.ToString();
            txtbxType2.Text = Poke.Type2.ToString();

            txtbxBaseStats.Text = $"Base Stats: {Poke.BaseStats.NiceString} (Total: {Poke.BaseStats.Total})";
            txtbxEVsExp.Text = $"EV Gain: {Poke.EffortPoints.NiceString}\tEXP/LVL: {Poke.BaseExp}";

            txtbxAbilities.Text = $"Abilities: {Poke.Abilities.ToSingular()}";
            txtbxHiddenAbility.Text = $"Hidden: {Poke.HiddenAbility}";

            txtbxMoves.Text = $"Moves: {Poke.Moves.ToSingular()}";
            txtbxEggMoves.Text = $"Egg Moves: {Poke.EggMoves.ToSingular()}";

            txtbxEvolutions.Text = $"Evolutions: {Poke.Evolutions.ToSingular()}";

            txtbxEggGroups.Text = $"Egg Groups: {Poke.Compatability.ToSingular()}";
            txtbxEggSteps.Text = $"Egg Steps: {Poke.StepsToHatch}";
            txtbxGenderRate.Text = $"Gender Rate: {Poke.GenderRate}";
            txtbxGrowthRate.Text = $"Growth Rate: {Poke.GrowthRate}";
            txtbxHappiness.Text = $"Base Happiness: {Poke.Happiness}";

            txtbxUselessInfo.Text = $"Rareness: {Poke.Rareness}\tHeight: {Poke.Height}\tWeight: {Poke.Weight}\tColour: {Poke.Colour}\tHabitat: {Poke.Habitat}\tKind: {Poke.Kind}\tRegional IDs: {Poke.RegionalNumbers.ToSingular()}";

            txtbxPokedexEntry.Text = $"Pokedex: {Poke.Pokedex}";

            SetPokeImage(imgFront, 0);
            SetPokeImage(imgBack, 1);
            SetPokeImage(imgFrontShiny, 2);
            SetPokeImage(imgBackShiny, 3);
            SetPokeImage(imgShadow, 4);
        }

        void SetPokeImage(Image i, byte b)
        {
            try
            {
                string s = Path.Combine(Program.GraphicsPath, Poke.IdString + (b == 0 ? "" : b == 1 ? "b" : b == 2 ? "s" : b == 3 ? "sb" : b == 4 ? "_shadow" : b == 5 ? "b_shadow" : ""));
                foreach (string ss in Program.GraphicsExtensions)
                {
                    if (File.Exists(s + "." + ss))
                    {
                        if (ss == "gif")
                        {
                            BitmapImage ret = new BitmapImage();
                            ret.BeginInit();
                            ret.UriSource = new Uri(s + "." + ss);
                            ret.EndInit();
                            ImageBehavior.SetAnimatedSource(i, ret);
                        }

                        i.Source = new BitmapImage(new Uri(s + "." + ss));
                    }
                }
            }
            catch
            {
                
            }
        }
    }
}
