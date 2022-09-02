using System;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

using static System.Windows.Forms.VisualStyles.VisualStyleElement;

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
            t1 = new Timer();
            t1.Tick += new EventHandler(t1_Tick);
            t1.Interval = 1000;
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
            try
            {



                if (e.KeyCode == Keys.Up)
                {
                    if (dataGridView1.Rows.Count > 0)
                    {
                        if (dataGridView1.Rows[0].Selected)
                        {

                            searchTextBox.Focus();
                        }
                    }
                }
                if (e.KeyCode == Keys.Escape)
                {
                    searchTextBox.Clear();
                    searchTextBox.Focus();
                }
                else if (e.KeyCode == Keys.Enter)
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
                else if (e.KeyCode == Keys.Back)
                {
                    string s = searchTextBox.Text;

                    if (s.Length > 1)
                    {
                        s = s.Substring(0, s.Length - 1);
                    }
                    else
                    {
                        s = "0";
                    }

                    searchTextBox.Text = s;
                    searchTextBox.Focus();
                    dataGridView1.ClearSelection();
                    searchTextBox.SelectionStart = searchTextBox.Text.Length;
                }
                else if (e.KeyCode != Keys.Up && e.KeyCode != Keys.Down && char.IsLetterOrDigit((char)e.KeyCode) && e.KeyCode.ToString().Length == 1 || e.KeyCode == Keys.Space)
                {
                    bool isShiftKeyPressed = (e.Modifiers == Keys.Shift);
                    bool isCapsLockOn = System.Windows.Forms.Control
                         .IsKeyLocked(System.Windows.Forms.Keys.CapsLock);
                    if (e.KeyCode == Keys.Space)
                    {
                        searchTextBox.Focus();
                        searchTextBox.Text += " ";
                        dataGridView1.ClearSelection();
                        searchTextBox.SelectionStart = searchTextBox.Text.Length;
                    }
                    else if (!isShiftKeyPressed && !isCapsLockOn)
                    {
                        searchTextBox.Focus();
                        searchTextBox.Text += e.KeyData.ToString().ToLower();
                        searchTextBox.SelectionStart = searchTextBox.Text.Length;
                    }
                    else
                    {
                        searchTextBox.Focus();
                        searchTextBox.Text += e.KeyData.ToString().Replace(", Shift", "").Replace("Oem", "").Replace("numpad", "");
                        searchTextBox.SelectionStart = searchTextBox.Text.Length;
                    }

                }
            }
            catch { }
        }


        private void searchTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.Modifiers == Keys.Control && e.KeyCode == Keys.A)
                {
                    searchTextBox.SelectAll();
                    e.SuppressKeyPress = true;
                }
                if (dataGridView1.Rows.Count == 0)
                {
                    return;
                }
                if (e.KeyCode == Keys.Down)
                {
                    dataGridView1.Focus();
                    dataGridView1.Rows[0].Selected = true;
                    dataGridView1.SelectedCells[0].Selected = true;

                    e.SuppressKeyPress = true;
                }
                else if (e.KeyCode == Keys.Tab)
                {
                    dataGridView1.Focus();
                    dataGridView1.Rows[0].Selected = true;
                    dataGridView1.SelectedCells[0].Selected = true;
                    e.SuppressKeyPress = true;
                }
                else  if (e.KeyCode == Keys.Enter)
                {
                    dataGridView1.Focus();
                    dataGridView1.Rows[0].Selected = true;
                    dataGridView1.SelectedCells[0].Selected = true;
                    e.SuppressKeyPress = true;
                }
                else if (e.KeyCode == Keys.Escape)
                {
                    searchTextBox.Clear();
                    searchTextBox.Focus();
                    e.SuppressKeyPress = true;
                }
                else if (e.KeyCode == Keys.Back)
                {
                    BackPressed = true;
                }
                else
                {
                    BackPressed = false;
                    SearchPause = false;
                    t1.Stop();
                }
            }
            catch { }
        }
        public static bool SearchPause = false;
        public static bool BackPressed = false;
        public static void t1_Tick(object sender, EventArgs e)
        {
            BackPressed = false;
            SearchPause = false;
            t1.Stop();
        }
        public static Timer t1;
        private void searchTextBox_TextChanged(object sender, EventArgs e)
        {
            if (SearchPause && searchTextBox.Text.Length > 0)
            {
                return;
            }

            try
            {
                var fore = dataGridView1.DefaultCellStyle.BackColor;
                string Search = searchTextBox.Text.ToLower();
                if (Search.StartsWith("$"))
                {
                    ((DataTable)dataGridView1.DataSource).DefaultView.RowFilter = String.Format("Name like '" + Search.Replace("$", "").Replace("&", "and").Replace(":", " -") + "'");
                }
                else
                {
                    Search = RemoveSpecialCharacters(searchTextBox.Text.ToLower());
                    ((DataTable)dataGridView1.DataSource).DefaultView.RowFilter = String.Format("Name like '%" + Search.Replace(" ", "%' AND Name LIKE '%").Replace(" and ", "").Replace(" & ", "").Replace(":", "") + "%'");
                }

                dataGridView1.Columns[0].SortMode = DataGridViewColumnSortMode.NotSortable;
                dataGridView1.Columns[1].SortMode = DataGridViewColumnSortMode.NotSortable;
                if (BackPressed)
                {
                    SearchPause = true;
                    t1.Start();
                }

                dataGridView1.ClearSelection();
                dataGridView1.DefaultCellStyle.SelectionBackColor = dataGridView1.DefaultCellStyle.BackColor;
                dataGridView1.DefaultCellStyle.SelectionForeColor = dataGridView1.DefaultCellStyle.ForeColor;

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

            }
            catch
            {

            }
        }

        private void searchTextBox_Enter(object sender, EventArgs e)
        {

        }

        private void dataGridView1_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {

        }
    }
}