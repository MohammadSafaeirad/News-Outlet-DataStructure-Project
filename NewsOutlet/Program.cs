using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;


namespace NewsOutlet
{
    class Program
    {
        private static long currentTime = DateTimeOffset.Now.ToUnixTimeSeconds();
        private static long timePeriod = currentTime - (24 * 60 * 60);

        static void Main(string[] args)
        {
            Console.WriteLine("Welcome to News Outlet application");
            menu();
        }

        public static void menu()
        {
            int curIndex = -1;
            int option;
            do
            {
                Console.WriteLine("\nPlease enter one of the options below: ");
                Console.WriteLine("\t1- Show recent");
                Console.WriteLine("\t2- Show trending");
                Console.WriteLine("\t3- Select by ID");
                Console.WriteLine("\t4- Back");
                Console.WriteLine("\t5- Set time");
                Console.WriteLine("\t6- EXIT");
                Console.Write("Enter your option: ");
                option = Convert.ToInt32(Console.ReadLine());
                
                switch (option)
                {
                    case 1:
                        Console.WriteLine("\n\t1- Show recent news based on time");
                        Console.WriteLine("\t2- Show recent news based on keywords");
                        Console.Write("Enter your option: ");
                        int optRecent = Convert.ToInt16(Console.ReadLine());
                        if (optRecent == 1)
                        {
                            int maxElements = 0;
                            PriorityQueue<News> recentNews = showRecentNewsByTimePriorityQueue(readFromJsonStack()); // O(n log n)
                            while (recentNews.Count > 0)
                            {
                                Console.WriteLine(recentNews.Dequeue().ToString());
                                maxElements++;
                                if (maxElements >= 500)
                                {
                                    break;
                                }
                            } // o(n = 500)
                        }
                        else if (optRecent == 2)
                        {
                            Console.Write("Enter the keyword you want to search for: ");
                            string keyword = Console.ReadLine();
                            PriorityQueue<News> recentNews = showRecentNewsByTimeStack(readFromJsonStack());
                            int maxElements = 0;
                            while (recentNews.Count > 0)
                            {
                                News news = recentNews.Dequeue();
                                if (news.Keywords.Contains(keyword))
                                {
                                    Console.WriteLine(recentNews.Dequeue().ToString());
                                    maxElements++;
                                    if (maxElements >= 500)
                                    {
                                        break;
                                    }
                                }

                            }
                        }
                            break;
                    case 2:
                        Console.WriteLine("\n\t1- Show trending news based on time");
                        Console.WriteLine("\t2- Show trending news based on keywords");
                        Console.Write("Enter your option: ");
                        int optTrending = Convert.ToInt16(Console.ReadLine());
                        if (optTrending == 1)
                        {                            
                            PriorityQueue<News> trendingNews = showTrendingNewsByTimePriorityQueue(readFromJsonQueue());
                            int maxElements = 0;
                            while (trendingNews.Count > 0)
                            {
                                Console.WriteLine(trendingNews.Dequeue().ToString());
                                maxElements++;
                                if (maxElements >= 500)
                                {
                                    break;
                                }
                            }
                        }
                        else if (optTrending == 2)
                        {
                            Console.Write("Enter the keyword you want to search for: ");
                            string keyword = Console.ReadLine();
                            Queue<News> trendingNews = showTrendingNewsByTimeQueue(readFromJsonQueue()); // O(n log n)
                            List<News> newsWithKeyword = new List<News>();
                            foreach (News news in trendingNews)
                            {
                                if (news.Keywords.Contains(keyword))
                                {
                                    newsWithKeyword.Add(news);
                                }
                            } // O(n)
                            if (newsWithKeyword.Count == 0)
                            {
                                Console.WriteLine("\nNo news found with the keyword {0}", keyword);
                            }
                            else
                            {
                                int maxElements = 0;
                                foreach (News news in newsWithKeyword)
                                {
                                    Console.WriteLine(news.ToString());
                                    maxElements++;
                                    if (maxElements >= 500)
                                    {
                                        break;
                                    }
                                }
                            }

                        }
                        else
                        {
                            Console.WriteLine("\nOption selected is wrong! Try Again");
                        }

                        break;
                    case 3:
                        Console.Write("Enter the ID of the news you want to read: ");
                        int newsID = Convert.ToInt32(Console.ReadLine());

                        News[] newsArray = readFromJsonArray();
                        curIndex = Array.FindIndex(newsArray, news => news.Id == newsID);
                        try
                        {
                            
                            Console.WriteLine("\n************* News ID {0} **************\n", newsID);
                            Console.WriteLine(newsArray[curIndex].ToString());
                            newsArray[curIndex].Hits++;

                            string updatedJson = JsonConvert.SerializeObject(newsArray, Formatting.Indented);
                            File.WriteAllText("MOCK_DATA.json", updatedJson);
                        }
                        catch
                        {
                            Console.WriteLine("The news ID {0} doesn't found!", newsID);
                        }

                        break;
                    case 4:
                        if (curIndex == -1)
                        {
                            Console.WriteLine("No news selected yet!");
                        }
                        else
                        {
                            Console.WriteLine(" -------------- The previous news -------------\n");
                            News[] newsArray_1 = readFromJsonArray();
                            Console.WriteLine(newsArray_1[--curIndex].ToString());
                        }
                        break;
                    case 5:                        
                        Console.Write("\nEnter the time to set as a default (EPHOC format): ");
                        currentTime = Convert.ToInt64(Console.ReadLine());
                        timePeriod = currentTime - (24 * 60 * 60);
                        break;
                    case 6:
                        Console.WriteLine("Thanks for using our News Outlet application. BYE!");
                        break;
                    default:
                        Console.WriteLine("Wrong option! try again");
                        break;
                }
            }
            while (option != 6);
        }

        /*
         The JsonConvert.DeserializeObject method has a time complexity of O(n), 
        where n is the size of the JSON string being deserialized. 
        Therefore, the overall time complexity of the readFromJsonArray method is also O(n),
        where n is the size of the JSON string being read from the file.
         */
        public static News[] readFromJsonArray()
        {
            News[] news;
            using (StreamReader reader = new StreamReader("MOCK_DATA.json"))
            {                
                var json = reader.ReadToEnd();
                news = JsonConvert.DeserializeObject<News[]>(json);
            }
            return news;
        }

        public static Queue<News> readFromJsonQueue()
        {
            Queue<News> news = new Queue<News>();
            using (StreamReader reader = new StreamReader("MOCK_DATA.json"))
            {
                var json = reader.ReadToEnd();
                news = JsonConvert.DeserializeObject<Queue<News>>(json);
            }
            return news;
        }

        public static Stack<News> readFromJsonStack()
        {
            Stack<News> news = new Stack<News>();
            using (StreamReader reader = new StreamReader("MOCK_DATA.json"))
            {
                var json = reader.ReadToEnd();
                news = JsonConvert.DeserializeObject<Stack<News>>(json);
            }
            return news;
        }

        /*
         The foreach loop iterates over each News object in the stack, which takes O(n) time. 
        Inside the loop, the Enqueue method is called on the priorityQueueNews object, 
        which is implemented as a binary heap priority queue. 
        The Enqueue method has a time complexity of O(log n), 
        where n is the number of items in the priority queue.
         */
        public static PriorityQueue<News> showRecentNewsByTimeStack(Stack<News> allNews)
        {
            PriorityQueue<News> priorityQueueNews = new PriorityQueue<News>((n1, n2) => n2.Time.CompareTo(n1.Time));
            foreach (News news in allNews)
            {
                if (news.Time >= timePeriod && news.Time <= currentTime)
                {
                    priorityQueueNews.Enqueue(news);
                }
            }
            return priorityQueueNews;
        }

        public static Queue<News> showTrendingNewsByTimeQueue(Queue<News> allNews)
        {
            Queue<News> trendingNewsQueue = new Queue<News>();
            foreach (News news in allNews)
            {
                if (news.Time >= timePeriod && news.Time <= currentTime)
                {
                    trendingNewsQueue.Enqueue(news);
                }
            }
            return trendingNewsQueue = new Queue<News>(trendingNewsQueue.OrderByDescending(news => news.Hits));
        }

        public static PriorityQueue<News> showRecentNewsByTimePriorityQueue(Stack<News> allNews)
        {
            PriorityQueue<News> recentNewsQueue = new PriorityQueue<News>((n1, n2) => n2.Time.CompareTo(n1.Time));
            foreach (News news in allNews)
            {
                if (news.Time >= timePeriod && news.Time <= currentTime)
                {
                    recentNewsQueue.Enqueue(news);
                }
            }
            return recentNewsQueue;

        }

        public static PriorityQueue<News> showTrendingNewsByTimePriorityQueue(Queue<News> allNews)
        {
            PriorityQueue<News> recentNewsQueue = new PriorityQueue<News>((n1, n2) => n2.Hits.CompareTo(n1.Hits));
            foreach (News news in allNews)
            {
                if (news.Time >= timePeriod && news.Time <= currentTime)
                {
                    recentNewsQueue.Enqueue(news);
                }
            }
            return recentNewsQueue;

        }

    }

}
