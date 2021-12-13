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
                Icon = new Icon("Launcher-40px.ico"),
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

        private void ConfigureMenu(object sender, EventArgs e)
        {
            string configPath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
            Process notepad = new Process();
            notepad.StartInfo.FileName = "notepad.exe";
            notepad.StartInfo.Arguments = $" {configPath}\\{configFile}";
            notepad.Start();
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

            ToolStripMenuItem configureMenu = new ToolStripMenuItem();
            configureMenu.Text = "Configure";
            configureMenu.Click += ConfigureMenu;
            contextMenu.Items.Add(configureMenu);

            ToolStripMenuItem refreshMenu = new ToolStripMenuItem();
            refreshMenu.Text = "Refresh";
            refreshMenu.Click += RefreshMenu;
            contextMenu.Items.Add(refreshMenu);

            ToolStripMenuItem exitMenu = new ToolStripMenuItem();
            exitMenu.Text = "Exit";
            exitMenu.Click += OnExit;
            contextMenu.Items.Add(exitMenu);
        }
    }
}
