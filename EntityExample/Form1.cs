using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace EntityExample
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        DbSinavOgrenciEntities db = new DbSinavOgrenciEntities();

        private void BtnDersListesi_Click(object sender, EventArgs e)
        {
            SqlConnection baglanti = new SqlConnection(@"Data Source=DESKTOP-2N06KQ9;Initial Catalog=DbSinavOgrenci;Integrated Security=True;TrustServerCertificate=True");
            SqlCommand komut = new SqlCommand("Select * From lessonTable", baglanti);
            SqlDataAdapter da = new SqlDataAdapter(komut);
            DataTable dt = new DataTable();
            da.Fill(dt);
            dataGridView1.DataSource = dt;
        }

        private void BtnOgrenciListele_Click(object sender, EventArgs e)
        {

            dataGridView1.DataSource = db.studentTable.ToList();
            dataGridView1.Columns[3].Visible = false;
            dataGridView1.Columns[4].Visible = false;
        }

        private void BtnNotListesi_Click(object sender, EventArgs e)
        {
            var query = from item in db.gradeTable
                        select new { item.gradeId, item.stdnt, item.exam1, item.exam2, item.exam3, item.average, item.situation };
            dataGridView1.DataSource = query.ToList();
        }

        private void BtnKaydet_Click(object sender, EventArgs e)
        {
            studentTable s = new studentTable();
            s.studentName = txtOgrenciAd.Text;
            s.studentSurname = txtOgrenciSoyad.Text;
            db.studentTable.Add(s);
            db.SaveChanges();
            MessageBox.Show("Öğrenci Listeye Eklenmiştir...");

        }

        private void BtnSil_Click(object sender, EventArgs e)
        {
            int id = Convert.ToInt32(txtOgrenciId.Text);
            var x = db.studentTable.Find(id);
            db.studentTable.Remove(x);
            db.SaveChanges();
            MessageBox.Show("Öğrenci sistemden silindi...");
        }

        private void BtnGuncelle_Click(object sender, EventArgs e)
        {
            int id = Convert.ToInt32(txtOgrenciId.Text);
            var x = db.studentTable.Find(id);
            x.studentName = txtOgrenciAd.Text;
            x.studentSurname = txtOgrenciSoyad.Text;
            x.photograph = txtOgrenciFotograf.Text;
            db.SaveChanges();
            MessageBox.Show("Öğrenci bilgileri güncellendi...");
        }

        private void btnDersEkle_Click(object sender, EventArgs e)
        {
            lessonTable l = new lessonTable();
            l.lessonName = txtDersAd.Text;
            db.lessonTable.Add(l);
            db.SaveChanges();
            MessageBox.Show("Ders Başarıyla eklendi...");

        }

        private void btnDersGuncelle_Click(object sender, EventArgs e)
        {
            int id = Convert.ToInt32(txtDersId.Text);
            var x = db.lessonTable.Find(id);
            x.lessonName = txtDersAd.Text;
            db.SaveChanges();
            MessageBox.Show("Ders Adı güncellendi...");
        }

        private void btnDersSil_Click(object sender, EventArgs e)
        {
            int id = Convert.ToInt32(txtDersId.Text);
            var x = db.lessonTable.Find(id);
            db.lessonTable.Remove(x);
            db.SaveChanges();
            MessageBox.Show("Ders başarıyla silindi...");
        }

        private void BtnProcedure_Click(object sender, EventArgs e)
        {
            dataGridView1.DataSource = db.NOTLISTESI();
        }

        private void BtnBul_Click(object sender, EventArgs e)
        {
            dataGridView1.DataSource = db.studentTable.Where(x => x.studentName.Contains(txtOgrenciAd.Text)).ToList();
        }

        private void txtOgrenciAd_TextChanged(object sender, EventArgs e)
        {
            string aranan = txtOgrenciAd.Text;
            var degerler = from item in db.studentTable
                           where item.studentName.Contains(aranan)
                           select item;
            dataGridView1.DataSource = degerler.ToList();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (radioButton1.Checked == true) 
            {
                List<studentTable> liste1 = db.studentTable.OrderBy(x => x.studentName).ToList();
                dataGridView1.DataSource=liste1;
            }
            if (radioButton2.Checked == true) 
            {
                List<studentTable> liste2 = db.studentTable.OrderByDescending(x => x.studentName).ToList();
                dataGridView1.DataSource=liste2;
            }
            if (radioButton3.Checked == true) 
            {
                List<studentTable> liste3 = db.studentTable.OrderBy(x=>x.studentName).Take(3).ToList();
                dataGridView1.DataSource=liste3;
            }
            if (radioButton4.Checked == true) 
            {
                List<studentTable> liste4 = db.studentTable.Where(x=>x.ıd==5).ToList();
                dataGridView1.DataSource=liste4;
            }
            if (radioButton5.Checked == true)
            {
                List<studentTable> liste5 = db.studentTable.Where(x=> x.studentName.StartsWith("a")).ToList();
                dataGridView1.DataSource = liste5;
            }
            if (radioButton6.Checked == true)
            {
                List<studentTable> liste6 = db.studentTable.Where(x => x.studentName.EndsWith("a")).ToList();
                dataGridView1.DataSource = liste6;
            }
            if (radioButton7.Checked == true) 
            {
                int toplam = db.studentTable.Count();
                MessageBox.Show(toplam.ToString(),"Toplam öğrenci sayısı", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void BtnJoin_Click(object sender, EventArgs e)
        {
            var sorgu = from d1 in db.gradeTable
                        join d2 in db.studentTable
                        on d1.stdnt equals d2.ıd
                        join d3 in db.lessonTable
                        on d1.less equals d3.lessonId

                        select new
                        {
                            ÖĞRENCİ = d2.studentName + " " + d2.studentSurname,
                            DERS = d3.lessonName,
                            SINAV1 = d1.exam1,
                            SINAV2 = d1.exam2,
                            SINAV3 = d1.exam3,
                            ORTALAMA = d1.average
                        };
            dataGridView1.DataSource = sorgu.ToList();
        }

    }
}
