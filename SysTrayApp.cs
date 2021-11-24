using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace Launcher
{
    public partial class SysTrayApp : Form
    {
        private Menu launcherMenu;
        private NotifyIcon notifyIcon;
        private ContextMenuStrip contextMenu;
        private readonly string configFile = "launcher.cfg";

        public SysTrayApp()
        {
            contextMenu = new ContextMenuStrip();
            PopulateContextMenu();
            InitializeNotifyIcon();
            notifyIcon.ContextMenuStrip = contextMenu;
            InitializeComponent();
        }

        protected override void OnLoad(EventArgs e)
        {
            Visible = false;
            ShowInTaskbar = false;

            base.OnLoad(e);
        }

        private void OnExit(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void InitializeNotifyIcon()
        {
            notifyIcon = new NotifyIcon
            {
                Text = "Launcher",
                Icon = new Icon("Launcher-line-40px.ico"),
                Visible = true
            };
        }

        private void PopulateContextMenu()
        {
            string configPath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
            launcherMenu = new Menu($"{configPath}\\{configFile}");

            List<string> categories = GetCategories(launcherMenu.Entries);
            categories.Sort();

            foreach (var category in categories)
            {
                var submenu = new ToolStripMenuItem
                {
                    Text = category
                };

                foreach (var entry in launcherMenu.Entries)
                {
                    if (entry.Category == category)
                    {
                        var item = new ToolStripMenuItem
                        {
                            Text = entry.Name
                        };
                        Icon appIcon = Icon.ExtractAssociatedIcon(entry.Path);
                        item.Image = appIcon.ToBitmap();
                        item.Click += MenuItem_Click;
                        submenu.DropDownItems.Add(item);
                    }
                }

                contextMenu.Items.Add(submenu);
            }

            AddDefaultEntries();
        }

        private void MenuItem_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem item = sender as ToolStripMenuItem;
            string menuItemTarget = string.Empty;
            string targetArguments = string.Empty;

            foreach (var entry in launcherMenu.Entries)
            {
                if (item.Text == entry.Name)
                {
                    menuItemTarget = entry.Path;

                    targetArguments = string.Empty;
                    if (!string.IsNullOrEmpty(entry.Arguments))
                    {
                        targetArguments = entry.Arguments;
                    }

                    break;
                }
                else
                {
                    menuItemTarget = string.Empty;
                    targetArguments = string.Empty;
                }
            }

            Process process = new Process();
            process.StartInfo.FileName = menuItemTarget;
            process.StartInfo.Arguments = targetArguments;
            process.Start();
        }

        private void RefreshMenu(object sender, EventArgs e)
        {
            notifyIcon.Visible = false;
            contextMenu.Dispose();
            contextMenu = new ContextMenuStrip();

            PopulateContextMenu();

            notifyIcon.ContextMenuStrip = contextMenu;
            notifyIcon.Visible = true;
        }

        private List<string> GetCategories(List<MenuEntry> entries)
        {
            List<string> categoryList = new List<string>();

            foreach (var entry in entries)
            {
                if (!string.IsNullOrEmpty(entry.Category) && !categoryList.Contains(entry.Category))
                {
                    categoryList.Add(entry.Category);
                }
            }

            return categoryList;
        }

        private void AddDefaultEntries()
        {
            contextMenu.Items.Add(new ToolStripSeparator());

            ToolStripMenuItem calculatorApp = new ToolStripMenuItem();
            calculatorApp.Text = "Calculator";
            calculatorApp.Click += RunCalculator;
            contextMenu.Items.Add(calculatorApp);

            ToolStripMenuItem notepadApp = new ToolStripMenuItem();
            notepadApp.Text = "Notepad";
            notepadApp.Click += RunNotepad;
            contextMenu.Items.Add(notepadApp);

            ToolStripMenuItem terminalApp = new ToolStripMenuItem();
            terminalApp.Text = "Terminal";
            terminalApp.Click += RunTerminal;
            contextMenu.Items.Add(terminalApp);

            contextMenu.Items.Add(new ToolStripSeparator());

            ToolStripMenuItem refreshMenu = new ToolStripMenuItem();
            refreshMenu.Text = "Refresh";
            refreshMenu.Click += RefreshMenu;
            contextMenu.Items.Add(refreshMenu);

            ToolStripMenuItem exitMenu = new ToolStripMenuItem();
            exitMenu.Text = "Exit";
            exitMenu.Click += OnExit;
            contextMenu.Items.Add(exitMenu);
        }

        private void RunCalculator(object sender, EventArgs e)
        {
            Process calculator = new Process();
            calculator.StartInfo.FileName = "calc.exe";
            calculator.Start();
        }

        private void RunNotepad(object sender, EventArgs e)
        {
            Process notepad = new Process();
            notepad.StartInfo.FileName = "notepad.exe";
            notepad.Start();
        }

        private void RunTerminal(object sender, EventArgs e)
        {
            Process notepad = new Process();
            notepad.StartInfo.FileName = "wt.exe";
            notepad.Start();
        }
    }
}
