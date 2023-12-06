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

namespace lb_12_bd
{
    public partial class Form1 : Form
    {
        private SqlConnection conn;
        private string currentTableName;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            conn = new SqlConnection(Properties.Settings.Default.Ip5_21_SofronovConnectionString);
            currentTableName = "Абитуриенты";
            LoadData(currentTableName);
        }

        private void абитуриентыToolStripMenuItem_Click(object sender, EventArgs e)
        {
            currentTableName = "Абитуриенты";
            LoadData(currentTableName);
            label2.Visible = false;
            comboBox2.Visible = false;
            label1.Visible = true;
            comboBox1.Visible = true;
        }
        private void LoadData(string tableName)
        {
            SqlConnection conn = new SqlConnection();
            conn.ConnectionString = (Properties.Settings.Default.Ip5_21_SofronovConnectionString);
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conn;
            cmd.CommandText = $"Select * from {tableName}";
            conn.Open();
            SqlDataReader rdr = cmd.ExecuteReader();
            DataTable dt = new DataTable();
            for (int i = 0; i < rdr.FieldCount; i++)
            {
                dt.Columns.Add(new DataColumn(rdr.GetName(i), rdr.GetFieldType(i)));
            }
            while (rdr.Read())
            {
                DataRow row = dt.NewRow();
                for (int i = 0; i < rdr.FieldCount; i++)
                {
                    row[i] = rdr.GetValue(i);
                }
                dt.Rows.Add(row);
            }
            conn.Close();
            dataGridView1.DataSource = dt;
            lbRecordCount.Text = "Количество записей: " + String.Format("{0}", dt.Rows.Count);
        }

        private void заявленияToolStripMenuItem_Click(object sender, EventArgs e)
        {
            currentTableName = "Заявления";
            LoadData(currentTableName);
        }

        private void специальностиToolStripMenuItem_Click(object sender, EventArgs e)
        {
            currentTableName = "Специальности";
            LoadData(currentTableName);
        }

        private void экзаменыToolStripMenuItem_Click(object sender, EventArgs e)
        {
            currentTableName = "Экзамены";
            LoadData(currentTableName);
        }

        private void оценкиToolStripMenuItem_Click(object sender, EventArgs e)
        {
            currentTableName = "Оценки";
            LoadData(currentTableName);
            label2.Visible = true;
            comboBox2.Visible = true;
            label1.Visible = false;
            comboBox1.Visible = false;
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            Form2 form2 = new Form2(((DataTable)dataGridView1.DataSource).NewRow(), false);
            if (form2.ShowDialog() == DialogResult.OK)
            {
                DataRow newRow = form2.Row;

                using (SqlConnection conn = new SqlConnection(Properties.Settings.Default.Ip5_21_SofronovConnectionString))
                using (SqlCommand cmd = new SqlCommand("Абитуриент_Insert", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@Фамилия", SqlDbType.NVarChar).Value = newRow["Фамилия"];
                    cmd.Parameters.Add("@Имя", SqlDbType.NVarChar).Value = newRow["Имя"];
                    cmd.Parameters.Add("@Отчество", SqlDbType.NVarChar).Value = newRow["Отчество"];
                    cmd.Parameters.Add("@Статус", SqlDbType.NVarChar).Value = newRow["Статус"];
                    cmd.Parameters.Add("@Город", SqlDbType.NVarChar).Value = newRow["Город"];

                    conn.Open();

                    if (cmd.ExecuteNonQuery() > 0)
                    {
                        MessageBox.Show("Студент успешно добавлен");
                    }
                }

                LoadData(currentTableName);
                dataGridView1.Rows[dataGridView1.Rows.Count - 1].Selected = true;

            }
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 0)
            {
                MessageBox.Show("Выберите запись для изменения.");
                return;
            }

            DataGridViewRow selectedRow = dataGridView1.SelectedRows[0];

            DataRow selectedDataRow = ((DataRowView)selectedRow.DataBoundItem).Row;
            Form2 form2 = new Form2(selectedDataRow, true);

            if (form2.ShowDialog() == DialogResult.OK)
            {
                using (SqlConnection conn = new SqlConnection(Properties.Settings.Default.Ip5_21_SofronovConnectionString))
                using (SqlCommand cmd = new SqlCommand("Абитуриент_Update", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@Фамилия", SqlDbType.NVarChar).Value = form2.Row["Фамилия"];
                    cmd.Parameters.Add("@Имя", SqlDbType.NVarChar).Value = form2.Row["Имя"];
                    cmd.Parameters.Add("@Отчество", SqlDbType.NVarChar).Value = form2.Row["Отчество"];
                    cmd.Parameters.Add("@Статус", SqlDbType.NVarChar).Value = form2.Row["Статус"];
                    cmd.Parameters.Add("@Город", SqlDbType.NVarChar).Value = form2.Row["Город"];

                    cmd.Parameters.Add("@Код_абитуриента", SqlDbType.Int).Value = selectedDataRow["Код_абитуриента"];

                    conn.Open();

                    if (cmd.ExecuteNonQuery() > 0)
                    {
                        MessageBox.Show("Запись успешно изменена");
                    }
                    else
                    {
                        MessageBox.Show("Операция отменена или не удалась.");
                    }

                    LoadData(currentTableName);
                }
            }
        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 0)
            {
                MessageBox.Show("Выберите запись для удаления.");
                return;
            }

            DataGridViewRow selectedRow = dataGridView1.SelectedRows[0];

            DataRow selectedDataRow = ((DataRowView)selectedRow.DataBoundItem).Row;

            DialogResult result = MessageBox.Show("Вы уверены, что хотите удалить выбранную запись?", "Подтверждение удаления", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                using (SqlConnection conn = new SqlConnection(Properties.Settings.Default.Ip5_21_SofronovConnectionString))
                using (SqlCommand cmd = new SqlCommand("Абитуриент_Delete1", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@Абитуриент", SqlDbType.NVarChar).Value = selectedDataRow["Код_абитуриента"];

                    conn.Open();

                    if (cmd.ExecuteNonQuery() > 0)
                    {
                        MessageBox.Show("Запись успешно удалена");
                    }
                    else
                    {
                        MessageBox.Show("Операция отменена или не удалась.");
                    }

                    LoadData(currentTableName);
                }
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            SqlConnection conn = new SqlConnection(Properties.Settings.Default.Ip5_21_SofronovConnectionString);
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conn;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "Student_Select_Faculty";

            cmd.Parameters.Add(new SqlParameter("@Код_специальности", SqlDbType.Int));
            cmd.Parameters["@Код_специальности"].Value = comboBox1.Text;
            
            conn.Open();

            SqlDataReader rdr = cmd.ExecuteReader();

            DataTable dt = new DataTable();

            for (int i = 0; i < rdr.FieldCount; i++)
            {
                dt.Columns.Add(new DataColumn(rdr.GetName(i), rdr.GetFieldType(i)));
            }

            while (rdr.Read())
            {
                DataRow row = dt.NewRow();

                for (int i = 0; i < rdr.FieldCount; i++)
                {
                    row[i] = rdr.GetValue(i);
                }

                dt.Rows.Add(row);
            }
            conn.Close();
            dataGridView1.DataSource = dt;
            lbRecordCount.Text = $"Количество записей: {dt.Rows.Count} записей";
        
        }
        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            SqlConnection conn = new SqlConnection(Properties.Settings.Default.Ip5_21_SofronovConnectionString);
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conn;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "Grade_Select_Faculty";

            cmd.Parameters.Add(new SqlParameter("@Код_специальности", SqlDbType.Int));
            cmd.Parameters["@Код_специальности"].Value = comboBox2.Text;

            conn.Open();

            SqlDataReader rdr = cmd.ExecuteReader();

            DataTable dt = new DataTable();

            for (int i = 0; i < rdr.FieldCount; i++)
            {
                dt.Columns.Add(new DataColumn(rdr.GetName(i), rdr.GetFieldType(i)));
            }

            while (rdr.Read())
            {
                DataRow row = dt.NewRow();

                for (int i = 0; i < rdr.FieldCount; i++)
                {
                    row[i] = rdr.GetValue(i);
                }

                dt.Rows.Add(row);
            }
            conn.Close();
            dataGridView1.DataSource = dt;
            lbRecordCount.Text = $"Количество записей: {dt.Rows.Count} записей";
        }

        private void comboBox1_SelectedValueChanged(object sender, EventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

       

        private void comboBox2_SelectedValueChanged(object sender, EventArgs e)
        {

        }
    }
}
