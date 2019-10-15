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

namespace Memo
{
    public partial class MainWindow : Window
    {
        private List<char> lista = new List<char>() { 'A', 'A', 'B', 'B', 'C', 'C', 'D', 'D','E','E','F', 'F', 'G', 'G', 'H', 'H', 'I', 'I', 'J', 'J' };
        private const int COLUMNAS = 4;

        public MainWindow()
        {
            InitializeComponent();            
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            GridCartas.RowDefinitions.Clear();
            GridCartas.ColumnDefinitions.Clear();
            if(Facil.IsChecked == true)            
                CrearFilasYColumnas(3);
            if (Media.IsChecked == true)
                CrearFilasYColumnas(4);
            if (Alta.IsChecked == true)
                CrearFilasYColumnas(5);
            
        }

        private void CrearFilasYColumnas(int numeroFilas)
        {
            for (int i = 0; i < COLUMNAS; i++)
            {
                GridCartas.ColumnDefinitions.Add(new ColumnDefinition());
            }
            for (int i = 0; i < numeroFilas; i++)
            {
                GridCartas.RowDefinitions.Add(new RowDefinition());
            }            

            for (int i = 0; i < numeroFilas; i++)
            {
                for (int j = 0; j < COLUMNAS; j++)
                {
                    CrearCartas(i, j);
                }
            }
        }
        
        private void CrearCartas(int filas, int columnas)
        {
                Border b = new Border();
                Viewbox vB = new Viewbox();
                TextBlock tB = new TextBlock();
                tB.Text = "?";
                vB.Child = tB;
                b.Child = vB;
                b.Background = Brushes.Red;
                b.MouseLeftButtonDown += B_MouseLeftButtonDown;
                GridCartas.Children.Add(b);
                Grid.SetRow(b, filas);
                Grid.SetColumn(b, columnas);           
        }

        private void B_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Random seed = new Random();
            
            Viewbox vB = new Viewbox();
            TextBlock tB = new TextBlock();
            ((Border)sender).Background = Brushes.White;
            ((Border)sender).Child = vB;
            vB.Child = tB;
            //tB.Text = lista[0].ToString();
            CrearCartasAleatorias(tB);
            tB.FontFamily = new FontFamily("Webdings");
            
        }

        private void CrearCartasAleatorias(TextBlock tB)
        {
            Random seed = new Random();
            string simbolo = "";
            int numero = 0;

            lista.RemoveRange(12, 8);
            for (int i = 0; i < COLUMNAS; i++)
            {
                numero = seed.Next(lista.Count);
                tB.Text = lista[numero].ToString();
                lista.RemoveAt(numero);
            }
        }
    }
}
