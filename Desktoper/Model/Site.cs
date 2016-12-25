using System;

namespace Desktoper.Model
{
    [Serializable]
    class Site
    {
        public string Name { get; set; }
        public string URL  { get; set; }
        public string Desc { get; set; }
        public string Type { get; } = "Site";
        public string ImgPath { get; set; }
    }
}
