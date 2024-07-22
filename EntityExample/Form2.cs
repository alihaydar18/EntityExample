using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using EntityExample;

namespace EntityExample
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        DbSinavOgrenciEntities db = new DbSinavOgrenciEntities();
        private void button1_Click(object sender, EventArgs e)
        {
            if (radioButton1.Checked == true)
            {
                var degerler = db.studentTable
                                .Where(x => x.studentName.Contains(textBox1.Text) || x.studentSurname.Contains(textBox1.Text))
                                .ToList();
                dataGridView1.DataSource = degerler.ToList();
            }
            if (radioButton2.Checked == true)
            {
                var degerler = db.studentTable.Select(x => new { soyadı = x.studentSurname });
                dataGridView1.DataSource = degerler.ToList();
            }
            if (radioButton3.Checked == true)
            {
                var degerler = db.studentTable.Select(x => new { Ad = x.studentName.ToUpper(), Soyadı = x.studentSurname.ToLower() });
                dataGridView1.DataSource = degerler.ToList();
            }
            if (radioButton4.Checked == true)
            {
                var degerler = db.gradeTable.Select(x => new
                {
                    OgrenciAd = x.stdnt,
                    ortalaması = x.average,
                    durumu = x.situation == true ? "Geçti" : "Kaldı"
                });
                dataGridView1.DataSource = degerler.ToList();
            }
            if (radioButton5.Checked == true) 
            {
                var degerler = db.gradeTable.SelectMany(x => db.studentTable.Where(y => y.ıd == x.stdnt), (x, y) => new
                {
                    y.studentName,
                    x.average,
                    durum = x.situation== true ? "Geçti" : "Kaldı"
                });
                dataGridView1.DataSource= degerler.ToList();
            }
        }
    }
}
