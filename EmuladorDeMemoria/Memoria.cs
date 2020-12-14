using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace EmuladorDeMemoria
{
    // Se define la clase Memoria que contiene los atributos y los metodos necesarios para el procesamiento de los procesos
    public class Memoria
    {
        
        public Segmento[] vector { set; get; } // Se crea un vector de segmentos del tamaño de la memoria
        public int Tamanno { set; get; } // se almacena el tamaño de la memoria

        //Constructor el cual tiene como parametro el tamaño
        public Memoria(int tamanno)
        {
            Tamanno = tamanno;
            vector = new Segmento[tamanno];
            InicializarMemoria();

        }
         // Metodo que llena de objetos tipo Segmento cada espacio del arreglo
        public void InicializarMemoria()
        {
            for (int i=0; i < Tamanno; i++)
            {
                Segmento s = new Segmento("Libre",0,0,Color.White);
                vector[i] = s;
            }
        }
         
        // Metodo que asigna un proceso de un determinado tamaño en una determinada posición
        public void AlmacenarProceso (int posicion, int tamannoProceso, string nombreProceso)
        {
            Color color = ColorRamdom();
            for (int i = 0; i < tamannoProceso; i++)
            {
                vector[i + posicion-1].NombreProceso = nombreProceso;
                vector[i + posicion-1].PosicionInicio = posicion;
                vector[i + posicion-1].TamannoProceso = tamannoProceso;
                vector[i + posicion - 1].Color = color;
            }
        }

        // Libera un determinado proceso del vector
        public void LiberaProceso(int posicion, int tamannoProceso)
        {
            for (int i = 0; i < tamannoProceso; i++)
            {
                vector[i + posicion-1].NombreProceso = "Libre";
                vector[i + posicion-1].PosicionInicio = 0;
                vector[i + posicion-1].TamannoProceso = 0;
                vector[i + posicion - 1].Color = Color.White;
            }
        }

        // Metodo que devuelve una lista de tipo Segmento con los proceso ya asignados
        public List<Segmento> ListaProcesos()
        {
            List<Segmento> resultado = new List<Segmento>();
            Segmento ultimo = new Segmento("Libre", 0 , 0);

            for (int i = 0; i < Tamanno; i++)
            {
               if (vector[i].NombreProceso != ultimo.NombreProceso)
                {
                    if (vector[i].NombreProceso != "Libre")
                    {
                        resultado.Add(vector[i]);
                    }
                    ultimo = vector[i];
                }
            }
            return resultado;
        }

        // Metodo que recorre el vector y crea una lista con los espacios en blanco de la memoria
        public List<Segmento> ListaEspaciosVacios()
        {
            List<Segmento> resultado = new List<Segmento>();
            Segmento vacio;
            int inicio = 0;
            int tamanno = 0;

            for (int i = 0; i < Tamanno; i++)
            {
                if (vector[i].NombreProceso == "Libre")
                {
                    if (i != Tamanno - 1)
                    {
                        if (inicio == 0)
                        {
                            inicio = i + 1;
                            tamanno++;
                        }
                        else tamanno++;
                    }
                    else
                    {
                        if (inicio == 0)
                        {
                            tamanno++;
                            vacio = new Segmento("Libre", 30, tamanno);
                            resultado.Add(vacio);
                        }
                        else { 
                            tamanno++;
                            vacio = new Segmento("Libre", inicio, tamanno);
                            resultado.Add(vacio);
                        }
                    }
                }

                else
                {
                    if (inicio != 0)
                    {
                        vacio = new Segmento("", 0, 0);
                        vacio.NombreProceso = "Libre";
                        vacio.PosicionInicio = inicio;
                        vacio.TamannoProceso = tamanno;
                        resultado.Add(vacio);
                        inicio = 0;
                        tamanno = 0;
                    }
                }
            }
            return resultado;
        }

        // Metodo que devuelve un color de forma aleatoria
        private Color ColorRamdom()
        {
            Random r = new Random();
            Color BackColor = Color.FromArgb(r.Next(0, 256), r.Next(0, 256), r.Next(0, 256));
            return BackColor;
        }
    }
}
