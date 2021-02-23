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


namespace WindowsFormsApp10
{
    public partial class Form1 : Form
    {
        SqlConnection baglanti = new SqlConnection("Data Source=DESKTOP-81HTBB2\\SQLEXPRESS;Initial Catalog=stajyer;Integrated Security=True");

        void TabloyuGuncelle()
        {
            if (baglanti.State == ConnectionState.Closed)
            { baglanti.Open(); }
            SqlDataAdapter tablo = new SqlDataAdapter("select * from tbl_Staj", baglanti);
            DataSet dataset_tablo = new DataSet();
            tablo.Fill(dataset_tablo, "tbl_Staj");
            dataGridView1.DataSource = dataset_tablo.Tables["tbl_Staj"];
           
            BitisTarihiFonksiyonu();
           
        }

        void TemizlemeFonksiyonu()
        {
            txt_Ad.Clear();
            txt_Soyad.Clear();
            txt_GunSayisi.Clear();
            dateTimePicker1.Value = DateTime.Now;
         //   dataGridView1.ClearSelection();
        }
        string BitisTarihiFonksiyonu()
        {
            DateTime BaslangicTarihi = dateTimePicker1.Value;
            DateTime BitisTarihi;
            if (txt_GunSayisi.Text != "")
            {
                int GunSayisi = Convert.ToInt32(txt_GunSayisi.Text);
                BitisTarihi = BaslangicTarihi.AddDays(GunSayisi);
                string YeniTarih = BitisTarihi.ToShortTimeString();
                return YeniTarih;
            }
            return "";
        }
        private void KayitEklemeFonksiyonu()
        {
            if (baglanti.State == ConnectionState.Closed)
            { baglanti.Open(); }

            if (txt_Ad.Text == "" || txt_Soyad.Text == "" || txt_GunSayisi.Text == "")
            {
                MessageBox.Show("Bu Alanlar Boş Geçilemez.Lütfen Gerekli Alanları Doldurunuz.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                SqlCommand KayitEkleme = new SqlCommand("INSERT INTO tbl_Staj(Ad,Soyad,BaslangicTarihi,GunSayisi,BitisTarihi) "
                                                                   + "VALUES( @ad, @soyad, @baslangic, @gun, @bitis)", baglanti);
                KayitEkleme.Parameters.AddWithValue("@ad", txt_Ad.Text);
                KayitEkleme.Parameters.AddWithValue("@soyad", txt_Soyad.Text);
                KayitEkleme.Parameters.AddWithValue("@baslangic", dateTimePicker1.Value);
                KayitEkleme.Parameters.AddWithValue("@gun", txt_GunSayisi.Text);
                DateTime BaslangicTarihi = dateTimePicker1.Value;
                DateTime BitisTarihi = BaslangicTarihi.AddDays(Convert.ToDouble(txt_GunSayisi.Text.ToString()));
                KayitEkleme.Parameters.AddWithValue("@bitis", BitisTarihi);
                KayitEkleme.ExecuteNonQuery();
                MessageBox.Show("Kayıt Başarılı", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
             //   TemizlemeFonksiyonu();
                TabloyuGuncelle();
            }
        }
        private void KayitGuncellemeFonksiyonu()
        {
            if (baglanti.State == ConnectionState.Closed)
            { baglanti.Open(); }

            if (txt_Ad.Text == "" || txt_Soyad.Text == "" || txt_GunSayisi.Text == "")
            {
                MessageBox.Show("Bu Alanlar Boş Geçilemez.Lütfen Gerekli Alanları Doldurunuz.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if (MessageBox.Show("Bilgileri Güncellemek İstediğinize Emin Misiniz?", "Uyarı", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                SqlCommand KayitGuncelleme = new SqlCommand("UPDATE tbl_Staj SET Ad=@ad,Soyad=@soyad,BaslangicTarihi=@baslangic,GunSayisi=@gun" +
                                                           " WHERE Ad=@ad", baglanti);            
                KayitGuncelleme.Parameters.AddWithValue("@ad", txt_Ad.Text);
                KayitGuncelleme.Parameters.AddWithValue("@soyad", txt_Soyad.Text);
                KayitGuncelleme.Parameters.AddWithValue("@baslangic", dateTimePicker1.Value);
                KayitGuncelleme.Parameters.AddWithValue("@gun", txt_GunSayisi.Text);
                DateTime BaslangicTarihi = dateTimePicker1.Value;
                DateTime BitisTarihi = BaslangicTarihi.AddDays(Convert.ToDouble(txt_GunSayisi.Text.ToString()));
                KayitGuncelleme.Parameters.AddWithValue("@bitis", BitisTarihi);
                KayitGuncelleme.ExecuteNonQuery();
                MessageBox.Show("Bilgiler Başarıyla Güncellenmiştir.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
           //     TemizlemeFonksiyonu();
                TabloyuGuncelle();
               
                bttn_Kydt.Enabled = true;
            }
        }
        private void KayitSilmeFonksiyonu()
        {
            
            if (baglanti.State == ConnectionState.Closed)
            { baglanti.Open(); }

            SqlCommand KayitSil = new SqlCommand("DELETE FROM tbl_Staj " +
                                                    "WHERE Ad=@ad", baglanti);
           KayitSil.Parameters.AddWithValue("@ad", txt_Ad.Text);

            if (MessageBox.Show("Silmek  İstediğinize Emin Misiniz", "Uyarı", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                KayitSil.ExecuteNonQuery();


                MessageBox.Show("Kayıt Başarıyla Silindi", "Mesaj", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }
            TabloyuGuncelle();

               TemizlemeFonksiyonu();
            bttn_Kydt.Enabled = true;
        }
        private void KayitlariGostermeFonksiyonu()
        {
            int secilen = dataGridView1.SelectedCells[0].RowIndex;
            string numara = dataGridView1.Rows[secilen].Cells[0].Value.ToString();
            string ad = dataGridView1.Rows[secilen].Cells[1].Value.ToString();
            string soyad = dataGridView1.Rows[secilen].Cells[2].Value.ToString();
            string baslangictarihi = dataGridView1.Rows[secilen].Cells[3].Value.ToString();
            string gun = dataGridView1.Rows[secilen].Cells[4].Value.ToString();
            txt_Ad.Text = ad;
            txt_Soyad.Text = soyad;
            dateTimePicker1.Text = baslangictarihi;
            txt_GunSayisi.Text = gun;
            string BitisTarihiSutunu = BitisTarihiFonksiyonu();
            if (BitisTarihiSutunu != "")
            {
                int secim = dataGridView1.SelectedCells[0].RowIndex;
                dataGridView1.Rows[secim].Cells[5].Value = BitisTarihiSutunu;
                bttn_Kydt.Enabled = false;
            }
            else
            {
                bttn_Kydt.Enabled = true;
            }
            
           
        }
        private void YeniKayit()
        {
            if (baglanti.State == ConnectionState.Closed)
            { baglanti.Open(); }

            bttn_Kydt.Enabled = true;
            SqlCommand YeniKayit = new SqlCommand("UPDATE tbl_Staj SET Ad=@ad,Soyad=@soyad,BaslangicTarihi=@baslangic,GunSayisi=@gun  " +
                                                  "WHERE Ad=@ad", baglanti);
            YeniKayit.Parameters.AddWithValue("@ad", txt_Ad.Text);
            YeniKayit.Parameters.AddWithValue("@soyad", txt_Soyad.Text);
            YeniKayit.Parameters.AddWithValue("@baslangic", dateTimePicker1.Value);
            YeniKayit.Parameters.AddWithValue("@gun", txt_GunSayisi.Text);
            YeniKayit.ExecuteNonQuery();
            TabloyuGuncelle();
           // TemizlemeFonksiyonu();
            
            BitisTarihiFonksiyonu();

        }
        public Form1()
        {
            InitializeComponent();

            bindingNavigator1.BindingSource = tblStajBindingSource;
            SqlConnection baglanti = new SqlConnection("Data Source=DESKTOP-81HTBB2\\SQLEXPRESS;Initial Catalog=stajyer;Integrated Security=True");
            SqlDataAdapter tablo_Staj = new SqlDataAdapter();
            SqlCommand cmd_Staj = new SqlCommand();
            DataSet Dataset_Staj = new DataSet();
            if (baglanti.State == ConnectionState.Closed)
                NewMethod(baglanti);
            TabloyuGuncelle();
            cmd_Staj.CommandText = "select * from tbl_Staj";
            tablo_Staj.SelectCommand = cmd_Staj;
            cmd_Staj.Connection = baglanti;
            tablo_Staj.Fill(Dataset_Staj, "stajyer");
            tblStajBindingSource.DataSource = Dataset_Staj.Tables[0];       
            txt_Ad.DataBindings.Add(new Binding("text", tblStajBindingSource, "Ad", true));
            txt_Soyad.DataBindings.Add(new Binding("text", tblStajBindingSource, "Soyad", true));
            txt_GunSayisi.DataBindings.Add(new Binding("text", tblStajBindingSource, "GunSayisi", true));
          
            
           

          
        }

        private static void NewMethod(SqlConnection baglanti)
        {
            baglanti.Open();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // TODO: Bu kod satırı 'stajyerDataSet8.tbl_Staj' tablosuna veri yükler. Bunu gerektiği şekilde taşıyabilir, veya kaldırabilirsiniz.
            this.tbl_StajTableAdapter.Fill(this.stajyerDataSet8.tbl_Staj);
            KayitlariGostermeFonksiyonu();
            
          /*  txt_Ad.Clear();
            txt_Soyad.Clear();
            txt_GunSayisi.Clear();
            dateTimePicker1.Value = DateTime.Now;*/
          // dataGridView1.ClearSelection();
            TabloyuGuncelle();
        }
        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            KayitlariGostermeFonksiyonu();
          
        //   TabloyuGuncelle();
          
        }
        private void btn_Guncelle_Click_1(object sender, EventArgs e)
        {
            
            KayitGuncellemeFonksiyonu();
            TabloyuGuncelle();
        }
        private void bttn_Kydt_Click_1(object sender, EventArgs e)
        {           
           KayitEklemeFonksiyonu();
         //   TabloyuGuncelle();         
        }
      private void btn_Sil_Click_1(object sender, EventArgs e)
        {
            KayitSilmeFonksiyonu();
        //    TabloyuGuncelle();
            
        }
        private void bttn_Yenileme_Click_1(object sender, EventArgs e)
        {
            YeniKayit();
           
           // TabloyuGuncelle();   
    
        }
        private void btn_Iptal_Click_1(object sender, EventArgs e)
        {
            if (MessageBox.Show("İptal etmek istediğinize emin misiniz?", "Uyarı", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Error) == DialogResult.Yes)
            {
                baglanti.Close();
                Application.Exit();
            }
        }
        private void txt_GunSayisi_KeyPress_1(object sender, KeyPressEventArgs e)
        {
            if ((e.KeyChar >= (char)65) && (e.KeyChar <= (char)125))
            {
                e.Handled = true;
                MessageBox.Show("Hatalı bir giriş yaptınız.Lütfen sadece(0-9) arası bir rakam giriniz.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void txt_Ad_KeyPress_1(object sender, KeyPressEventArgs e)
        {
            if (char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
                MessageBox.Show("Hatalı Giris Yaptınız.Lütfen (0-9)dan farklı giriş yapınız.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void txt_Soyad_KeyPress_1(object sender, KeyPressEventArgs e)
        {
            if (char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
                MessageBox.Show("Hatalı Giris Yaptınız.Lütfen (0-9)dan farklı giriş yapınız.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
          
            KayitlariGostermeFonksiyonu();

           // TabloyuGuncelle();
            
            
        }
        private void bindingNavigatorDeleteItem_Click(object sender, EventArgs e)
        {
            KayitSilmeFonksiyonu();
        }
        private void txt_Ad_Click(object sender, EventArgs e)
        {
            YeniKayit();
           // TabloyuGuncelle();
            bttn_Kydt.Enabled = true;
        }
        private void txt_Soyad_TextChanged(object sender, EventArgs e)
        {
            txt_Soyad.CharacterCasing = CharacterCasing.Upper;
        }
        private void txt_Ad_TextChanged(object sender, EventArgs e)
        {
           

            if (txt_Ad.Text != "")
            {
                int kelime = txt_Ad.Text.Length;
                string ilkharf = txt_Ad.Text.Substring(0, 1);
                txt_Ad.Text = ilkharf.ToUpper() + txt_Ad.Text.Substring(1, kelime - 1);
                txt_Ad.SelectionStart = txt_Ad.Text.Length;
            }
           
            
        }

       
        private void bindingNavigatorMoveLastItem_Click(object sender, EventArgs e)
        {
            tblStajBindingSource.MoveLast();
        }

     

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            TemizlemeFonksiyonu();
        }

        private void bindingNavigatorMoveNextItem_Click(object sender, EventArgs e)
        {
            tblStajBindingSource.MoveNext();
            
        }

      
        private void bindingNavigatorMoveNextItem_Click_1(object sender, EventArgs e)
        {
            dataGridView1.ClearSelection();
            dataGridView1.Rows[tblStajBindingSource.Position].Selected = true;
        }

        private void bindingNavigatorMoveLastItem_Click_1(object sender, EventArgs e)
        {
            dataGridView1.ClearSelection();
            dataGridView1.Rows[tblStajBindingSource.Position].Selected = true;
        }

        private void bindingNavigatorMovePreviousItem_Click(object sender, EventArgs e)
        {

            dataGridView1.ClearSelection();
            dataGridView1.Rows[tblStajBindingSource.Position].Selected = true;
        }

        private void bindingNavigatorMoveFirstItem_Click(object sender, EventArgs e)
        {
            dataGridView1.ClearSelection();
            dataGridView1.Rows[tblStajBindingSource.Position].Selected = true;
        }
    }
}
