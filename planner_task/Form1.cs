using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace planner_task
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        DataTable todoList = new DataTable();
        bool isEdditing = false;

        private void Form1_Load(object sender, EventArgs e)
        {
            todoList.Columns.Add("Статус");
            todoList.Columns.Add("Название");
            todoList.Columns.Add("Описание");
            todoList.Columns.Add("Начало дедлайна");
            todoList.Columns.Add("Конец дедлайна");
            toDoListView.DataSource = todoList;

        }


        private void editButton_Click(object sender, EventArgs e)
        {
            isEdditing = true;
            titleTextBox.Text = todoList.Rows[toDoListView.CurrentCell.RowIndex].ItemArray[1].ToString();
            descriptionTextbox.Text = todoList.Rows[toDoListView.CurrentCell.RowIndex].ItemArray[2].ToString();
            startTextBox.Text = todoList.Rows[toDoListView.CurrentCell.RowIndex].ItemArray[3].ToString();
            endTextBox.Text = todoList.Rows[toDoListView.CurrentCell.RowIndex].ItemArray[4].ToString();

        }

        private void deleteButton_Click(object sender, EventArgs e)
        {
            try
            {
                todoList.Rows[toDoListView.CurrentCell.RowIndex].Delete();
            }
            catch(Exception ex)
            {
                Console.WriteLine("Error:" + ex);
            }
        }

        private void saveButton_Click(object sender, EventArgs e)
        {
            if (isEdditing)
            {
                todoList.Rows[toDoListView.CurrentCell.RowIndex]["Статус"] = "☒";
                todoList.Rows[toDoListView.CurrentCell.RowIndex]["Название"] = titleTextBox.Text;
                todoList.Rows[toDoListView.CurrentCell.RowIndex]["Описание"] = descriptionTextbox.Text;
                todoList.Rows[toDoListView.CurrentCell.RowIndex]["Начало дедлайна"] = startTextBox.Text;
                todoList.Rows[toDoListView.CurrentCell.RowIndex]["Конец дедлайна"] = endTextBox.Text;

            }
            else
            {
                todoList.Rows.Add("☒", titleTextBox.Text, descriptionTextbox.Text, startTextBox.Text, endTextBox.Text);
            }
            titleTextBox.Text = "";
            descriptionTextbox.Text = "";
            startTextBox.Text = "";
            endTextBox.Text = "";
            isEdditing = false;
        }

        private void completeButton_Click(object sender, EventArgs e)
        {
            todoList.Rows[toDoListView.CurrentCell.RowIndex]["Статус"] = "☑";
        }

        
    }

}
