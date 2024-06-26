using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Extractor.Core.Model
{
    public class Group
    {
        public string Name { get; set; }
        public List<Field> Fields { get; set; }
        public Rectangle rect { get; set; }
    }
}
