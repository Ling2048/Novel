using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class BookPageModel
    {
        public string PicHref { get; set; }
        public string Review { get; set; }
        public string Title { get; set; }
        public IList<ChapterLink> ChapterList { get; set; }
    }

    public class ChapterLink
    {
        public string Name { get; set; }
        public string URL { get; set; }
    }
}
