using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SQLite;

namespace TaskManager
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            RefreshTaskList();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
        }

        private void RefreshTaskList()
        {
            using (SQLiteConnection con = new SQLiteConnection(@"Data Source=..\..\tasks.db; Version=3;"))
            {
                con.Open();
                string query = "SELECT * from Tasks";
                using (SQLiteCommand cmd = new SQLiteCommand(query, con))
                {
                    DataTable dt = new DataTable();
                    using (SQLiteDataAdapter adapter = new SQLiteDataAdapter(cmd))
                    {
                        adapter.Fill(dt);
                        dataGridView1.DataSource = dt;
                    }
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            using (Form2 addForm = new Form2())
            {
                if (addForm.ShowDialog() == DialogResult.OK)
                {
                    Task newTask = addForm.EditedTask;
                    AddNewRecordToDatabase(newTask);
                    RefreshTaskList();
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                int selectedIndex = dataGridView1.SelectedRows[0].Index;
                int taskID = Convert.ToInt32(dataGridView1.Rows[selectedIndex].Cells["ID"].Value);
                DeleteRecordFromDatabase(taskID);
                RefreshTaskList();
            }
            else
            {
                MessageBox.Show("Please select a task to delete.");
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                int selectedIndex = dataGridView1.SelectedRows[0].Index;
                Task selectedTask = GetTaskFromSelectedRow(selectedIndex);

                using (Form2 editForm = new Form2(selectedTask))
                {
                    editForm.TaskID = selectedTask.ID;

                    if (editForm.ShowDialog() == DialogResult.OK)
                    {
                        UpdateRecordInDatabase(editForm.EditedTask);
                        RefreshTaskList();
                    }
                }
            }
            else
            {
                MessageBox.Show("Please select a task to edit.");
            }
        }

        private void AddNewRecordToDatabase(Task task)
        {
            using (SQLiteConnection con = new SQLiteConnection(@"Data Source=..\..\tasks.db; Version=3;"))
            {
                con.Open();
                string query = "INSERT INTO Tasks (Name, Description, DueDate, Complete) VALUES (@Name, @Description, @DueDate, @Complete)";
                using (SQLiteCommand cmd = new SQLiteCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@Name", task.Name);
                    cmd.Parameters.AddWithValue("@Description", task.Description);
                    cmd.Parameters.AddWithValue("@DueDate", task.DueDate);
                    cmd.Parameters.AddWithValue("@Complete", task.Complete);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        private void UpdateRecordInDatabase(Task updatedtask)
        {
            using (SQLiteConnection con = new SQLiteConnection(@"Data Source=..\..\tasks.db; Version=3;"))
            {
                con.Open();
                string query = "UPDATE Tasks SET Name = @Name, Description = @Description, DueDate = @DueDate, Complete = @Complete WHERE ID = @ID";
                using (SQLiteCommand cmd = new SQLiteCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@Name", updatedtask.Name);
                    cmd.Parameters.AddWithValue("@Description", updatedtask.Description);
                    cmd.Parameters.AddWithValue("@DueDate", updatedtask.DueDate);
                    cmd.Parameters.AddWithValue("@Complete", updatedtask.Complete);
                    cmd.Parameters.AddWithValue("@ID", updatedtask.ID);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        private void DeleteRecordFromDatabase(int taskID)
        {
            using (SQLiteConnection con = new SQLiteConnection(@"Data Source=..\..\tasks.db; Version=3;"))
            {
                con.Open();
                string query = "DELETE FROM Tasks WHERE ID = @ID";
                using (SQLiteCommand cmd = new SQLiteCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@ID", taskID);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        private Task GetTaskFromSelectedRow(int rowIndex)
        {
            Task task = new Task();
            task.ID = Convert.ToInt32(dataGridView1.Rows[rowIndex].Cells["ID"].Value);
            task.Name = dataGridView1.Rows[rowIndex].Cells["Name"].Value.ToString();
            task.Description = dataGridView1.Rows[rowIndex].Cells["Description"].Value.ToString();
            task.DueDate = dataGridView1.Rows[rowIndex].Cells["DueDate"].Value.ToString();
            task.Complete = Convert.ToInt32(dataGridView1.Rows[rowIndex].Cells["Complete"].Value);
            return task;
        }
    }
}