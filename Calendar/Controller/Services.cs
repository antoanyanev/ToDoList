/*
    This is the Services controller class.
    Use it to retrieve DB and API data.
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Net;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using Calendar.Models;

namespace Calendar.Controller {
    public static class Services {
        private static string initialURL = "http://api.openweathermap.org/data/2.5/weather?q=city&appid=3d5632822352c9cd93370a8212356d3f"; // API request url

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
            catch {
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

        public static bool CheckLogin()
        {
            // Checks if there is a registered user

            var context = new DataEntities();

            if (!context.Users.Any())
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public static void CreateUser(List<string> info)
        {
            // Adds a user registration

            var context = new DataEntities();
            var user = new User()
            {
                Name = info[0],
                Surname = info[1],
                Birthdate = Services.ReformatDate(info[2]),
                Gender = info[3],
                City = info[4]
            };

            context.Users.Add(user);
            context.SaveChanges();
        }

        public static User GetUser()
        {
            // Gets the user's info

            var context = new DataEntities();

            return context.Users.FirstOrDefault<User>();
        }

        public static void AddTask(string content)
        {
            // Adds a task to the Task table

            var context = new DataEntities1();

            if (content != String.Empty)
            {

                var task = new Task()
                {
                    Content = content
                };

                context.Tasks.Add(task);
                context.SaveChanges();
            }
        }

        public static List<Task> GetAllTasks()
        {
            // Returns a list of all tasks

            var context = new DataEntities1();

            return context.Tasks.ToList();
        }

        public static void DeleteAllTasks()
        {
            // Deletes all the tasks from the database

            var context = new DataEntities1();

            context.Tasks.RemoveRange(context.Tasks);
            context.SaveChanges();
        }

        public static void DeleteTask(string content)
        {
            // Deletes a task from the database

            var context = new DataEntities1();
            context.Tasks.Remove(context.Tasks.Single(t => t.Content == content));
            context.SaveChanges();
        }
    }
}
