using System;
using System.Collections.Generic;
using System.IO;

namespace Launcher
{
    class Menu
    {
        private readonly List<MenuEntry> _entries = new List<MenuEntry>();

        public List<MenuEntry> Entries => _entries;

        public Menu(string configFile)
        {
            if (!File.Exists(configFile))
                return;

            string line;
            var filestream = new FileStream(configFile, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            var file = new StreamReader(filestream);

            while ((line = file.ReadLine()) != null)
            {
                if (string.IsNullOrWhiteSpace(line)) continue;
                if (line.StartsWith("#")) continue;
                _entries.Add(new MenuEntry(line));
            }

            if (_entries.Count > 1)
            {
                _entries.Sort();
            }
        }
    }
}
