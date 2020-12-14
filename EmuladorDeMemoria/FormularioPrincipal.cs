using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EmuladorDeMemoria
{
    public partial class FormularioPrincipal : Form
    {
        Memoria memoria; 
        int tamannoMemoria; // almacena tamaño de la memoria
        Segmento ultimoProceso; // utilizado para el contro del untimo proceso asignado
        PanelGrafico PG = new PanelGrafico();
        public FormularioPrincipal()
        {
            InitializeComponent(); // Inicializa los componentes del formulario
            ultimoProceso = new Segmento("", 0, 0,Color.White); // Inicializa el segmento
        }

        // Llena el ComboBox con la lista de los procesos asignados
        private void LlenaCombo()
        {
            foreach (Segmento dato in memoria.ListaProcesos())
            {
                comboBox1.Items.Add(dato);
            }
        }

        // Se encarga del trabajo de liberacion de proceso de la memoria
        private void BorraProceso(Segmento seg)
        {
            Segmento s = seg;
            memoria.LiberaProceso(s.PosicionInicio,s.TamannoProceso);
            comboBox1.Items.Clear();
            LlenaCombo();
            dataGridView1.DataSource = memoria.ListaProcesos();
            dataGridView2.DataSource = memoria.ListaEspaciosVacios();
        }

        // Metodo del boton que cierra la aplicacion
        private void bunifuImageButton2_Click(object sender, EventArgs e)
        {
            Dispose();
        }

        // Metodo del boton que minimiza el formulario
        private void bunifuImageButton3_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        // Metodo para la cración de la memoria
        private void btnCrear_Click(object sender, EventArgs e)
        {
            if(cmbSeleccion.Text == "")
            {
                MessageBox.Show("Es necesario seleccionar primero un Algoritmo de Asignación", "Seleccionar Algoritmo", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            
            else if (txtTam.Text == "") 
            {
                MessageBox.Show("Es necesario asignar un tamaño a la memoria", "Tamaño de Memoria", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
           
            else { 

                tamannoMemoria = int.Parse(txtTam.Text);
                memoria = new Memoria(tamannoMemoria);
                LlenarControles();
                grbCreacion.Enabled = false;
                grbAsignacion.Enabled = true;
                grbLiberacion.Enabled = true;
                PG.GenerarPanel(panel3, tamannoMemoria);
            }
        }

        // Metodo encargado de llenar los dataGrids y combobox
        private void LlenarControles()
        {
            comboBox1.Items.Clear();
            dataGridView1.DataSource = memoria.ListaProcesos();
            dataGridView1.Columns[0].HeaderText = "Nom";
            dataGridView1.Columns[1].HeaderText = "Pos";
            dataGridView1.Columns[2].HeaderText = "Tam";
            dataGridView1.Columns[3].Visible = false;

            dataGridView2.DataSource = memoria.ListaEspaciosVacios();
            dataGridView2.Columns[0].HeaderText = "Nom";
            dataGridView2.Columns[1].HeaderText = "Pos";
            dataGridView2.Columns[2].HeaderText = "Tam";
            dataGridView2.Columns[3].Visible = false;
            LlenaCombo();
        }

        // Metodos de asignacion de memoria 

        //Algoritmo de asignación de Primer ajuste
        public void PrimerAjuste(Segmento Proceso)
        {
            int pos = -1; // variable para asignar la posición donde se almacenará el proceso
            bool encontroPos = false; // variable para controlar si encontro un espacio
            int tam = Proceso.TamannoProceso; // almacena el tamaño del proceso a almacenar

            // Recorremos la lista de espacios vacios desde el inicio
            foreach (Segmento espaciosVacios in memoria.ListaEspaciosVacios())
            {
                // en el momento que encuentre un espacio, almacena la posicion de este
                if (espaciosVacios.TamannoProceso >= tam)
                {
                    pos = espaciosVacios.PosicionInicio;
                    encontroPos = true;
                    break;
                }
            }
            // verificamos que haya encontrado un espacio y se envia a almacenar en memoria
            if (encontroPos == true)
            {
                memoria.AlmacenarProceso(pos, tam, Proceso.NombreProceso);
                LlenarControles();
            }
            // Sino existe espacio entonces se notifica al usuario.
            else
            {
                MessageBox.Show("No es posible encontrar un espacio para el proceso", "Memoria Llena", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        //Algoritmo de asignación de Mejor ajuste
        public void MejorAjuste(Segmento Proceso)
        {
            int mejorPos = tamannoMemoria; // variable que almacena la mejor posición
            int mejorDiferencia = tamannoMemoria;
            bool encontroPos = false; // variable para controlar si encontro un espacio
            int tam = Proceso.TamannoProceso; // almacena el tamaño del proceso a almacenar

            // Recorremos la lista de espacios vacios desde el inicio
            foreach (Segmento espaciosVacios in memoria.ListaEspaciosVacios())
            {
                // En el momento que encuentre un espacio, evalua si es el mas pequeño
                if (espaciosVacios.TamannoProceso >= tam)
                {
                    int diferencia = espaciosVacios.TamannoProceso - tam; // Diferencia entre el espacio vacio en memoria y el proceso al almacenar

                    if (diferencia <= mejorDiferencia)
                    {
                        mejorDiferencia = diferencia;
                        mejorPos = espaciosVacios.PosicionInicio;
                        encontroPos = true;
                    }
                }
            }
            if (encontroPos == true)
            {
                memoria.AlmacenarProceso(mejorPos, tam, Proceso.NombreProceso);
                LlenarControles();
            }
            // Sino existe espacio entonces se notifica al usuario.
            else
            {
                MessageBox.Show("No es posible encontrar un espacio para el proceso", "Memoria Llena", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        //Algoritmo de asignación de Peor ajuste
        public void PeorAjuste(Segmento Proceso)
        {
            int peorPos = tamannoMemoria; // variable que almacena la peor posición
            int peorDiferencia = 0;
            bool encontroPos = false; // variable para controlar si encontro un espacio
            int tam = Proceso.TamannoProceso; // almacena el tamaño del proceso a almacenar

            // Recorremos la lista de espacios vacios desde el inicio
            foreach (Segmento espaciosVacios in memoria.ListaEspaciosVacios())
            {
                // En el momento que encuentre un espacio, evalua si es el mas grande encontrado
                if (espaciosVacios.TamannoProceso >= tam)
                {
                    int diferencia = espaciosVacios.TamannoProceso - tam; // Diferencia entre el espacio vacio en memoria y el proceso al almacenar

                    // si el espacio en memoria que se evalua es mayor que el anterior entonces se almacena el actual
                    if (diferencia >= peorDiferencia)
                    {
                        peorDiferencia = diferencia;
                        peorPos = espaciosVacios.PosicionInicio;
                        encontroPos = true;
                    }
                }
            }
            if (encontroPos == true)
            {
                memoria.AlmacenarProceso(peorPos, tam, Proceso.NombreProceso);
                LlenarControles();
            }
            // Sino existe espacio entonces se notifica al usuario.
            else
            {
                MessageBox.Show("No es posible encontrar un espacio para el proceso", "Memoria Llena", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        public void SiguienteAjuste(Segmento Proceso)
        {
            int pos = -1; // variable para asignar la posición donde se almacenará el proceso
            bool encontroPos = false; // variable para controlar si encontro un espacio
            int tam = Proceso.TamannoProceso; // almacena el tamaño del proceso a almacenar

            // Recorremos la lista de espacios vacios desde el inicio
            foreach (Segmento espaciosVacios in memoria.ListaEspaciosVacios())
            {
                // En el momento que encuentre un espacio, evalua si es el mas grande encontrado
                if (espaciosVacios.TamannoProceso >= tam)
                {
                   
                    // Evaluamos que la posicion del recorrido sea despues del ultimo registro
                    if (espaciosVacios.PosicionInicio > ultimoProceso.PosicionInicio)
                    {
                        pos = espaciosVacios.PosicionInicio;
                        encontroPos = true;

                        break;
                    }
                }
            }

            // Sino se encontro una posicion adecuada despues del ultimo registro, empezamos de nuevo pero desde elprincipio

            if (encontroPos == false) { 


                //Recorremos la lista de nuevo pero evaluamos desde el principio de la memoria hasta la posicion de
                //ultimo registro
                foreach (Segmento espaciosVacios in memoria.ListaEspaciosVacios())
                {
                    // En el momento que encuentre un espacio, evalua si es el mas grande encontrado
                    if (espaciosVacios.TamannoProceso >= tam)
                    {

                        // si el espacio en memoria que se evalua es mayor que el anterior entonces se almacena el actual
                        if ((espaciosVacios.PosicionInicio < ultimoProceso.PosicionInicio))
                        {
                            pos = espaciosVacios.PosicionInicio;
                            encontroPos = true;
                            break;
                        }
                    }
                }

            }
            // si se encontro la posicion registramos el proceso en memoria
            if (encontroPos == true)
            {
                Segmento s = new Segmento(Proceso.NombreProceso,pos, tam);
                ultimoProceso = s;
                memoria.AlmacenarProceso(pos, tam, Proceso.NombreProceso);
                LlenarControles();
            }
            // Sino existe espacio entonces se notifica al usuario.
            else
            {
                MessageBox.Show("No es posible encontrar un espacio para el proceso", "Memoria Llena", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        //Metodo deribado del boton asignar, encargado de asignar el proceso en la memoria
        private void btnAsignar_Click(object sender, EventArgs e)
        {
            if (txtTamPro.Text == "")
            {
                MessageBox.Show("Es necesario asignar un tamaño al proceso", "Tamaño de Proceso", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            else if (txtNombrePro.Text == "")
            {
                MessageBox.Show("Es necesario asignar un nombre al proceso", "Nombre de Proceso", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

            }
            else if (ExisteProceso(txtNombrePro.Text) ==true) {
                MessageBox.Show("Ya existe un proceso con el mismo nombre", "Nombre de Proceso", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }

            else
            { 
                string seleccion = cmbSeleccion.Text;
                Segmento s = new Segmento(txtNombrePro.Text, 0, int.Parse(txtTamPro.Text));
            
                switch (seleccion)
                {
                    case "Primer Ajuste":
                        PrimerAjuste(s);
                        break;
                    case "Siguiente Ajuste":
                        SiguienteAjuste(s);
                        break;

                    case "Mejor Ajuste":
                        MejorAjuste(s);
                        break;
                    case "Peor Ajuste":
                        PeorAjuste(s);
                        break;

                }

            PG.PersonalizarPanel(panel3,memoria);
            PG.AgregaControl(flpanel, memoria.ListaProcesos());
            }
        }

        // Metodo encargado de liberar el proceso de la memoria 
        private void btnLiberar_Click(object sender, EventArgs e)
        {
            if (comboBox1.Text != "")
            {
                BorraProceso((Segmento)comboBox1.SelectedItem);
                PG.PersonalizarPanel(panel3, memoria);
                PG.AgregaControl(flpanel, memoria.ListaProcesos());
            }

        }

        // Metodo que se ejecuta con el boton de reiniciar 
        private void btnReiniciar_Click(object sender, EventArgs e)
        {
            tamannoMemoria = 0;
            memoria.InicializarMemoria();

            LlenarControles();

            grbCreacion.Enabled = true;
            grbAsignacion.Enabled = false;
            grbLiberacion.Enabled = false;

            panel3.Controls.Clear();
            flpanel.Controls.Clear();
        }

        // Metodo para el tool tip del caja de texto de tamaño de Memoria
        private void txtTam_TextEnter(object sender, EventArgs e)
        {
            TextBox TB = (TextBox)sender;

            int VisibleTime = 2000;  //in milliseconds

            ToolTip tt = new ToolTip();
            tt.Show("Se recomienda un tamaño maximo de 1000", TB, 0, -10, VisibleTime);

        }

        // Metodo para el tool tip del caja de texto de nombre de Proceso
        private void txtNombrePro_TextEnter(object sender, EventArgs e)
        {
            TextBox TB = (TextBox)sender;

            int VisibleTime = 2000;  //in milliseconds

            ToolTip tt = new ToolTip();
            tt.Show("Se recomienda un nombre con maximo 4 caracteres", TB, 0, -10, VisibleTime);
        }

        // Metodo para validar que solo se digiten numeros
        private void soloNumeros(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
                MessageBox.Show("Este campo solo admite números", "Solo números", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        // Metodo que se ejecuta cuando se preciona una tecla en el textBox de tamaño de memoria
        private void txtTam_KeyPress(object sender, KeyPressEventArgs e)
        {
            soloNumeros(sender, e);
        }
        // Metodo que se ejecuta cuando se preciona una tecla en el textBox de tamaño de proceso
        private void txtTamPro_KeyPress(object sender, KeyPressEventArgs e)
        {
            soloNumeros(sender, e);
        }
        // Metodo que se ejecuta cuando se preciona una tecla en el textBox de nombre
        // La finalidad es que sean solo mayusculas
        private void txtNombrePro_TextChanged(object sender, EventArgs e)
        {
            txtNombrePro.CharacterCasing = CharacterCasing.Upper;
        }

        // Metodo que revisa si existe un mismo nombre de proceso a asignar
        private bool ExisteProceso(string nombre)
        {
            bool existe = false;
            foreach (Segmento dato in memoria.ListaProcesos())
            {
                if (nombre == dato.NombreProceso)
                {
                    existe = true;
                }
            }
            return existe;
        }
    }

}
