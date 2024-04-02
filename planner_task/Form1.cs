using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
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
       
        bool isEdditing = false;
        DataTable todoList = new DataTable();
        private void Form1_Load(object sender, EventArgs e)
        {
            InitDataTable();
        }

        private void InitDataTable()
        {
            todoList.Columns.Add("Статус");
            todoList.Columns.Add("Название");
            todoList.Columns.Add("Описание");
            todoList.Columns.Add("Начало дедлайна");
            todoList.Columns.Add("Конец дедлайна");
            //string basePath = AppDomain.CurrentDomain.BaseDirectory;

            string F = "data.txt";
            //MessageBox.Show(F);
            if (File.Exists(F))
            {
                using (StreamReader sr = new StreamReader(F))
                {
                    string line;
                    while ((line = sr.ReadLine()) != null)
                    {
                        string[] words = line.Split('_');
                        if (Convert.ToDateTime(words[3]) > Convert.ToDateTime(words[4]))
                        {
                            if (Convert.ToDateTime(words[3]).AddDays(1) < DateTime.Now)
                            {
                                words[0] = "Просрочено";
                            }
                            todoList.Rows.Add(words[0], words[1], words[2], words[4], words[3]);
                        }
                        else
                        {
                            if (Convert.ToDateTime(words[4]).AddDays(1) < DateTime.Now)
                            {
                                words[0] = "Просрочено";
                            }
                            todoList.Rows.Add(words[0], words[1], words[2], words[3], words[4]);
                        }
                        
                        
                        
                        Console.WriteLine(line);
                    }
                }
            }
            else
            {
                MessageBox.Show("not file");
            }




            toDoListView.DataSource = todoList;

        }
        private void editButton_Click(object sender, EventArgs e)
        {
            isEdditing = true;
            titleTextBox.Text = todoList.Rows[toDoListView.CurrentCell.RowIndex].ItemArray[1].ToString();
            descriptionTextbox.Text = todoList.Rows[toDoListView.CurrentCell.RowIndex].ItemArray[2].ToString();
            if (dateTimePickerStart.Value > dateTimePickerEnd.Value)
            {
                MessageBox.Show("Даты были перепутаны местами, но мы поправили)");
                dateTimePickerStart.Value = Convert.ToDateTime(todoList.Rows[toDoListView.CurrentCell.RowIndex].ItemArray[4].ToString());
                dateTimePickerEnd.Value = Convert.ToDateTime(todoList.Rows[toDoListView.CurrentCell.RowIndex].ItemArray[3].ToString());
            }
            else
            {
                dateTimePickerStart.Value = Convert.ToDateTime(todoList.Rows[toDoListView.CurrentCell.RowIndex].ItemArray[3].ToString());
                dateTimePickerEnd.Value = Convert.ToDateTime(todoList.Rows[toDoListView.CurrentCell.RowIndex].ItemArray[4].ToString());
            }
            
            string file = "data.txt";
            using (StreamWriter sw = new StreamWriter(file))
            {
                foreach (DataRow row in todoList.Rows)
                {
                    sw.WriteLine(row["Статус"].ToString()+"_"+ row["Название"].ToString() + "_"+row["Описание"].ToString() + "_" + row["Начало дедлайна"].ToString() + "_" + row["Конец дедлайна"].ToString());
                }
            }
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
            //string basePath = AppDomain.CurrentDomain.BaseDirectory;
            string file = "data.txt";
            using (StreamWriter sw = new StreamWriter(file))
            {
                foreach (DataRow row in todoList.Rows)
                {
                    sw.WriteLine(row["Статус"].ToString() + "_" + row["Название"].ToString() + "_" + row["Описание"].ToString() + "_" + row["Начало дедлайна"].ToString() + "_" + row["Конец дедлайна"].ToString());
                }
            }
        }

        private void saveButton_Click(object sender, EventArgs e)
        {
            if (isEdditing)
            {
                
                if (dateTimePickerStart.Value > dateTimePickerEnd.Value)
                {
                    MessageBox.Show("Даты были перепутаны местами, но мы поправили)");
                    todoList.Rows[toDoListView.CurrentCell.RowIndex]["Начало дедлайна"] = dateTimePickerEnd.Text;
                    todoList.Rows[toDoListView.CurrentCell.RowIndex]["Конец дедлайна"] = dateTimePickerStart.Text;
                }
                else
                {
                    todoList.Rows[toDoListView.CurrentCell.RowIndex]["Начало дедлайна"] = dateTimePickerStart.Text;
                    todoList.Rows[toDoListView.CurrentCell.RowIndex]["Конец дедлайна"] = dateTimePickerEnd.Text;
                }
                if (Convert.ToDateTime(todoList.Rows[toDoListView.CurrentCell.RowIndex].ItemArray[4].ToString()).AddDays(1) < DateTime.Now)
                {
                    todoList.Rows[toDoListView.CurrentCell.RowIndex]["Статус"] = "Просрочено";
                }
                else todoList.Rows[toDoListView.CurrentCell.RowIndex]["Статус"] = "☒";
                todoList.Rows[toDoListView.CurrentCell.RowIndex]["Название"] = titleTextBox.Text;
                todoList.Rows[toDoListView.CurrentCell.RowIndex]["Описание"] = descriptionTextbox.Text;


            }
            else
            {
                if (dateTimePickerEnd.Value.AddDays(1) < DateTime.Now)
                {
                    if (dateTimePickerStart.Value > dateTimePickerEnd.Value)
                    {
                        MessageBox.Show("Даты были перепутаны местами, но мы поправили)");
                        todoList.Rows.Add("Просрочено", titleTextBox.Text, descriptionTextbox.Text, dateTimePickerEnd.Text, dateTimePickerStart.Text);
                    }
                    else todoList.Rows.Add("Просрочено", titleTextBox.Text, descriptionTextbox.Text, dateTimePickerStart.Text, dateTimePickerEnd.Text);
                }
                else
                {
                    if (dateTimePickerStart.Value > dateTimePickerEnd.Value)
                    {
                        MessageBox.Show("Даты были перепутаны местами, но мы поправили)");
                        todoList.Rows.Add("☒", titleTextBox.Text, descriptionTextbox.Text, dateTimePickerEnd.Text, dateTimePickerStart.Text);
                    }
                    else todoList.Rows.Add("☒", titleTextBox.Text, descriptionTextbox.Text, dateTimePickerStart.Text, dateTimePickerEnd.Text);
                }
            }
            //string basePath = AppDomain.CurrentDomain.BaseDirectory;
            string file =  "data.txt";
            using (StreamWriter sw = new StreamWriter(file))
            {
                foreach (DataRow row in todoList.Rows)
                {
                    sw.WriteLine(row["Статус"].ToString() + "_" + row["Название"].ToString() + "_" + row["Описание"].ToString() + "_" + row["Начало дедлайна"].ToString() + "_" + row["Конец дедлайна"].ToString());
                }
            }
            titleTextBox.Text = "";
            descriptionTextbox.Text = "";
            dateTimePickerStart.Value = DateTime.Now;
            dateTimePickerEnd.Value = DateTime.Now;
            isEdditing = false;
        }

        private void completeButton_Click(object sender, EventArgs e)
        {
            if (Convert.ToDateTime(todoList.Rows[toDoListView.CurrentCell.RowIndex].ItemArray[4].ToString()).AddDays(1) < DateTime.Now)
            {
                todoList.Rows[toDoListView.CurrentCell.RowIndex]["Статус"] = "Просрочено";
            }
            else todoList.Rows[toDoListView.CurrentCell.RowIndex]["Статус"] = "☑";
            //string basePath = AppDomain.CurrentDomain.BaseDirectory;
            string file =  "data.txt";
            using (StreamWriter sw = new StreamWriter(file))
            {
                foreach (DataRow row in todoList.Rows)
                {
                    sw.WriteLine(row["Статус"].ToString() + "_" + row["Название"].ToString() + "_" + row["Описание"].ToString() + "_" + row["Начало дедлайна"].ToString() + "_" + row["Конец дедлайна"].ToString());
                }
            }
        }

        
    }

}
