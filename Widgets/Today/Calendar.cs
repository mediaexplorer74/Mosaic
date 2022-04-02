using System;
using System.Collections;
using System.ComponentModel;
//using System.Management;
//using Google.Apis.Calendar.v3;
using Google.GData.Client;
using Google.GData.Extensions;
//using Google.GData.Calendar;

namespace Today
{
    /// <summary>
    /// Summary description for Calendar.
    /// </summary>
    public class Calendar
    {
        public CalendarData GetFeedOfToday(String feedUrl, String user, String pass)
        {
            /*
            var myService = new CalendarService("Mosaic_Today");
            myService.setUserCredentials(user, pass);

            var myQuery = new EventQuery(feedUrl);
            myQuery.StartTime = DateTime.Now;
            myQuery.EndTime = DateTime.Today.AddDays(2);    // we search two days after

            try
            {
                EventFeed calFeed = myService.Query(myQuery);   // запрос на получение "новостной ленты" календаря


                // now populate the calendar
                if (calFeed.Entries.Count > 0)
                {
                    var entry = (EventEntry)calFeed.Entries[0];
                    var result = new CalendarData();

                    // Title
                    result.Title = entry.Title.Text;

                    if (entry.Locations.Count > 0 && !string.IsNullOrEmpty(entry.Locations[0].ValueString))
                        result.Location = entry.Locations[0].ValueString;
                    if (entry.Content != null)
                        result.Description = entry.Content.Content;

                    if (entry.Times.Count > 0)
                    {
                        result.BeginTime = entry.Times[0].StartTime;
                        result.EndTime = entry.Times[0].EndTime;
                    }

                    return result;
                }
            }
            catch
            {
                // "Возникло исключение -- ошибка авторизации
                Console.WriteLine("Возникло исключение!");
            }
            finally
            {
                //
            }
            */

            return null;
        }

    }
}