using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Threading;
namespace Warcraft
{
    /// <summary>
    /// Логика взаимодействия для Open_char_info.xaml
    /// </summary>
    public partial class Open_char_info : Window
    {
        public Open_char_info()
        {
            InitializeComponent();
        }
        public string name;
        private void Grid_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (Formas.IsVisible == true)
            {
                // MessageBox.Show("Show");
                DispatcherTimer timer = new DispatcherTimer();
                timer.Tick += new EventHandler(hide_this);
                timer.Interval = new TimeSpan(0, 0, 5);
                timer.Start();
            }

        }
        private void hide_this(object sender, EventArgs e)
        {
            this.Close();
            // Formas.IsVisible = false;
        }
        private void hidee()
        {
            // IsVisible = false;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Process.Start("https://worldofwarcraft.com/ru-ru/character/eu/howling-fjord/" + name);
            this.Close();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Process.Start("https://raider.io/characters/eu/howling-fjord/" + name);
            this.Close();
        }


    }
}
