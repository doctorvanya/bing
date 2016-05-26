using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Bing
{
    [XmlRoot(ElementName="images")]
    public class Picture
    {
        [XmlElement(ElementName = "image")]
        public Images items { get; set; }
    }

    public class Images
    {
        [XmlElement(ElementName = "copyright")]
        public string Name { get; set; }

        [XmlElement(ElementName = "url")]
        public string Path { get; set; }
    }
}
