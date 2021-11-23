using System;
using System.IO;

namespace Launcher
{
    class MenuEntry : IComparable<MenuEntry>
    {
        private readonly string _name;
        private readonly string _path;
        private readonly string _arguments;
        private readonly string _icon;
        private readonly string _category;
        private readonly string _groupInCategory;

        public string Name => _name;
        public string Path => _path;
        public string Arguments => _arguments;
        public string Icon => _icon;
        public string Category => _category;
        public string GroupInCategory => _groupInCategory;

        public MenuEntry(string configurationLine)
        {
            string[] fields = configurationLine.Split(',');

            if (string.IsNullOrEmpty(fields[1]))
                throw new ArgumentException("The file path cannot be null or empty.");

            _path = fields[1];

            if (!File.Exists(_path))
                throw new ArgumentException($"{_path} does not exist.");

            _name = string.IsNullOrEmpty(fields[0]) ? string.Empty : fields[0];
            _icon = string.IsNullOrEmpty(fields[2]) ? string.Empty : fields[2];
            _category = string.IsNullOrEmpty(fields[3]) ? string.Empty : fields[3];
            // TODO: If field is not specified in the config file, an exception will be thrown
            _groupInCategory = string.IsNullOrEmpty(fields[4]) ? string.Empty : fields[4];
            _arguments = string.IsNullOrEmpty(fields[5]) ? string.Empty : fields[5];
        }

        public int CompareTo(MenuEntry that)
        {
            return this.Name.CompareTo(that.Name);
        }
    }
}
