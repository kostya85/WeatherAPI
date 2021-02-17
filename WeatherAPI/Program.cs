using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace WeatherAPI
{
    class Client
    {
        private WeatherInfo _weatherInfo;
        public Client()
        {
            _weatherInfo = new WeatherInfo();
        }
        static void Main(string[] args)
        {
            long a = DateTime.Now.Ticks;
            Console.WriteLine(a);
            Console.WriteLine(a.ToString().Length);
            DateTime d = new DateTime(1568977200);
            Console.WriteLine(d.ToString());
            Console.WriteLine(new Client()._weatherInfo.Result());
        }
    }
    class WeatherInfo
    {
        public string Url { get; set; }
        public string AppId{get;set; }
        public WeatherInfo()
        {
            Url = "http://api.openweathermap.org/data/2.5/weather?";
            AppId = "b465f78c20e5fc7797d3e86c60fb7842";
        }
        public string Result()
        {
            string url = $"https://api.openweathermap.org/data/2.5/forecast?q=London&cnt=3&units=metric&mode=xml&appid={AppId}";

            HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(url);

            HttpWebResponse httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();

            string response;

            using (StreamReader streamReader = new StreamReader(httpWebResponse.GetResponseStream()))
            {
                response = streamReader.ReadToEnd();
            }
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(response);
            XmlNodeList nodeList =(doc.SelectNodes("weatherdata/forecast/time/temperature"));
            foreach (XmlNode elem in nodeList)
            {
                Console.WriteLine(elem.Attributes[1].Value );
                
            }
            Console.WriteLine();
            var s = doc.GetElementsByTagName("time");
            foreach (XmlNode e in s) {
                DateTime time = DateTime.Parse(e.Attributes[1].Value.ToString());
                Console.WriteLine(time); }
            return response;
        }
        private static void PrintItem(XmlElement item, int indent = 0)
        {
            // Выводим имя самого элемента.
            // new string('\t', indent) - создает строку состоящую из indent табов.
            // Это нужно для смещения вправо.
            // Пробел справа нужен чтобы атрибуты не прилипали к имени.
            Console.Write($"{new string('\t', indent)}{item.LocalName} ");

            // Если у элемента есть атрибуты, 
            // то выводим их поочередно, каждый в квадратных скобках.
            foreach (XmlAttribute attr in item.Attributes)
            {
                Console.Write($"[{attr.InnerText}]");
            }

            // Если у элемента есть зависимые элементы, то выводим.
            foreach (var child in item.ChildNodes)
            {
                
                if (child is XmlElement node)
                {
                    if(node.InnerText=="temperature")Console.WriteLine(node.InnerText+"*******************");
                    // Если зависимый элемент тоже элемент,
                    // то переходим на новую строку 
                    // и рекурсивно вызываем метод.
                    // Следующий элемент будет смещен на один отступ вправо.
                    Console.WriteLine();
                    PrintItem(node, indent + 1);
                }

                if (child is XmlText text)
                {
                    // Если зависимый элемент текст,
                    // то выводим его через тире.
                    Console.Write($"- {text.InnerText}");
                }
            }
        }
            public WeatherResponse GetWeather(string city, int daysCount)
        {
            string url = "http://api.openweathermap.org/data/2.5/weather?q=London&units=metric&appid=[your_app_id]";

            HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(url);

            HttpWebResponse httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();

            string response;

            using (StreamReader streamReader = new StreamReader(httpWebResponse.GetResponseStream()))
            {
                response = streamReader.ReadToEnd();
            }

            WeatherResponse weatherResponse = JsonConvert.DeserializeObject<WeatherResponse>(response);
            return weatherResponse;
        }
        public WeatherResponse GetWeather(string x, string y,int daysCount)
        {
            string url = "http://api.openweathermap.org/data/2.5/weather?q=London&units=metric&appid=[your_app_id]";

            HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(url);

            HttpWebResponse httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();

            string response;

            using (StreamReader streamReader = new StreamReader(httpWebResponse.GetResponseStream()))
            {
                response = streamReader.ReadToEnd();
            }

            WeatherResponse weatherResponse = JsonConvert.DeserializeObject<WeatherResponse>(response);
            return weatherResponse;
        }
    }
    public class Response
    {
        List<WeatherResponse> list { get; set; }
    }
    public class WeatherResponse
    {
        public TemperatureInfo Main { get; set; }

        
    }
    public class TemperatureInfo
    {
        public float Temp { get; set; }
    }
}
