using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace TaskManager
{
    public partial class Form2 : Form
    {
        public int TaskID { get; set; }
        public Task EditedTask { get; private set; }

        public Form2()
        {
            InitializeComponent();
        }

        public Form2(Task task)
        {
            TaskID = task.ID;
            InitializeComponent();
            textBox1.Text = task.Name;
            textBox2.Text = task.Description;
            textBox3.Text = task.DueDate;
        }


        private void Form2_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            
            EditedTask = new Task();
            EditedTask.ID = TaskID;
            EditedTask.Name = textBox1.Text;
            EditedTask.Description = textBox2.Text;
            EditedTask.DueDate = textBox3.Text;
            EditedTask.Complete = (int)numericUpDown1.Value;

            DialogResult = DialogResult.OK;
            Close();
        }
    }
}
