using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace EmuladorDeMemoria
{
    // Se define la clase la cual es utilizada para la creacion de los controles de forma dinamica
    // estos seran agregados al panel principal dependiendo del tamaño de la memoria y de los colores de cada proceso
    public class ControlesDinamicos
    {
        //Constructor vacio
        public ControlesDinamicos()
        {
        }

        // Metodo que crea y devuelve un textBox segun los argumentos que recibe
        public TextBox MemoriaTxt(string nombre, string texto, int alto, int ancho, Color colorFondo, Color colorLetra, int x, int y)
        {
            TextBox txt = new TextBox
            {
                Height = alto,
                Width = ancho,
                BackColor = colorFondo,
                ForeColor = colorLetra,
                Name = nombre,
                Text = texto,
                Location = new Point(x, y),
                BorderStyle = BorderStyle.None,
                TextAlign = HorizontalAlignment.Center,
                Font = new Font("Microsoft Sans Serif", 9, FontStyle.Bold)
            };
            return txt;
        }
    }
}
