
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace NewsOutlet
{
    class News
    {
        int id;
        long time;
        string[] keywords;
        string content;
        int hits;

        public News(int id, long time, string[] keywords, string content, int hits)
        {
            this.id = id;
            this.time = time;
            this.keywords = keywords;
            this.content = content;
            this.hits = hits;
        }


        public int Id
        {
            get { return id; }
            set { id = value; }
        }
        
        public long Time
        {
            get { return time; }
            set { time = value; }
        }

        public string[] Keywords
        {
            get { return keywords; }
            set { keywords = value; }
        }

        public string Content
        {
            get { return content; }
            set { content = value; }
        }

        public int Hits
        {
            get { return hits; }
            set { hits = value; }
        }

        public override string ToString()
        {
            string newsKeywords = ""; 
            for (int i = 0; i < keywords.Length; i++)
            {
                newsKeywords += keywords[i] + " ";
            }
            return "\nNews ID: " + id + "\nTime: " + time + "\nKeywords: " + newsKeywords + "\nContent: " + content + "\nHits: " + hits + "\n*****************************\n";
        }
    }
}
