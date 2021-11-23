using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MintPlayer.IconUtils;

namespace Launcher
{
    public partial class IconTest : Form
    {
        public IconTest()
        {
            GetIcons();
            InitializeComponent();
        }

        protected override void OnLoad(EventArgs e)
        {
            Visible = false;
            //ShowInTaskbar = false;

            base.OnLoad(e);
        }

        private void OnExit(object sender, EventArgs e)
        {
            Application.Exit();
        }

        public void GetIcons()
        {
            string folder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "IconTest");
            if (!Directory.Exists(folder))
                Directory.CreateDirectory(folder);

            List<Icon> icons = IconExtractor.Split(@"C:\Program Files\Vivaldi\Application\vivaldi.exe");
            //List<Icon> icons = IconExtractor.Split(@"C:\Program Files (x86)\BraveSoftware\Brave-Browser\Application\brave.exe");
            //List<Icon> icons = IconExtractor.Split(@"C:\Program Files (x86)\Google\Chrome\Application\chrome.exe");
            //List<Icon> icons = IconExtractor.Split(@"C:\Program Files (x86)\Microsoft\Edge\Application\msedge.exe");
            //List<Icon> icons = IconExtractor.Split(@"C:\Program Files\Mozilla Firefox\firefox.exe");
            var index = 1000;

            foreach (var icon in icons)
            {
                var filename = Path.Combine(folder, "icon_" + (index++).ToString() + ".ico");
                using (var fs = new FileStream(filename, FileMode.Create, FileAccess.ReadWrite))
                {
                    icon.Save(fs);
                }
            }
        }
    }
}
