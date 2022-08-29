using System;
using System.Data;
using System.Diagnostics;
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
        static string BetterReplace(string originalString, string oldValue, string newValue)
        {
            Regex regEx = new Regex(oldValue,
            RegexOptions.IgnoreCase | RegexOptions.Multiline);
            return regEx.Replace(originalString, newValue);
        }
        public static string RemoveSpecialCharacters(string str)
        {
            return Regex.Replace(str, "[^a-zA-Z0-9._0-]+", " ", RegexOptions.Compiled);
        }
        private void SteamAppId_Load(object sender, EventArgs e)
        {

            searchTextBox.Text = "         ";
            string args3;
            dataGridView1.DataSource = dataTableGeneration.DataTableToGenerate;
            dataGridView1.SelectionMode = DataGridViewSelectionMode.CellSelect;
            dataGridView1.MultiSelect = false;
            string args2 = "";
            dataGridView1.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            dataGridView1.Columns[0].Width = 550;
            foreach (string args1 in Program.args2)
            {
                args3 = RemoveSpecialCharacters(args1);
                args2 += args3 + " ";
            }
            try
            {
                string Search = RemoveSpecialCharacters(args2.ToLower()).Trim();
                ((DataTable)dataGridView1.DataSource).DefaultView.RowFilter = String.Format("Name like '%" + Search.Replace(" ", "%' AND Name LIKE '%").Replace("and", "").Replace("the", "").Replace(":", "") + "%'"); if (dataGridView1.Rows[0].Cells[1].Value.ToString() != null)
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
            dataGridView1.Columns[0].SortMode = DataGridViewColumnSortMode.NotSortable;
            dataGridView1.Columns[1].SortMode = DataGridViewColumnSortMode.NotSortable;
        }


        private void btnSearch_Click(object sender, EventArgs e)
        {
            dataTableGeneration = new DataTableGeneration();
            Task.Run(async () => await dataTableGeneration.GetDataTableAsync(dataTableGeneration)).Wait();
            dataGridView1.DataSource = dataTableGeneration.DataTableToGenerate;

            if (dataGridView1.Rows.Count > 1)
            {
                dataGridView1.CurrentCell = this.dataGridView1[1, 1];
                try
                {
                    string Search = RemoveSpecialCharacters(searchTextBox.Text.ToLower()).Trim();
                    ((DataTable)dataGridView1.DataSource).DefaultView.RowFilter = String.Format("Name like '%" + Search.Replace(" ", "%' AND Name LIKE '%").Replace("and", "").Replace("the", "").Replace(":", "") + "%'");

                    if (dataGridView1.Rows[0].Cells[1].Value.ToString() != null)
                        Clipboard.SetText(dataGridView1.Rows[1].Cells[1].Value.ToString());


                    CurrentCell = 0;
                }
                catch
                {
                }
            }
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
                    string PropName = RemoveSpecialCharacters(dataGridView1[0, e.RowIndex].Value.ToString()).Trim();
					File.WriteAllText($"{APPDATA}\\VRL\\ProperName.txt",PropName);
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
                    searchTextBox.Text = "";
                }
                this.Close();
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
                string PropName = RemoveSpecialCharacters(dataGridView1[0, e.RowIndex].Value.ToString());
                File.WriteAllText($"{APPDATA}\\VRL\\ProperName.txt", PropName);
                Clipboard.SetText(dataGridView1[1, e.RowIndex].Value.ToString());
            }
            catch { }
            Application.Exit();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {


        }

        private void label2_Click(object sender, EventArgs e)
        {
            Process.Start("https://discord.gg/QvYwqvdgxc");
            Process.Start("https://t.me/FFAMain");
        }


        private void dataGridView1_KeyDown(object sender, KeyEventArgs e)
        {
            if (dataGridView1[1, CurrentCell].Value == null)
            {
                return;
            }
            if (e.KeyCode == Keys.Escape)
            {
                this.Close();
            }
            if (e.KeyCode == Keys.Enter)
            {
                string PropName = RemoveSpecialCharacters(dataGridView1[0, CurrentCell].Value.ToString());
                File.WriteAllText($"{APPDATA}\\VRL\\ProperName.txt", PropName);
                Clipboard.SetText(dataGridView1[1, CurrentCell].Value.ToString());
                this.Close();
            }
            
        }
        private void dataGridView1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (dataGridView1[1, CurrentCell].Value == null)
            {
                return;
            }
            if (e.KeyChar == (char)Keys.Escape)
            {
                this.Close();
            }
            if (e.KeyChar == (char)Keys.Enter)
            {
                string PropName = RemoveSpecialCharacters(dataGridView1[0, CurrentCell].Value.ToString());
                File.WriteAllText($"{APPDATA}\\VRL\\ProperName.txt", PropName);
                Clipboard.SetText(dataGridView1[1, CurrentCell].Value.ToString());
                this.Close();
            }
        }
        private void searchTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    if (searchTextBox.Text.Length > 0)
                    {
                        btnSearch_Click(sender, e);
                        searchTextBox.Clear();
                    }
                }
            }
            catch { }
        }

        private void searchTextBox_TextChanged(object sender, EventArgs e)
        {
            try
            {
                string Search = RemoveSpecialCharacters(searchTextBox.Text.ToLower());
                ((DataTable)dataGridView1.DataSource).DefaultView.RowFilter = String.Format("Name like '%" + Search.Replace(" ", "%' AND Name LIKE '%").Replace("and", "").Replace("the", "").Replace(":", "") + "%'");
            }
            catch { }
        }

        private void dataGridView1_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            string PropName = RemoveSpecialCharacters(dataGridView1[0, e.RowIndex].Value.ToString());
            File.WriteAllText($"{APPDATA}\\VRL\\ProperName.txt", PropName);
            Clipboard.SetText(dataGridView1[1, CurrentCell].Value.ToString());
            this.Close();
        }
    }
}