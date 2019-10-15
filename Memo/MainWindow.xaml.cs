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
        
        private const int COLUMNAS = 4;
        private List<char> lista;
        public MainWindow()
        {
            InitializeComponent();            
        }

        //Evento para cuando hacemos Click en el boton iniciar.
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //Limpiamos las filas y columnas, para que no se añadan nuevas dentro de las otras
            GridCartas.RowDefinitions.Clear();
            GridCartas.ColumnDefinitions.Clear();
            lista = new List<char>() { 'N', 'N', '&', '&', 'v', 'v', 'Y', 'Y', 'b', 'b', '~', '~', '!', '!', 'O', 'O', 'P', 'P', 'w', 'w' };
            //Vemos que boton esta Cheked y añadimos el numero de filas que tiene que tener.
            //RemoveRange para borrar los valores de la lista que no necesito.
            if(Facil.IsChecked == true)
            {
                lista.RemoveRange(12, 8);
                CrearFilasYColumnas(3);
            }               
            if (Media.IsChecked == true)
            {
                lista.RemoveRange(16, 4);
                CrearFilasYColumnas(4);
            }
                
            if (Alta.IsChecked == true)
                CrearFilasYColumnas(5);
            
        }

        private void CrearFilasYColumnas(int numeroFilas)
        {
            //Definimos el numero de Columnas que tiene el Grid
            for (int i = 0; i < COLUMNAS; i++)
            {
                GridCartas.ColumnDefinitions.Add(new ColumnDefinition());
            }

            //Definimos el numero de Filas que tiene el Grid
            for (int i = 0; i < numeroFilas; i++)
            {
                GridCartas.RowDefinitions.Add(new RowDefinition());
            }            

            //Introducimos las filas y las columnas al grid.
            for (int i = 0; i < numeroFilas; i++)
            {
                for (int j = 0; j < COLUMNAS; j++)
                {
                    CrearCartas(i, j);
                }
            }
        }
        
        //Definimos el aspecto de las cartas que se introduciran en cada celda.
        private void CrearCartas(int filas, int columnas)
        {
                Border b = new Border();
                Viewbox vB = new Viewbox();
                TextBlock tB = new TextBlock();
                tB.Text = "?";
                vB.Child = tB;
                b.Child = vB;
                b.Background = Brushes.Yellow;

            //Introducimos en cada borde el evento para cuando hacemos click con el boton izq del raton encima de él.
                b.MouseLeftButtonDown += B_MouseLeftButtonDown;
            //Introducimos las cartas al grid, utilizamos el border ya que es el que contiene todo.
                GridCartas.Children.Add(b);
            //Añadimos el borde a cada fila y columna.
                Grid.SetRow(b, filas);
                Grid.SetColumn(b, columnas);           
        }

        //Cuando hacemos click en la carta, aparece otro Border diferente con el Icono
        private void B_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {            
            Viewbox vB = new Viewbox();
            TextBlock tB = new TextBlock();
            ((Border)sender).Background = Brushes.White;
            ((Border)sender).Child = vB;
            vB.Child = tB;
            
            //Para cambiar el texto del icono
            CrearCartasAleatorias(tB);
            
            tB.FontFamily = new FontFamily("Webdings");
            
        }

        /*Añadimos aleatoriamente los iconos de la lista en el texto del TextBlock y 
        una vez añadido, lo eliminamos para que no se pueda usar*/
        private void CrearCartasAleatorias(TextBlock tB)
        {
            Random seed = new Random();
            int numero = 0;

            for (int i = 0; i < COLUMNAS; i++)
            {
                numero = seed.Next(lista.Count);
                tB.Text = lista[numero].ToString();                
            }
            lista.RemoveAt(numero);
        }
    }
}
