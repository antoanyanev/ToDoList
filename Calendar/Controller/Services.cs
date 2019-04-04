/// <summary>
using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Net;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using Calendar.Models.DataModels;

namespace Calendar.Controller {
    /// This is the Services controller class.
    /// </summary>
    /// <remarks>
    /// Use it to retrieve DB and API data.
    /// </remarks>
    public static class Services {
        #region Variables

        /// <summary>
        /// API request URL
        /// </summary>
        private static string initialURL = "http://api.openweathermap.org/data/2.5/weather?q=city&appid=3d5632822352c9cd93370a8212356d3f";

        #endregion
        #region Methods

        /// <summary>
        /// Generates a greeting message based on the time of day
        /// </summary>
        /// <param name="time">Current time of the day</param>
        /// <returns>String Message</returns>
        /// <seealso cref="GetTime()">
        /// Time input
        /// </seealso>
        public static string GenerateGreeting(string time) {
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

        /// <summary>
        /// Returns the current time
        /// </summary>
        /// <returns>Time string HH:mm</returns>
        public static string GetTime() {
            return DateTime.Now.ToString("HH:mm");
        }

        /// <summary>
        /// Gets the current temperature for a city
        /// </summary>
        /// <param name="city">City</param>
        /// <returns>Temperature in Celsius</returns>
        public static int GetWeather(string city) {
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

            // Check if the request with the city is valid
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

        /// <summary>
        /// Changes the date format from dd/mm/yyy to yyyy-mm-dd
        /// </summary>
        /// <param name="input">Input date string</param>
        /// <returns>Reformatted date string</returns>
        public static string ReformatDate(string input) {
            return String.Join("-", input.Split('/').Reverse().ToArray());
        }

        /// <summary>
        /// Checks if the birthdate is in the correct format
        /// </summary>
        /// <param name="input">Date string</param>
        /// <param name="labels">Labels collection</param>
        /// <returns>True or False</returns>
        public static bool CheckDate(string input, List<Label> labels) {
            bool ok = true;
            Regex rx = new Regex(@"([12]\d{3}-(0[1-9]|1[0-2])-(0[1-9]|[12]\d|3[01]))");
            MatchCollection matches = rx.Matches(input);

            if (matches.Count != 1) {
                labels.Where(x => x.Name == "labelError").First().Text = "Invalid Date!";
                ok = false;
            }

            return ok;
        }

        /// <summary>
        /// Checks if any fields are left empty
        /// </summary>
        /// <param name="input">User input collection</param>
        /// <param name="labels">Labels collection</param>
        /// <returns>True or False</returns>
        public static bool CheckNull(List<string> input, List<Label> labels) {
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

        /// <summary>
        /// Checks if there is a registered user
        /// </summary>
        /// <returns>True or False</returns>
        public static bool CheckLogin() {
            var context = new UserEntity();

            if (!context.Users.Any()) {
                return false;
            }
            else {
                return true;
            }
        }

        /// <summary>
        /// Adds a user registration
        /// </summary>
        /// <param name="info">Info of the user</param>
        public static void CreateUser(List<string> info) {
            var context = new UserEntity();
            var user = new User() {
                Name = info[0],
                Surname = info[1],
                Birthdate = ReformatDate(info[2]),
                Gender = info[3],
                City = info[4]
            };

            context.Users.Add(user);
            context.SaveChanges();
        }

        /// <summary>
        /// Gets the user's info
        /// </summary>
        /// <returns>User</returns>
        public static User GetUser() {
            var context = new UserEntity();
            return context.Users.FirstOrDefault<User>();
        }

        /// <summary>
        /// Adds a task to the Task table
        /// </summary>
        /// <param name="content">Content of the task</param>
        public static void AddTask(string content) {
            var context = new TaskEntity();

            if (content != String.Empty) {

                var task = new Task() {
                    Content = content
                };

                context.Tasks.Add(task);
                context.SaveChanges();
            }
        }

        /// <summary>
        /// Returns all tasks
        /// </summary>
        /// <returns>A list of all tasks</returns>
        public static List<Task> GetAllTasks() {
            var context = new TaskEntity();
            return context.Tasks.ToList();
        }

        /// <summary>
        /// Deletes all the tasks from the database
        /// </summary>
        public static void DeleteAllTasks() {
            var context = new TaskEntity();
            context.Tasks.RemoveRange(context.Tasks);
            context.SaveChanges();
        }

        /// <summary>
        /// Deletes a task from the database
        /// </summary>
        /// <param name="content">Task content</param>
        public static void DeleteTask(string content) {
            var context = new TaskEntity();
            context.Tasks.Remove(context.Tasks.Single(t => t.Content == content));
            context.SaveChanges();
        }

        #endregion
    }
}
