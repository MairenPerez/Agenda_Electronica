using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace Agenda_Electronica
{
    public partial class Agenda : Form
    {
        private List<Contacto> Contactos = new List<Contacto>();
        private int indice = -1;

        public Agenda()
        {
            InitializeComponent();
        }
        private void Agenda_Load(object sender, EventArgs e)
        {
            try
            {
                StreamReader lector = new StreamReader("Agenda.txt");
                string linea;

                while((linea = lector.ReadLine()) != null)
                {
                    int posicion;
                    Contacto persona = new Contacto();
                    posicion = linea.IndexOf("|");
                    persona.Nombre = linea.Substring(0, posicion);
                    linea = linea.Substring(posicion + 1);
                    posicion = linea.IndexOf("|");
                    persona.Apellido = linea.Substring(0, posicion);
                    linea = linea.Substring(posicion + 1);
                    posicion = linea.IndexOf("|");
                    persona.Telefono = linea.Substring(0, posicion);
                    linea = linea.Substring(posicion + 1);
                    posicion = linea.IndexOf("|");
                    persona.Correo = linea.Substring(0, posicion);
                    Contactos.Add(persona);
                }
                lector.Close();
                actualizaVista();
            }
            catch(Exception ex)
            {
                Console.WriteLine("Exception:" + ex.Message);
            }
            finally
            {
                Console.WriteLine("Ejecución finalizada");
            }
        }
        private void btnAgregar_Click(object sender, EventArgs e)
        {
            Contacto persona = new Contacto();
            persona.Nombre = txtNombre.Text;
            persona.Apellido = txtApellidos.Text;
            persona.Telefono = txtTelefono.Text;
            persona.Correo = txtEmail.Text;
            if (indice > -1)
            {
                Contactos[indice] = persona;
                indice = -1;
            }
            else
            {
               Contactos.Add(persona);
            }
            actualizaVista();
            limpiarCampos();
        }

        private void actualizaVista()
        {
            dgvContactos.DataSource = null;
            dgvContactos.DataSource = Contactos;
            dgvContactos.ClearSelection();
        }

        private void dgvContactos_DoubleClick(object sender, EventArgs e)
        {
            DataGridViewRow renglon = dgvContactos.SelectedRows[0];
            indice = dgvContactos.Rows.IndexOf(renglon);

            Contacto persona = Contactos[indice];
            txtNombre.Text = persona.Nombre;
            txtApellidos.Text = persona.Apellido;
            txtTelefono.Text = persona.Telefono;
            txtEmail.Text = persona.Correo;
        }

        private void limpiarCampos()
        {
            txtNombre.Text = null;
            txtApellidos.Text = null;
            txtTelefono.Text = null;
            txtEmail.Text = null;
        }

        private void btnBorrar_Click(object sender, EventArgs e)
        {
            if (indice > -1)
            {
                Contactos.RemoveAt(indice);
                actualizaVista();
                limpiarCampos();
                indice = -1;
            } 
            else
            {
                MessageBox.Show("Seleccione el registro a eliminar");
            }
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            TextWriter Escribir = new StreamWriter("Agenda.txt");
            foreach (Contacto persona in Contactos)
            {
                Escribir.WriteLine(persona.Nombre + "|" + persona.Apellido + "|" + persona.Telefono + "|" + persona.Correo + "|");
            }
            Escribir.Close();
            MessageBox.Show("Contactos guardados");
        }
    }
}
