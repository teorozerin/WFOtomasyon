using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics.Eventing.Reader;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WFOtomasyon
{
    public partial class FRMMain : Form
    {
        SqlConnection baglanti = new SqlConnection("Data Source=DESKTOP-F23C5M8;Initial Catalog=OtomasyonDB;Trusted_Connection=True;MultipleActiveResultSets=true;Encrypt=False");
        public FRMMain()
        {
            InitializeComponent();
        }

        private void FRMMain_Load(object sender, EventArgs e)
        {

            GridYukle();

        }
        void GridYukle()
        {
            try
            {
                baglanti.Open();
                SqlCommand komut = new SqlCommand("Select * from Brand ", baglanti);
                DataTable dataTable = new();
                dataTable.Load(komut.ExecuteReader());
                dataGridView1.DataSource = dataTable;
                dataGridView1.Columns["ID"].Visible = false;
                dataGridView1.Columns["Name"].HeaderText = "Marka Adı";

                //MessageBox.Show("Bağlantı kuruldu...");
                baglanti.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Bağlantı kurulamadı , açıklaması: " + ex.Message);
            }
        }

        private void btnBrand_Click(object sender, EventArgs e)
        {
            if (btnBrand.Text == "MARKA EKLE")
            {
                //SqlCommand komut = new SqlCommand("insert Brand values('"+txtBrandName.Text+"')",baglanti);//SQL Injection
                SqlCommand komut = new SqlCommand("insert Brand values(@Name)", baglanti);
                komut.Parameters.AddWithValue("@Name", txtBrandName.Text);
                baglanti.Open();
                komut.ExecuteNonQuery();
                baglanti.Close();
                GridYukle();
                txtBrandName.Clear();
            }
            else
            {
                SqlCommand komut = new SqlCommand("Update Brand set Name=@Name Where id=@id", baglanti);
                komut.Parameters.AddWithValue("@Name", txtBrandName.Text);
                int seciliID = Convert.ToInt32(dataGridView1.CurrentRow.Cells["id"].Value);
                komut.Parameters.AddWithValue("@id", seciliID);
                baglanti.Open();
                komut.ExecuteNonQuery();
                baglanti.Close();
                GridYukle() ;
                btnBrand.Text = "MARKA EKLE";

            }
        }
         
        

        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {

        }

        private void silToolStripMenuItem_Click(object sender, EventArgs e)
        {
             
             DialogResult sonuc = MessageBox.Show("Silmek istediğinizden emin misiniz?",
                "DİİKKAT!....", MessageBoxButtons.YesNo);
            if (sonuc == DialogResult.Yes) {
                SqlCommand komut = new SqlCommand("Delete Brand Where id=@id", baglanti);
                komut.Parameters.AddWithValue("@Name", txtBrandName.Text);
                int seciliID = Convert.ToInt32(dataGridView1.CurrentRow.Cells["id"].Value);
                komut.Parameters.AddWithValue("@id", seciliID);
                baglanti.Open();
                komut.ExecuteNonQuery();
                baglanti.Close();
                GridYukle();
                btnBrand.Text = "MARKA EKLE";
            }
        }

        private void düzenleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int seciliID = Convert.ToInt32(dataGridView1.CurrentRow.Cells["id"].Value);
            SqlCommand komut= new("select * from Brand Where id=@id",baglanti);
            komut.Parameters.AddWithValue("@id",seciliID);
            baglanti.Open();
            SqlDataReader okuyucu = komut.ExecuteReader();
            if(okuyucu.Read()) 
            {
                txtBrandName.Text = okuyucu["Name"].ToString();
                btnBrand.Text = "MARKA GÜNCELLE";
            }
            baglanti.Close() ;
        }
    }
}
