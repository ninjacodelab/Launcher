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
        private NotifyIcon trayIcon;
        private ContextMenu trayMenu;
        private readonly string configFile = "_menu.cfg";

        public SysTrayApp()
        {
            CreateMenu();
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

        private void MyClickHandler(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                // TODO: Figure out how to send a right mouse click or figure
                //       how to get menu to display after mouse click on
                //       desktop, at which point this will not be needed
                var notifyIcon = sender as NotifyIcon;
                // trayMenu.Show() ?

                //throw new Exception("Left mouse button clicked");
            }
        }

        private void CreateMenu()
        {
            // Create a simple tray menu with only one item.
            trayMenu = new ContextMenu();
            PopulateMenu();
            AddDefaultEntries();

            // Create a tray icon. In this example we use a
            // standard system icon for simplicity, but you
            // can of course use your own custom icon too.
            trayIcon = new NotifyIcon();
            trayIcon.Text = "Launcher";
            //trayIcon.Icon = new Icon(SystemIcons.Application, 40, 40);
            trayIcon.Icon = new Icon("Launcher-line-40px.ico");
            trayIcon.MouseClick += MyClickHandler;

            // Add menu to tray icon and show it.
            trayIcon.ContextMenu = trayMenu;
            trayIcon.Visible = true;
        }

        private void PopulateMenu()
        {
            string configPath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
            launcherMenu = new Menu($"{configPath}\\{configFile}");

            List<string> categories = GetCategories(launcherMenu.Entries);
            categories.Sort();

            foreach (var category in categories)
            {
                List<MenuItem> items = new List<MenuItem>();

                foreach (var entry in launcherMenu.Entries)
                {
                    if (entry.Category == category)
                    {
                        var item = new MenuItem
                        {
                            Text = entry.Name
                        };
                        item.Click += LaunchMenuItem;
                        items.Add(item);
                    }
                }

                trayMenu.MenuItems.Add(category, items.ToArray());
            }
        }

        private void LaunchMenuItem(object sender, EventArgs e)
        {
            MenuItem item = sender as MenuItem;
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
            trayIcon.Visible = false;
            trayMenu.Dispose();
            trayMenu = new ContextMenu();

            PopulateMenu();
            AddDefaultEntries();

            trayIcon.ContextMenu = trayMenu;
            trayIcon.Visible = true;
        }

        private void AddDefaultEntries()
        {
            trayMenu.MenuItems.Add("-");
            trayMenu.MenuItems.Add("Calculator", RunCalculator);
            trayMenu.MenuItems.Add("Notepad", RunNotepad);
            trayMenu.MenuItems.Add("Terminal", RunTerminal);
            trayMenu.MenuItems.Add("-");
            trayMenu.MenuItems.Add("Refresh", RefreshMenu);
            trayMenu.MenuItems.Add("Exit", OnExit);
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
