using System;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SteamAppIdIdentifier
{
    public partial class SteamAppId : Form
    {
        protected DataTableGeneration dataTableGeneration;
        public static int CurrentCell = 0;
        public static string APPDATA = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        public SteamAppId()
        {
            dataTableGeneration = new DataTableGeneration();
            Task.Run(async () => await dataTableGeneration.GetDataTableAsync(dataTableGeneration)).Wait();
            InitializeComponent();
        }

        public static bool VRLExists = false;
        public static string RemoveSpecialCharacters(string str)
        {
            return Regex.Replace(str, "[^a-zA-Z0-9._0-]+", " ", RegexOptions.Compiled);
        }
        private void SteamAppId_Load(object sender, EventArgs e)
        {
            if (Directory.Exists($"{APPDATA}\\VRL"))
            {
                VRLExists = true;
            }

            this.TopMost = true;
            string args3;
            dataGridView1.DataSource = dataTableGeneration.DataTableToGenerate;
            dataGridView1.MultiSelect = false;
            string args2 = "";
            dataGridView1.Columns[0].Width = 540;


            if (args2 != null)
            {
                foreach (string args1 in Program.args2)
                {
                    args3 = RemoveSpecialCharacters(args1);
                    args2 += args3 + " ";
                }
                try
                {
                    string Search = RemoveSpecialCharacters(args2.ToLower());
                    ((DataTable)dataGridView1.DataSource).DefaultView.RowFilter = String.Format("Name like '%" + Search.Replace(" ", "%' AND Name LIKE '%").Replace(" and ", " ").Replace(" the ", " ").Replace(":", "") + "%'"); if (dataGridView1.Rows[0].Cells[1].Value.ToString() != null)
                        if (!String.IsNullOrEmpty(dataGridView1.Rows[0].Cells[1].Value.ToString()))
                            Clipboard.SetText($"{dataGridView1.Rows[0].Cells[1].Value.ToString()}");
                    if (String.IsNullOrWhiteSpace(dataGridView1.Rows[0].Cells[1].Value.ToString()))
                    {
                        dataTableGeneration = new DataTableGeneration();
                        Task.Run(async () => await dataTableGeneration.GetDataTableAsync(dataTableGeneration)).Wait();
                        dataGridView1.DataSource = dataTableGeneration.DataTableToGenerate;

                    }
                }
                catch { }
                searchTextBox.Text = "";

            }
            dataGridView1.Columns[0].SortMode = DataGridViewColumnSortMode.NotSortable;
            dataGridView1.Columns[1].SortMode = DataGridViewColumnSortMode.NotSortable;
            dataGridView1.ClearSelection();
            dataGridView1.DefaultCellStyle.SelectionBackColor = dataGridView1.DefaultCellStyle.BackColor;
            dataGridView1.DefaultCellStyle.SelectionForeColor = dataGridView1.DefaultCellStyle.ForeColor;
        }


        private void btnSearch_Click(object sender, EventArgs e)
        {
            searchTextBox.Clear();
            searchTextBox.Focus();
        }
        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (dataGridView1[1, e.RowIndex].Value == null)
                {
                    return;
                }
                if (dataGridView1.SelectedCells.Count > 0)
                {
                    Clipboard.SetText(dataGridView1[1, e.RowIndex].Value.ToString());
                    if (VRLExists)
                    {
                        string PropName = RemoveSpecialCharacters(dataGridView1[0, e.RowIndex].Value.ToString()).Trim();
                        File.WriteAllText($"{APPDATA}\\VRL\\ProperName.txt", PropName);
                    }
                    label3.Text = $"{dataGridView1[1, e.RowIndex].Value.ToString()} ({dataGridView1[0, e.RowIndex].Value.ToString()}) copied to clipboard.";
                    CurrentCell = e.RowIndex;
                }
            }
            catch { }


        }


        protected override bool ProcessDialogKey(Keys keyData)
        {
            if (Form.ModifierKeys == Keys.None && keyData == Keys.Escape)
            {
                if (searchTextBox.Text.Length > 0)
                {
                    searchTextBox.Clear();
                    searchTextBox.Focus();
                }
                return true;
            }
            return base.ProcessDialogKey(keyData);
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridView1[1, e.RowIndex].Value == null)
            {
                return;
            }
            try
            {
                if (VRLExists)
                {
                    string PropName = RemoveSpecialCharacters(dataGridView1[0, e.RowIndex].Value.ToString());
                    File.WriteAllText($"{APPDATA}\\VRL\\ProperName.txt", PropName);
                }
                Clipboard.SetText(dataGridView1[1, e.RowIndex].Value.ToString());
                this.Close();
            }
            catch { }

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (VRLExists)
                {
                    string PropName = RemoveSpecialCharacters(dataGridView1[0, e.RowIndex].Value.ToString());
                    File.WriteAllText($"{APPDATA}\\VRL\\ProperName.txt", PropName);
                }
                Clipboard.SetText(dataGridView1[1, e.RowIndex].Value.ToString());
            }
            catch
            {
            }
        }

        private void label2_Click(object sender, EventArgs e)
        {
            Process.Start("https://discord.gg/QvYwqvdgxc");
            Process.Start("https://t.me/FFAMain");
        }


        private void dataGridView1_KeyDown(object sender, KeyEventArgs e)
        {

            if (dataGridView1.Rows[0].Selected)
            {
                if (e.KeyCode == Keys.Up)
                {
                    searchTextBox.Clear();
                    searchTextBox.Focus();
                }
            }
            if (e.KeyCode == Keys.Escape)
            {
                searchTextBox.Clear();
                searchTextBox.Focus();
            }
            if (e.KeyCode == Keys.Enter)
            {
                if (dataGridView1[1, CurrentCell].Value == null)
                {
                    return;
                }
                if (VRLExists)
                {
                    string PropName = RemoveSpecialCharacters(dataGridView1[0, CurrentCell].Value.ToString());
                    File.WriteAllText($"{APPDATA}\\VRL\\ProperName.txt", PropName);
                }
                Clipboard.SetText(dataGridView1[1, CurrentCell].Value.ToString());
                this.Close();
            }
        }
        private void dataGridView1_KeyPress(object sender, KeyPressEventArgs e)
        {

            if (e.KeyChar == (char)Keys.Escape)
            {
                searchTextBox.Clear();
                searchTextBox.Focus();
            }
            if (e.KeyChar == (char)Keys.Enter)
            {
                if (dataGridView1[1, CurrentCell].Value == null)
                {
                    return;
                }

                if (VRLExists)
                {
                    string PropName = RemoveSpecialCharacters(dataGridView1[0, CurrentCell].Value.ToString());
                    File.WriteAllText($"{APPDATA}\\VRL\\ProperName.txt", PropName);
                }
                Clipboard.SetText(dataGridView1[1, CurrentCell].Value.ToString());
                this.Close();
            }
        }
        private void searchTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Down)
                {
                    dataGridView1.Focus();
                    e.SuppressKeyPress = true;
                }
                if (e.KeyCode == Keys.Enter)
                {
                    dataGridView1.Focus();
                    e.SuppressKeyPress = true;
                }
                if (e.KeyCode == Keys.Escape)
                {
                    searchTextBox.Clear();
                    searchTextBox.Focus();
                    e.SuppressKeyPress = true;
                }
            }
            catch { }
        }

        private void searchTextBox_TextChanged(object sender, EventArgs e)
        {
            try
            {
                string Search = searchTextBox.Text.ToLower();
                if (Search.StartsWith("$"))
                {
                    ((DataTable)dataGridView1.DataSource).DefaultView.RowFilter = String.Format("Name like '" + searchTextBox.Text.Replace("$", "") + "'");
                }
                else
                {
                    Search = RemoveSpecialCharacters(searchTextBox.Text.ToLower());
                    ((DataTable)dataGridView1.DataSource).DefaultView.RowFilter = String.Format("Name like '%" + Search.Replace(" ", "%' AND Name LIKE '%").Replace(" and ", "").Replace(" & ", "").Replace(":", "") + "%'");
                }
                dataGridView1.ClearSelection();
                dataGridView1.DefaultCellStyle.SelectionBackColor = dataGridView1.DefaultCellStyle.BackColor;
                dataGridView1.DefaultCellStyle.SelectionForeColor = dataGridView1.DefaultCellStyle.ForeColor;
                dataGridView1.Columns[0].SortMode = DataGridViewColumnSortMode.NotSortable;
                dataGridView1.Columns[1].SortMode = DataGridViewColumnSortMode.NotSortable;

            }
            catch { }
        }

        private void dataGridView1_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (VRLExists)
            {
                string PropName = RemoveSpecialCharacters(dataGridView1[0, e.RowIndex].Value.ToString());
                File.WriteAllText($"{APPDATA}\\VRL\\ProperName.txt", PropName);
            }
            Clipboard.SetText(dataGridView1[1, CurrentCell].Value.ToString());
            this.Close();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            Process.Start("https://github.com/harryeffinpotter/SteamAPPIDFinder");
        }

        private void dataGridView1_Enter(object sender, EventArgs e)
        {
            try
            {
                dataGridView1.DefaultCellStyle.SelectionBackColor = Color.SpringGreen;
                dataGridView1.DefaultCellStyle.SelectionForeColor = Color.Black;
                dataGridView1.Rows[0].Selected = true;
            }
            catch
            {

            }
        }

        private void searchTextBox_Enter(object sender, EventArgs e)
        {
            dataGridView1.ClearSelection();
            dataGridView1.DefaultCellStyle.SelectionBackColor = dataGridView1.DefaultCellStyle.BackColor;
            dataGridView1.DefaultCellStyle.SelectionForeColor = dataGridView1.DefaultCellStyle.ForeColor;
        }

        private void dataGridView1_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {

        }
    }
}