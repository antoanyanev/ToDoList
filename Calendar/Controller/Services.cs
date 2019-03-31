using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;
using System.Net;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace Calendar.Controller {
    public static class Services {
        private static string initialURL = "http://api.openweathermap.org/data/2.5/weather?q=city&appid=3d5632822352c9cd93370a8212356d3f";

        public static string GetConnectionString(string file) {
            // Returns the DB connection string stored in the Connecton.json file
            Connection con; 
            using (StreamReader r = new StreamReader(file)) {
                string json = r.ReadToEnd();
                con = JsonConvert.DeserializeObject<Connection>(json);
            }

            return con.ConnectionString;
        }

        public static string GenerateGreeting() {
            // Generates a greeting message based on the time of day
            string time = GetTime();

            int hour = int.Parse(time.Split(':')[0]);
            string message = "";

            if (hour >= 0 && hour <= 12) {
                message = "Good morning, ";
            }
            else if (hour > 12 && hour <= 19) {
                message = "Hello, ";
            }
            else if (hour > 19 && hour < 24) {
                message = "Good evening, ";
            }

            return message;
        }

        public static string GetTime() {
            return DateTime.Now.ToString("HH:mm");
        }

        public static int GetWeather(string city) {
            // Gets the weather based on the city parameter
            // Executes a HTTP GEt request to an API (www.openweathermap.org)
            // returns the temperature value as an integer

            // Add the city parameter to the request URL

            int temp;
            int index = initialURL.IndexOf("city");
            string url1 = initialURL.Substring(0, index);
            string url2 = initialURL.Substring(index + 4);
            string url = url1 + city + url2;

            // Initialize the request with the URL

            var webRequest = WebRequest.Create(url) as HttpWebRequest;
            webRequest.ContentType = "application/json";
            webRequest.UserAgent = "Nothing";

            // Check if the request wit hthe city is valid
            // Otherwise, use the default value of 0

            try {
                using (var s = webRequest.GetResponse().GetResponseStream()) {
                    using (var sr = new StreamReader(s)) {
                        string result = sr.ReadToEnd();
                        index = result.IndexOf("temp");
                        temp = int.Parse(result.Substring(index + 6, 3));
                        temp -= 273; // kelvin to degrees centigrade conversion
                    }
                }
            }
            catch (Exception e) {
                temp = 0;
            }

            return temp;
        }

        public static string ReformatDate(string input) {
            // Reformats the default DateTime format from yyyy-mm-dd to dd/mm/yyyy

            return String.Join("-", input.Split('/').Reverse().ToArray());
        }

        public static bool CheckDate(string input, List<Label> labels) {
            // Checks if the birthdate is in the correct format

            bool ok = true;
            Regex rx = new Regex(@"([12]\d{3}-(0[1-9]|1[0-2])-(0[1-9]|[12]\d|3[01]))");

            MatchCollection matches = rx.Matches(input);

            if (matches.Count != 1) {
                labels.Where(x => x.Name == "labelError").First().Text = "Invalid Date!";
                ok = false;
            }

            return ok;
        }

        public static bool CheckNull(List<string> input, List<Label> labels) {
            // Checks if any fields are left empty

            bool ok = true;

            foreach (string str in input) {
                if (str == String.Empty) {
                    ok = false;
                    labels.Where(x => x.Name == "labelError").First().Text = "No empty fields!";
                    break;
                }
            }

            return ok;
        }
    }
}
