﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Demo_WinForms_FlintstonesViewer
{
    public partial class ListForm : Form
    {
        private List<Character> _characters;

        public ListForm()
        {
            InitializeComponent();
        }

        private void ReadXmlFileAndBindToDataGrid()
        {
            string dataFilePath = AppConfig.dataFilePath;

            //
            // read data file
            //
            IDataService dataService = new XmlDataService(dataFilePath);
            _characters = dataService.ReadAll();

            //
            // bind list to DataGridView control
            //
            var bindingList = new BindingList<Character>(_characters);
            var source = new BindingSource(bindingList, null);
            dataGridView_Characters.DataSource = source;

            //
            // configure DataGridView control
            //
            this.dataGridView_Characters.Columns["Id"].Visible = false;
            this.dataGridView_Characters.Columns["ImageFileName"].Visible = false;
            this.dataGridView_Characters.Columns["Description"].Visible = false;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                ReadXmlFileAndBindToDataGrid();
            }
            catch (FileNotFoundException)
            {

                MessageBox.Show("Unable to locate data file.\nExiting the application.");
                this.Close();
            }

        }

        private void btn_CheckList_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in dataGridView_Characters.SelectedRows)
            {
                dataGridView_Characters.Rows.RemoveAt(row.Index);
            }
        }

        private void btn_DetailView_Click(object sender, EventArgs e)
        {
            if (dataGridView_Characters.SelectedRows.Count == 1)
            {
                Character character = new Character();
                character = (Character)dataGridView_Characters.CurrentRow.DataBoundItem;

                DetailForm detailForm = new DetailForm(character);
                detailForm.ShowDialog();
            }
        }

        private void btn_exit_Click(object sender, EventArgs e)
        {
            try
            {
                IDataService XmlDataService = new XmlDataService(AppConfig.dataFilePath);
                XmlDataService.WriteAll(_characters);
            }
            catch (Exception)
            {

                throw;
            }
            this.Close();

        }

        private void btn_sort_Click(object sender, EventArgs e)
        {
            // this what i had at the time but something came to mind
            //
            //var bindingList = new BindingList<Character>(_characters);
            //var source = new BindingSource(bindingList, null);
            //source.Sort = "Age DESC";
            //dataGridView_Characters.Refresh();
            //dataGridView_Characters.Update();

            var sortedList = _characters.OrderBy(c => c.Age).ToList();
            dataGridView_Characters.DataSource = sortedList;
        }

        private void btn_search_Click(object sender, EventArgs e)
        {
            //var bindingList = new BindingList<Character>(_characters);
            //var source = new BindingSource(bindingList, null);
            //source.Filter = dataGridView_Characters.Columns[1].HeaderText.ToString() + " LIKE '%" + txtBox_searchField.Text + "%'";
            //dataGridView_Characters.DataSource = source;
            //dataGridView_Characters.Refresh();

            string searchTerm = txtBox_searchField.Text;
            var searchedList = _characters.Where(c => c.LastName.ToUpper().Contains(searchTerm.ToUpper())).ToList();

            if (searchedList == null)
            {
                MessageBox.Show("Nothing to search!");
            }
            else
            {
                dataGridView_Characters.DataSource = searchedList;
            }
        }

        private void btn_filter_Click(object sender, EventArgs e)
        {
            var filteredList = _characters.Where(c => c.Gender == Character.GenderType.Female).ToList();

            if (filteredList == null)
            {
                MessageBox.Show("Nothing to filter!");
            }
            else
            {
                dataGridView_Characters.DataSource = filteredList;
            }
        }

        private void btn_help_Click(object sender, EventArgs e)
        {
            HelpForm helpForm = new HelpForm();
            helpForm.ShowDialog();
        }
    }
}
