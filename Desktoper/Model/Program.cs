using System;

namespace Desktoper.Model
{
    [Serializable]
    class Program
    {
        public string Name { get; set; }
        public string WorkingDirectory { get; set; }
        public string Type { get; } = "Program";
        public string ImgPath { get; set; }
    }
}
