using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;
using System.Xml.Linq;

namespace Restorant_Sistemi
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            DialogResult onay= MessageBox.Show("Çıkmak istediğinize emin misiniz?", "Uyarı", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if(onay == DialogResult.Yes)
            {
                Application.Exit();
            }
            else
            {
                MessageBox.Show("Devam ediliyor", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

        }

        private void button1_Click(object sender, EventArgs e)
        {
            string connectionString = "Server=DESKTOP-798VB1N\\MSSQLSERVERDEV;Database=RestorantSistemi;Trusted_Connection=True;";

            string isim = textBox3.Text;
                string kullaniciAd = textBox1.Text;
                string sifre = textBox2.Text;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "INSERT INTO Tbl_Users (Name, UserName, Password) VALUES (@name, @username, @password)";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@name", isim);
                    command.Parameters.AddWithValue("@username", kullaniciAd);
                    command.Parameters.AddWithValue("@password", sifre);

                    connection.Open();

                    int result = command.ExecuteNonQuery();


                    if (result > 0)
                    {
                        MessageBox.Show("Kullanıcı başarıyla eklendi!");
                    }
                    else
                    {
                        MessageBox.Show("Kayıt eklenemedi.");
                    }
                    connection.Close();
                }
            }
        }
            


        private void label3_Click(object sender, EventArgs e)
        {

        }
    }
}
 