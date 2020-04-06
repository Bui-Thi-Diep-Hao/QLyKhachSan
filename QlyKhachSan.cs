using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace QlyKhachSan
{
    public partial class QlyKhachSan : Form
    {
        SqlConnection Con = new SqlConnection();
        public QlyKhachSan()
        {
            InitializeComponent();
        }

        private void QlyKhachSan_Load(object sender, EventArgs e)
        {
            string  Connectionstring = "Data Source=localhost\\SQLEXPRESS;Initial Catalog=QLyKhachSan;Integrated Security=True";
            Con.ConnectionString = Connectionstring;
            Con.Open();
            string SQL = " select* from tblPhong";
            SqlDataAdapter dataadp = new SqlDataAdapter(SQL, Con);
            DataTable tablePhong = new DataTable();
            dataadp.Fill(tablePhong);
            dataGridViewPhong.DataSource = tablePhong;
        }
        private void loadGridView_Phong()
        {
            string SQL = " select* from tblPhong";
            SqlDataAdapter dataadp = new SqlDataAdapter(SQL, Con);
            DataTable tablePhong = new DataTable();
            dataadp.Fill(tablePhong);
            dataGridViewPhong.DataSource = tablePhong;
        }
        public void disconnect()
        {
            if (Con.State == ConnectionState.Open)
            {
                Con.Close();
                Con.Dispose();
            }
        }

        private void bntThemMoi_Click(object sender, EventArgs e)
        {
            bntHuy.Enabled = true;
            bntLuu.Enabled = true;
            bntSua.Enabled = true;
            bntThoat.Enabled = true;
            bntThemMoi.Enabled = false;
            bntXoa.Enabled = false;
            txtMaPhong.Text = "";
            txtTenPhong.Text = "";
            txtDonGia.Text = "";
            txtMaPhong.Focus();
        }

        private void bntXoa_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("bạn có muốn xóa không?", "Thông Báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes)
            {
                string SQL = "Delete from tblPhong where MaPhong='" + txtMaPhong.Text + "'";
                SqlCommand command = new SqlCommand(SQL, Con);
                command.ExecuteNonQuery();
                RunSQL(SQL);
                loadGridView_Phong();

            }
        }
        public void RunSQL(string SQL)
        {
            SqlCommand command = new SqlCommand();
            command.CommandText = SQL;
            command.Connection = Con;
            try
            {
                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void dataGridViewPhong_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            txtMaPhong.Text = dataGridViewPhong.CurrentRow.Cells["MaPhong"].Value.ToString();
            txtTenPhong.Text = dataGridViewPhong.CurrentRow.Cells["TenPhong"].Value.ToString();
            txtDonGia.Text = dataGridViewPhong.CurrentRow.Cells["DonGia"].Value.ToString();
        }
        private void bntLuu_Click(object sender, EventArgs e)
        {
            if (txtMaPhong.Text == "")
            {
                MessageBox.Show("Bạn Phải Nhập Mã Phòng");
                txtMaPhong.Focus();
                return;
            }
            if (txtTenPhong.Text == "")
            {
                MessageBox.Show("Bạn Phải Nhập Tên Phòng");
                txtTenPhong.Focus();
                return;
            }
            if (txtDonGia.Text == "")
            {
                MessageBox.Show("bạn Phải nhập đơn giá");
                txtDonGia.Focus();
                return;
            }
            else
            {
                string SQL;
                SQL = "select MaPhong from tblPhong WHere MaPhong='" + txtMaPhong.Text + "'";
                SqlDataAdapter adap = new SqlDataAdapter(SQL, Con);
                DataTable tablePhong = new DataTable();
                adap.Fill(tablePhong);
                if(tablePhong.Rows.Count>0)
                {
                    MessageBox.Show("Mã Phòng đã tồn tại");
                    txtMaPhong.Focus();
                    return;
                }    
                SQL = "insert into tblPhong Values('" + txtMaPhong.Text + "','" + txtTenPhong.Text + "'";
                if (txtDonGia.Text != "")
                    SQL = SQL + "," + txtDonGia.Text.Trim();
                SQL = SQL + ")";
                SqlCommand command = new SqlCommand(SQL, Con);
                command.ExecuteNonQuery();
                RunSQL(SQL);
                loadGridView_Phong();
            }
            bntHuy.Enabled = false;
        }
        private void txtDonGia_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (((e.KeyChar >= '0') && (e.KeyChar <= '9') || (e.KeyChar == '.') || (Convert.ToInt32(e.KeyChar) == 8) || (Convert.ToInt32(e.KeyChar) == 13)))
            {
                e.Handled = false;
            }
            else
                e.Handled = true;
        }

        private void bntHuy_Click(object sender, EventArgs e)
        {
            bntThemMoi.Enabled = false;
            bntSua.Enabled = true;
            bntLuu.Enabled = false;
            bntHuy.Enabled = false;
            bntXoa.Enabled = true;
            bntThoat.Enabled = true;
            txtMaPhong.Enabled = false;
        }

        private void bntSua_Click(object sender, EventArgs e)
        {


            if (txtTenPhong.Text.Length == 0)
            {
             MessageBox.Show("Bạn cần phải nhập tên bảng", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Question);
            txtTenPhong.Focus();
             return;
            }
            if (txtDonGia.Text.Length == 0)
            {
              MessageBox.Show("bạn cần phải nhập đơn giá", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Question);
            }

            string SQL;
            SQL = " Update tblPhong Set TenPhong=N'" + txtTenPhong.Text + "',DonGia=N'" + txtDonGia.Text +"'"+
            "Where MaPhong=N'" + txtMaPhong.Text + "'";
            SqlCommand command = new SqlCommand(SQL, Con);
            command.ExecuteNonQuery();
            RunSQL(SQL);
            loadGridView_Phong();
            txtMaPhong.Enabled = false;
            bntHuy.Enabled = false;
           
        }

        private void bntThoat_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Bạn có muốn thoát không?", " Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes)
                Application.Exit();
            else
                Con.Close();

        }
    }
}
