using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Desktoper.Model
{
    [Serializable]
    class UFile
    {
        public string Name { get; set; }
        public string FilePath { get; set; }
        public string Type { get; } = "File";
        public string ImgPath { get; set; }
    }
}
