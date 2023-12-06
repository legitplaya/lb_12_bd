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
    public partial class Form2 : Form
    {
        private DataRow row;
        private bool isUpdate;
        public Form2(DataRow inputRow, bool update)
        {
            InitializeComponent();
            row = inputRow;
            isUpdate = update;
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            if (isUpdate)
            {
                this.Text = "Изменение данных об абитуриенте";
                textBox1.Text = row["Фамилия"].ToString();
                textBox2.Text = row["Имя"].ToString();
                textBox3.Text = row["Отчество"].ToString();
                comboBox1.Text = (string)row["Статус"];
                textBox4.Text = row["Город"].ToString();

            }
        }
        public DataRow Row
        {
            get { return row; }
            set { row = value; }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            row["Фамилия"] = textBox1.Text;
            row["Имя"] = textBox2.Text;
            row["Отчество"] = textBox3.Text;
            row["Статус"] = comboBox1.Text;
            row["Город"] = textBox4.Text;

            DialogResult = DialogResult.OK;
            Close();
        }
    }
}
