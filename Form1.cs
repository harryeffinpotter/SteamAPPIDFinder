﻿using System;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using CliWrap;
using SteamAppIdIdentifier;
using Clipboard = System.Windows.Forms.Clipboard;
using DataFormats = System.Windows.DataFormats;
using DragDropEffects = System.Windows.Forms.DragDropEffects;
using Timer = System.Windows.Forms.Timer;

namespace APPID
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
        public static void Tit(string Message)
        {
            Program.form.Text = Message;
        }
        public static void Tat(string Message)
        {
            Program.form.currDIrText.Text = $"Path: {Message}";
        }
        public static bool VRLExists = false;
        public static string RemoveSpecialCharacters(string str)
        {
            return Regex.Replace(str, "[^a-zA-Z0-9._0-]+", " ", RegexOptions.Compiled);
        }
        public async void SteamAppId_Load(object sender, EventArgs e)
        {
            if (Properties.Settings.Default.Pinned)
            {
                this.TopMost = true;
                unPin.BringToFront();
            }
            if (Properties.Settings.Default.Goldy)
            {
                dllSelect.SelectedIndex = 0;
            }
            else
            {
                dllSelect.SelectedIndex = 1;
            }
            Tit("Checking for Internet... ");
            bool check = Updater.CheckForNet();
            if (check)
            {
                Tit("Checking for Steamless update...");
                await Updater.CheckGitHubNewerVersion("atom0s", "Steamless", "https://api.github.com/repos");
                Tit("Checking for Goldberg update...");
                Updater.UpdateGoldBerg();
                Tit("Select directory...");
            }

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
                    CurrentCell = e.RowIndex;
                }
            }
            catch { }


        }
        public static String APPID;
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
                APPID = dataGridView1[1, e.RowIndex].Value.ToString();
                searchTextBox.Clear();
                mainPanel.Visible = true;
                startCrackPic.Visible = true;
                resinstruccZip.Visible = false;

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
                    APPID = dataGridView1[1, CurrentCell].Value.ToString();
                    searchTextBox.Clear();
                    mainPanel.Visible = true;
                    startCrackPic.Visible = true;

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
                if (e.Modifiers == Keys.LShiftKey && e.KeyCode == Keys.Oem4)
                {
                    searchTextBox.Text = $"${searchTextBox.Text}";
                    searchTextBox.SelectionStart = searchTextBox.Text.Length + 1;
                }
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
                else if (e.KeyCode == Keys.Enter)
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
            APPID = dataGridView1[1, CurrentCell].Value.ToString();
            searchTextBox.Clear();
            mainPanel.Visible = true;
            startCrackPic.Visible = true;
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            Process.Start("https://github.com/harryeffinpotter/SteamAPPIDFinder");
        }


        private void searchTextBox_Enter(object sender, EventArgs e)
        {

        }

        public void Crack()
        {
            int execount = -20;
            int steam64count = -1;
            int steamcount = -1;
            string parentdir = "";

            bool cracked = false;
            try
            {
                var files = Directory.GetFiles(gameDir, "*.*", SearchOption.AllDirectories);


                foreach (string file in files)
                {
                    if (file.EndsWith("steam_api64.dll"))
                    {
                        
                        steam64count++;
                        parentdir = Directory.GetParent(file).FullName;
                        string steam = $"{parentdir}\\steam_settings";
                        if (Directory.Exists(steam))
                        {
                            var filesz = Directory.GetFiles(steam, "*.*", SearchOption.AllDirectories);
                            foreach (var filee in filesz)
                            {
                                File.Delete(filee);
                            }
                            Directory.Delete(steam, true);
                        }
                        if (File.Exists($"{file}.bak"))
                        {
                            File.Delete(file);
                            File.Move($"{file}.bak", file);
                        }
                        Tit("Replacing steam_api64.dll.");
                        cracked = true;
                        File.Move(file, $"{file}.bak");
                        if (goldy)
                        {
                            Directory.CreateDirectory(steam);
                            File.Copy($"{Environment.CurrentDirectory}\\_bin\\Goldberg\\steam_api64.dll", file);
                        }
                        else
                        {
                            File.Copy($"{Environment.CurrentDirectory}\\_bin\\ALI213\\steam_api64.dll", file);
                            if (File.Exists(parentdir + "\\SteamConfig.ini"))
                            {
                                File.Delete(parentdir + "\\SteamConfig.ini");
                            }
                            IniFileEdit($"{Environment.CurrentDirectory}\\_bin\\ALI213\\SteamConfig.ini [Settings] \"AppID = {APPID}\"");
                            File.Copy($"{Environment.CurrentDirectory}\\_bin\\ALI213\\SteamConfig.ini", $"{parentdir}\\SteamConfig.ini");
                        }
                    }
                    if (file.EndsWith("steam_api.dll"))
                    {
                        steamcount++;
                        parentdir = Directory.GetParent(file).FullName;
                        string steam = $"{parentdir}\\steam_settings";
                        if (Directory.Exists(steam))
                        {
                            var filesz = Directory.GetFiles(steam, "*.*", SearchOption.AllDirectories);
                            foreach (var filee in filesz)
                            {
                                File.Delete(filee);
                            }
                            Directory.Delete(steam, true);
                        }
                        if (File.Exists($"{file}.bak"))
                        {
                            File.Delete(file);
                            File.Move($"{file}.bak", file);
                        }
                        Tit("Replacing steam_api.dll.");
                        parentdir = Directory.GetParent(file).FullName;
                        cracked = true;
                        File.Move(file, $"{file}.bak");

                        if (goldy)
                        {
                            Directory.CreateDirectory(steam);
                            File.Copy($"{Environment.CurrentDirectory}\\_bin\\Goldberg\\steam_api.dll", file);
                        }
                        else
                        {
                            File.Copy($"{Environment.CurrentDirectory}\\_bin\\ALI213\\steam_api.dll", file);
                            if (File.Exists(parentdir + "\\SteamConfig.ini"))
                            {
                                File.Delete(parentdir + "\\SteamConfig.ini");
                            }
                            IniFileEdit($"\".\\_bin\\ALI213\\SteamConfig.ini\" [Settings] \"AppID = {APPID}\"");
                            File.Copy("_bin\\ALI213\\SteamConfig.ini", $"{parentdir}\\SteamConfig.ini");
                        }

                    }
                    if (Path.GetExtension(file) == ".exe")
                    {
                        if (File.Exists($"{file}.bak"))
                        {
                            File.Delete(file);
                            File.Move($"{file}.bak", file);
                        }

                        string exeparent = Directory.GetParent(file).FullName;
                        Process x2 = new Process();
                        ProcessStartInfo pro = new ProcessStartInfo();
                        pro.WindowStyle = ProcessWindowStyle.Hidden;
                        pro.UseShellExecute = false;
                        pro.CreateNoWindow = true;
                        pro.RedirectStandardError = true;
                        pro.RedirectStandardOutput = true;
                        pro.WorkingDirectory = parentdir;
                        pro.FileName = @"C:\Windows\System32\cmd.exe";
                        pro.Arguments = $"/c start /B \"\" \"{Environment.CurrentDirectory}\\_bin\\Steamless\\Steamless.CLI.exe\" \"{file}\"";
                        x2.StartInfo = pro;
                        x2.Start();
                        string Output = x2.StandardError.ReadToEnd() + x2.StandardOutput.ReadToEnd();
                        x2.WaitForExit();
                        if (File.Exists($"{file}.unpacked.exe"))
                        {
                            Tit($"Unpacked {file} successfully!");
                            File.Move(file, file + ".bak");
                            File.Move($"{file}.unpacked.exe", file);
                        }

                    }
                    if (steamcount > 1 || execount > 1 || steam64count > 1)
                    {
                        DialogResult Diagg = MessageBox.Show(
                            "This is the 2nd steam_api64.dll on this run - something is broken. " +
                            "The APPID has to match the ini/txt files or the cracks will not work.\n\n" +
                            "This usually happens when SACGUI determines the wrong parent dir " +
                            "from EXE or when user selects incorrect folder. Please select " +
                            "the GAME DIRECTORY, for example:\n\n" +
                            "-CORRECT-\nD:\\SteamLibrary\\Common\\SomeGameName\n\n" +
                            "-INCORRECT-\nD:\\SteamLibrary\\Common\n\n" +
                            "If you think this message is wrong, verify the path on bottom left and hit YES to continue..",
                            "Somethings wrong..., continue?", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                        if (Diagg == DialogResult.Yes)
                        {
                            execount = -600;
                            steam64count = -600;
                            steamcount = -600;
                        }
                        else
                        {
                            break;
                        }
                    }

                }


                if (cracked)
                {
                    if (goldy)
                    {
                    if (Directory.Exists($"{parentdir}\\steam_settings"))
                    {
                        Directory.Delete($"{parentdir}\\steam_settings");
                    }
                    Directory.CreateDirectory($"{parentdir}\\steam_settings");
                    Tit("Generating achievements and DLC info...");
                    var infos = Cli.Wrap($"{Environment.CurrentDirectory}\\_bin\\generate_game_infos.exe").WithValidation(CommandResultValidation.None).WithArguments($"{APPID} -s 92CD46192F62DE1A769F79A667CE5631 -o\"{parentdir}\\steam_settings\"").WithWorkingDirectory(parentdir).ExecuteAsync().GetAwaiter().GetResult();

                    }
                }
            }
            catch (Exception ex){ File.WriteAllText("_CRASHlog.txt", ex.StackTrace + "\n" + ex.Message); }
        }

        public void IniFileEdit(string args)
        {
            Process IniProcess = new Process();
            IniProcess.StartInfo.CreateNoWindow = true;
            IniProcess.StartInfo.UseShellExecute = false;
            IniProcess.StartInfo.FileName = $"{Environment.CurrentDirectory}\\_bin\\ALI213\\inifile.exe";
            IniProcess.StartInfo.Arguments = args;
            IniProcess.Start();
            IniProcess.WaitForExit();
        }

        private string parentOfSelection;
        private string gameDir;
        private string gameDirName;
        private void pictureBox2_Click(object sender, EventArgs e)
        {

            FolderSelectDialog folderSelectDialog = new FolderSelectDialog();
            folderSelectDialog.Title = "Select the game's main folder.";

            if (Properties.Settings.Default.lastDir.Length > 0)
                folderSelectDialog.InitialDirectory = Properties.Settings.Default.lastDir;

            if (folderSelectDialog.Show(Handle))
            {
                if (folderSelectDialog.FileName.Contains("Program Files"))
                {
                    MessageBox.Show("It looks like you selected a Program Files directory, " +
                        "this will often cause the autocrack to fail.");
                }
                gameDir = folderSelectDialog.FileName;
                Tat(gameDir);
                parentOfSelection = Directory.GetParent(gameDir).FullName;
                gameDirName = Path.GetFileName(gameDir);
                mainPanel.Visible = false;
                startCrackPic.Visible = true;
                searchTextBox.Text = gameDirName;
                Properties.Settings.Default.lastDir = parentOfSelection;
                Properties.Settings.Default.Save();
            }
            else
            { 
                MessageBox.Show("You must select a folder to continue..."); 
            }
        }
        public bool cracking;
        private async void startCrackPic_Click(object sender, EventArgs e)
        {
           if (!cracking)
            {
                cracking = true;

                Crack();
                cracking = false;
                Tit("Crack complete!");
                donePic.Visible = true;
                await Task.Delay(3000);
                startCrackPic.Visible = false;
                donePic.Visible = false;
                Tit("Select directory...");
                Tat("");
            }
        }
        public bool goldy = false;
        private void dllSelect_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (dllSelect.SelectedIndex == 0)
            {
                goldy = true;
                Properties.Settings.Default.Goldy = true;
            }
            else if (dllSelect.SelectedIndex == 1)
            {
                goldy = false;
                Properties.Settings.Default.Goldy = false;
            }
            Properties.Settings.Default.Save();
        }
        private void selectDir_MouseEnter(object sender, EventArgs e)
        {
            selectDir.Image = Image.FromFile("_bin\\hoveradd.png");
        }
        private void selectDir_MouseHover(object sender, EventArgs e)
        {
            selectDir.Image = Image.FromFile("_bin\\hoveradd.png");
        }

        private void selectDir_MouseDown(object sender, MouseEventArgs e)
        {
            selectDir.Image = Image.FromFile("_bin\\clickadd.png");
        }

        private void startCrackPic_MouseDown(object sender, MouseEventArgs e)
        {
            startCrackPic.Image = Image.FromFile("_bin\\clickr2c.png");
        }

        private void startCrackPic_MouseEnter(object sender, EventArgs e)
        {
            startCrackPic.Image = Image.FromFile("_bin\\hoverr2c.png");
        }

        private void startCrackPic_MouseHover(object sender, EventArgs e)
        {
            startCrackPic.Image = Image.FromFile("_bin\\hoverr2c.png");
        }

        private void selectDir_MouseLeave(object sender, EventArgs e)
        {
            selectDir.Image = Image.FromFile("_bin\\add.png");
        }

        private void startCrackPic_MouseLeave(object sender, EventArgs e)
        {
            startCrackPic.Image = Image.FromFile("_bin\\r2c.png");
        }

        private void mainPanel_DragEnter(object sender, System.Windows.Forms.DragEventArgs e)
        {
            drgdropText.BringToFront();
            drgdropText.Visible = true;
            e.Effect = DragDropEffects.Copy;
        }

        private void mainPanel_DragLeave(object sender, EventArgs e)
        {
            drgdropText.SendToBack();
            drgdropText.Visible = false;
        }

        private void mainPanel_DragDrop(object sender, System.Windows.Forms.DragEventArgs e)
        {
   
            string[] drops = (string[])e.Data.GetData(DataFormats.FileDrop);
            foreach (string d in drops)
            {
                FileAttributes attr = File.GetAttributes(d);
                string parent = Directory.GetParent(d).FullName;
                //detect whether its a directory or file
                if ((attr & FileAttributes.Directory) == FileAttributes.Directory)
                {
                    //DIR
                    Tat(d);
                    gameDir = d;
                    parentOfSelection = Directory.GetParent(gameDir).FullName;
                    gameDirName = Path.GetFileName(gameDir);
                    mainPanel.Visible = false;
                    startCrackPic.Visible = true;
                    searchTextBox.Text = gameDirName;
                    Properties.Settings.Default.lastDir = parentOfSelection;
                    Properties.Settings.Default.Save();
                }
                else
                {
                    //FILE
                    if (Path.GetExtension(d) != ".exe")
                    {
                        return;
                    }
                    bool parentfound = false;
                    string[] dirs = Directory.GetDirectories(parent);
                    foreach (string dir in dirs)
                    {
                        if (dir.ToLower().EndsWith("_data") || dir.ToLower().EndsWith("\\engine"))
                        {
                            parentfound = true;
                            gameDir = Directory.GetParent(dir).FullName;
                            Tat(gameDir);
                            parentOfSelection = Directory.GetParent(gameDir).FullName;
                            gameDirName = Path.GetFileName(gameDir);
                            mainPanel.Visible = false;
                            startCrackPic.Visible = true;
                            searchTextBox.Text = gameDirName;
                            Properties.Settings.Default.lastDir = parentOfSelection;
                            Properties.Settings.Default.Save();
                        }
                    }
                    if (!parentfound)
                    {
                        try
                        {
                            parent = Directory.GetParent(parent).FullName;
                            parent = Directory.GetParent(parent).FullName;
                            parent = Directory.GetParent(parent).FullName;
                            string[] topdirs = Directory.GetDirectories(parent);
                            foreach (string topdir in topdirs)
                            {
                                if (topdir.ToLower().EndsWith("engine"))
                                {
                                    parentfound = true;
                                    gameDir = parent;
                                    Tat(gameDir);
                                    parentOfSelection = Directory.GetParent(gameDir).FullName;
                                    gameDirName = Path.GetFileName(gameDir);
                                    mainPanel.Visible = false;
                                    startCrackPic.Visible = true;
                                    searchTextBox.Text = gameDirName;
                                    Properties.Settings.Default.lastDir = parentOfSelection;
                                    Properties.Settings.Default.Save();
                                }
                            }
                        }
                        catch(Exception Ex)
                        {
                        if (!Ex.Message.ToLower().Contains("object"))
                            {
                                MessageBox.Show(Ex.Message);
                            }
                        }

                    }
                    if (!parentfound)
                    {
                        MessageBox.Show("No recognizable game folder found!" +
                            "\nTry the top directory of the game you're trying to crack instead!");
                    }
                }
            }
            drgdropText.SendToBack();
            drgdropText.Visible = false;
        }

        private void unPin_Click(object sender, EventArgs e)
        {
            this.TopMost = false;
            pin.BringToFront();
            Properties.Settings.Default.Pinned = false;
            Properties.Settings.Default.Save();
        }

        private void pin_Click(object sender, EventArgs e)
        {
            this.TopMost = true;
            unPin.BringToFront();
            Properties.Settings.Default.Pinned = true;
            Properties.Settings.Default.Save();
        }
    }
}