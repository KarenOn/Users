using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using System.Windows.Forms;
using UsersForm.Models;

namespace UsersForm
{
    public partial class Form1 : Form
    {
        string url = "https://localhost:44364/api/Users";
        public Form1()
        {
            InitializeComponent();
        }

        private async void Form1_Load(object sender, EventArgs e)
        {
            LlenarGrid();
        }

        private async Task<string> GetHttp()
        {
            WebRequest request = WebRequest.Create(url);
            WebResponse response = request.GetResponse();
            StreamReader sr = new StreamReader(response.GetResponseStream());
            return await sr.ReadToEndAsync();
        }

        private async Task<string> DeleteHttp(int id)
        {

            WebRequest request = WebRequest.Create(url + "/" + id);
            WebResponse response = request.GetResponse();
            StreamReader sr = new StreamReader(response.GetResponseStream());
            return await sr.ReadToEndAsync();
        }

        public string Send<SendUser>(string url, SendUser objectRequest, string method)
        {
            string result = "";
            try
            {
                JavaScriptSerializer js = new JavaScriptSerializer();

                //serializamos el objeto
                string json = JsonConvert.SerializeObject(objectRequest);

                //peticion
                WebRequest request = WebRequest.Create(url);
                //headers
                request.Method = method;
                request.PreAuthenticate = true;
                request.ContentType = "application/json;charset=utf-8'";
                request.Timeout = 10000; //esto es opcional

                using (var streamWriter = new StreamWriter(request.GetRequestStream()))
                {
                    streamWriter.Write(json);
                    streamWriter.Flush();
                }

                var httpResponse = (HttpWebResponse)request.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    result = streamReader.ReadToEnd();
                }
                LlenarGrid();
                Limpiar();
                MessageBox.Show("Registro guardado correctamente", "Informacion", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception e)
            {
                result = e.Message;
            }
            
            return result;
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            UsersRequest user = new UsersRequest();
            user.Nombre = txtnombre.Text;
            user.Apellido = txtapellido.Text;
            user.Email = txtemail.Text;
            string result = Send<UsersRequest>(url, user, "POST");
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            Limpiar();
        }

        public async void LlenarGrid()
        {
            string respuesta = await GetHttp();
            List<ClassLibrary1.UsersModelView> lst = JsonConvert.DeserializeObject<List<ClassLibrary1.UsersModelView>>(respuesta);
            dataGridView1.DataSource = lst;
        }

        public void Limpiar()
        {
            txtid.Text = "";
            txtnombre.Text = "";
            txtapellido.Text = "";
            txtemail.Text = "";
        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private async void btneliminar_Click(object sender, EventArgs e)
        {
            UsersRequest user = new UsersRequest();
            user.Id = int.Parse(txtid.Text);
            string result = Send<UsersRequest>(url, user, "DELETE");
            string respuesta = await DeleteHttp(user.Id);
            LlenarGrid();
            Limpiar();
            MessageBox.Show("Registro eliminado correctamente", "Informacion", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            txtid.Text = dataGridView1.CurrentRow.Cells[0].Value.ToString();
        }
    }
}
