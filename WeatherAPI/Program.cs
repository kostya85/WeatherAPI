using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;


namespace WeatherAPI
{
    /// <summary>
    /// Класс клиента
    /// </summary>
    class Client
    {
        private WeatherInfo _weatherInfo;
        public Client()
        {
            _weatherInfo = new WeatherInfo();
        }
        static void Main(string[] args)
        {
            Client client = new Client();
            double lat, lon;
            Console.WriteLine("Введите координаты");
            Console.Write("Lat: ");
            if (!double.TryParse(Console.ReadLine(), out lat))
            {
                throw new InvalidCastException();
            }
            Console.Write("Lon: ");
            if (!double.TryParse(Console.ReadLine(), out lon))
            {
                throw new InvalidCastException();
            }

            var result = client._weatherInfo.GetWeather(lat, lon);

            Console.WriteLine($"Прогноз утренней погоды в городе с координатами: {lat}, {lon}");
            Console.WriteLine("Celsius | Date");
            foreach (var elem in result)
            {
                Console.WriteLine(elem.MorningTemperature + " | " + elem.Date.ToString("dd.MM.yyyy"));

            }
            Console.WriteLine("**********************");
            Console.WriteLine("Максимальная утренняя температура за 5 предстоящих дней: " + result.Select(x => x.MorningTemperature).Max() + " градус(ов)");
            Console.WriteLine("Средняя утренняя температура за 5 предстоящих дней: " + result.Select(x => x.MorningTemperature).Average() + " градус(ов)");
        }
    }
    /// <summary>
    /// Класс предназначен для работы с API портала openweathermap.org
    /// </summary>
    class WeatherInfo
    {
        public string Url { get; set; }
        public string AppId { get; set; }
        public WeatherInfo()
        {
            Url = "https://api.openweathermap.org/data/2.5/onecall?";
            AppId = "here your app id";
        }

        /// <summary>
        /// Метод возвращает коллекцию, содержащую 5 объектов класса WeatherModel, соответствующих 5 дням
        /// </summary>
        /// <param name="lat">Широта</param>
        /// <param name="lon">Долгота</param>
        /// <returns></returns>

        public List<WeatherModel> GetWeather(double lat, double lon)
        {
            try
            {
                string url = Url + $"lat={lat}&lon={lon}&units=metric&appid={AppId}";

                HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(url);

                HttpWebResponse httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();

                string response;

                using (StreamReader streamReader = new StreamReader(httpWebResponse.GetResponseStream()))
                {
                    response = streamReader.ReadToEnd();
                }
                var json = JObject.Parse(response);
                var data = json["daily"];
                List<WeatherModel> result = new List<WeatherModel>();
                for (int i = 0; i < 5; i++)
                {
                    if (result.Count == 0)
                    {
                        result.Add(new WeatherModel(lat, lon, DateTime.Now, (string)data[i]["temp"]["morn"]));
                    }
                    else
                    {
                        result.Add(new WeatherModel(lat, lon, result.Last<WeatherModel>().Date.AddDays(1), (string)data[i]["temp"]["morn"]));
                    }
                }

                return result;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return new List<WeatherModel>();
            }
        }
    }
    /// <summary>
    /// Модель погоды
    /// </summary>
    class WeatherModel
    {
        public WeatherModel(double lat, double lon, DateTime date, string morningTemperature)
        {
            Lat = lat;
            Lon = lon;
            Date = date;
            MorningTemperature = double.Parse(morningTemperature);
        }

        public double Lat { get; set; }
        public double Lon { get; set; }
        public DateTime Date { get; set; }
        public double MorningTemperature { get; set; }
    }
}
