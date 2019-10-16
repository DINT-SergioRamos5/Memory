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
using System.Windows.Threading;
using System.Threading;

namespace Memo
{
    public partial class MainWindow : Window
    {
        
        private const int COLUMNAS = 4;
        private List<char> lista;
        private const int FILAS_BAJO = 3;
        private const int FILAS_MEDIO = 4;
        private const int FILAS_ALTO = 5;
        private TextBlock comparacion1;
        private TextBlock comparacion2;
        private Border borderComparacion1;
        private DispatcherTimer timer;
        private double progresoBarra;

        public MainWindow()
        {
            InitializeComponent();
            timer = new DispatcherTimer();
        }

        //Evento para cuando hacemos Click en el boton iniciar.
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //Limpiamos las filas y columnas, para que no se añadan nuevas dentro de las otras.
            GridCartas.RowDefinitions.Clear();
            GridCartas.ColumnDefinitions.Clear();

            //Siempre que iniciemos un nivel pondremos la barra a 0.
            barraProgreso.Value = 0;

            //Inicializo el array aqui para que si cambio de nivel se vuelva a crear y no esten eliminados los caracteres.
            lista = new List<char>() { 'N', 'N', '&', '&', 'v', 'v', 'Y', 'Y', 'b', 'b', '~', '~', '!', '!', 'O', 'O', 'P', 'P', 'w', 'w' };

            //Vemos que RadioButton esta Cheked y añadimos el numero de filas que tiene que tener.
            //RemoveRange para borrar los valores de la lista que no necesito.
            //Calculamos viendo las parejas que hay en cada nivel el progreso que tendra la barra.
            if(Facil.IsChecked == true)
            {
                lista.RemoveRange(12, 8);
                CrearFilasYColumnas(FILAS_BAJO);
                progresoBarra = (double)100/((FILAS_BAJO * COLUMNAS) / 2);
            }               
            if (Media.IsChecked == true)
            {
                lista.RemoveRange(16, 4);
                CrearFilasYColumnas(FILAS_MEDIO);
                progresoBarra = (double)100 / ((FILAS_MEDIO * COLUMNAS) / 2);
            }
                
            if (Alta.IsChecked == true)
            {
                CrearFilasYColumnas(FILAS_ALTO);
                progresoBarra = (double)100 / ((FILAS_ALTO * COLUMNAS) / 2);
            }
                

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
            tB.FontFamily = new FontFamily("Webdings");
            tB.Text = "s";
            vB.Child = tB;
            b.Child = vB;
            b.Background = Brushes.Yellow;
            

            //Introducimos en cada borde el evento para cuando hacemos click con el boton izq del raton encima de él.
            b.MouseLeftButtonDown += B_MouseLeftButtonDown;
            CrearCartasAleatorias(tB);
            //Introducimos las cartas al grid, utilizamos el border ya que es el que contiene todo.
            GridCartas.Children.Add(b);

            //Añadimos el borde a cada fila y columna.
            Grid.SetRow(b, filas);
            Grid.SetColumn(b, columnas);           
        }

        //Cuando hacemos click en la carta, aparece otro Border diferente con el Icono
        private void B_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {            
            /*Todo esta creado en el meteodo CrearCartas()*/

            // Cogemos el border anteriormente creado y le cambiamos el color.
            ((Border)sender).Background = Brushes.White;

            // Creamos una variable ViewBox con la que cogemos el ViewBox creado anteriormente que es el hijo del border.
            Viewbox vB = (Viewbox)((Border)sender).Child;

            //Creamos una variable TextBlock y cogemos el hijo creado anteriormente del ViewBox que es un TextBlock donde tenemos
            //guardado el TAG.
            TextBlock tB = (TextBlock)vB.Child;

            //Cambiamos el Text que es la ? por el TAG donde tenemos guardados los simbolos.
            tB.Text = tB.Tag.ToString();

            CompararCartas(tB, ((Border)sender));                        

        }

        private void CompararCartas(TextBlock tB, Border b)
        {
            if (comparacion1 == null)
            {
                comparacion1 = tB;
                borderComparacion1 = b;
                borderComparacion1.MouseLeftButtonDown -= B_MouseLeftButtonDown;
            }
            else
            {
                comparacion2 = tB;

                if (comparacion1.Tag.ToString() != comparacion2.Tag.ToString())
                {

                    b.Background = Brushes.Yellow;
                    borderComparacion1.Background = Brushes.Yellow;
                    comparacion1.Text = "s";
                    comparacion2.Text = "s";
                    borderComparacion1.MouseLeftButtonDown += B_MouseLeftButtonDown;
                }
                else
                {
                    borderComparacion1.MouseLeftButtonDown -= B_MouseLeftButtonDown;
                    b.MouseLeftButtonDown -= B_MouseLeftButtonDown;
                    barraProgreso.Value += progresoBarra;
                    LanzarMensaje();
                }
                comparacion1 = null;
                comparacion2 = null;
            }
        }
        /*Añadimos aleatoriamente los iconos de la lista en el texto del TextBlock y 
        una vez añadido, lo eliminamos con el RemoveAT para que no se pueda usar*/
        private void CrearCartasAleatorias(TextBlock tB)
        {
            Random seed = new Random();
            int numero = 0;           
            numero = seed.Next(lista.Count);

            tB.Tag = lista[numero].ToString();

            lista.RemoveAt(numero);                       
        }

        private void LanzarMensaje()
        {
            if(barraProgreso.Value >= 100)
                MessageBox.Show("Partida finalizada.", "Memory", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}
