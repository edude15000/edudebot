﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using Newtonsoft.Json.Linq;
using System.Linq;
using Newtonsoft.Json;
using System.Threading;

public class Utils
{
    public static String version { get; set; } = "2.12.1"; // UPDATE AS NECESSARY
    public static String releaseDate = "7/17/2017"; // UPDATE AS NECESSARY
    public static String twitchClientID = "c203ik5st5i3kde6amsageei2snaj1v";
    public static String botMaker = "edude15000";
    public static String currentSongFile = @"bin\currentsong.txt";
    public static String currentRequesterFile = @"bin\currentrequester.txt";
    public static String userDataFile = @"bin\userData.json";
    public static String songListFile = @"bin\songList.json";
    public static String readmeFile = "README.txt";
    public static String notesFile = @"bin\notes.txt";
    public static String songListTextFile = @"bin\song.txt";
    public static String googleApiKey = "AIzaSyDU4bPym2G64rrPgk7B9a5L6LWtIyLhFQg";
    public static String customsForgeAuthKey = "880ea6a14ea49e853634fbdc5015a024";

    public static int getDurationOfVideoInSeconds(String time)
    {
        TimeSpan youTubeDuration = System.Xml.XmlConvert.ToTimeSpan(time);
        return (youTubeDuration.Hours * 3600) + (youTubeDuration.Minutes * 60) + youTubeDuration.Seconds;
    }
    
    public static List<Song> loadSongs()
    {
        try
        {
            StreamReader r = new StreamReader(songListFile);
            string json = r.ReadToEnd();
            List<Song> list = JsonConvert.DeserializeObject<List<Song>>(json);
            if (list == null)
            {
                list = new List<Song>();
            }
            r.Close();
            return list;
        }
        catch (Exception e)
        {
            errorReport(e);
            Debug.WriteLine(e.ToString());
        }
        return new List<Song>();
    }

    public static void saveSongs(List<Song> songs)
    {
        try
        {
            string json = JsonConvert.SerializeObject(songs, Formatting.Indented);
            StreamWriter sw = new StreamWriter(songListFile);
            sw.Write(json);
            sw.Close();
        }
        catch (Exception e)
        {
            Thread.Sleep(1000);
            try
            {
                string json = JsonConvert.SerializeObject(songs, Formatting.Indented);
                StreamWriter sw = new StreamWriter(songListFile);
                sw.Write(json);
                sw.Close();
            }
            catch (Exception)
            {
                errorReport(e);
                Debug.WriteLine(e.ToString());
            }
        }
    }

    public static TwitchBot loadData()
    {
        if (!File.Exists(userDataFile))
        {
            return null;
        }
        try
        {
            StreamReader r = new StreamReader(userDataFile);
            string json = r.ReadToEnd();
            r.Close();
            return JsonConvert.DeserializeObject<TwitchBot>(json);
        }
        catch (Exception e)
        {
            errorReport(e);
            Debug.WriteLine(e.ToString());
        }
        return null;
    }

    public static void saveData(TwitchBot twitchBot)
    {
        try
        {
            string json = JsonConvert.SerializeObject(twitchBot, Formatting.Indented);
            StreamWriter sw = new StreamWriter(userDataFile);
            sw.Write(json);
            sw.Close();
            saveSongs(twitchBot.requestSystem.songList);
        }
        catch (Exception)
        {
            Thread.Sleep(1000);
            try
            {
                string json = JsonConvert.SerializeObject(twitchBot, Formatting.Indented);
                StreamWriter sw = new StreamWriter(userDataFile);
                sw.Write(json);
                sw.Close();
                saveSongs(twitchBot.requestSystem.songList);
            }
            catch (Exception e)
            {
                errorReport(e);
                Debug.WriteLine(e.ToString());
            }
        }
    }

    public static void copyFile(String f1, String f2)
    {
        try
        {
            File.Copy(f1, f2);
        }
        catch (IOException e)
        {
            Utils.errorReport(e);
            Debug.WriteLine(e.ToString());
        }
    }

    public static void errorReport(Exception e)
    {
        try
        {
            StreamWriter output = new StreamWriter(@"bin\errorlog.txt", true);
            output.Write(getDate() + " " + getTime() + " - DudeBot " + version + " Error : " + e + "\r\r\r");
            output.Close();
        }
        catch (IOException e1)
        {
            Debug.WriteLine(e1.ToString());
        }
    }

    public static String getDate()
    {
        return DateTime.Now.ToString("MM/dd/yyyy");
    }

    public static String getTime()
    {
        return DateTime.Now.ToString("HH:mm:ss");
    }
    
    public static Boolean isInteger(String s)
    {
        if (s.Contains("-") || s.Contains(" "))
        {
            return false;
        }
        try
        {
            Int32.Parse(s);
        }
        catch (Exception)
        {
            return false;
        }
        return true;
    }

    public static Boolean isDouble(String s)
    {
        if (s.Contains("-") || s.Contains(" "))
        {
            return false;
        }
        try
        {
            Double.Parse(s);
        }
        catch (Exception)
        {
            return false;
        }
        return true;
    }

    public static double closest(double of, List<Double> i)
    {
        double min = Int32.MaxValue;
        double closest = of;
        foreach (double v in i)
        {
            double diff = Math.Abs(v - of);
            if (diff < min)
            {
                min = diff;
                closest = v;
            }
        }
        return closest;
    }

    public static String timeConversion(int totalSeconds)
    {
        int MINUTES_IN_AN_HOUR = 60;
        int SECONDS_IN_A_MINUTE = 60;
        int seconds = totalSeconds % SECONDS_IN_A_MINUTE;
        int totalMinutes = totalSeconds / SECONDS_IN_A_MINUTE;
        int minutes = totalMinutes % MINUTES_IN_AN_HOUR;
        int hours = totalMinutes / MINUTES_IN_AN_HOUR;
        if (hours == 0 && minutes == 0)
        {
            if (seconds == 1)
            {
                return seconds + " second";
            }
            else
            {
                return seconds + " seconds";
            }
        }
        else if (hours == 0)
        {
            if (minutes == 1 && seconds == 1)
            {
                return minutes + " minute and " + seconds + " second";
            }
            else if (minutes == 1)
            {
                return minutes + " minute and " + seconds + " seconds";
            }
            else if (seconds == 1)
            {
                return minutes + " minutes and " + seconds + " second";
            }
            else
            {
                return minutes + " minutes and " + seconds + " seconds";
            }
        }
        else
        {
            if (hours == 1 && minutes == 1 && seconds == 1)
            {
                return hours + " hour, " + minutes + " minute, and " + seconds + " second";
            }
            else if (hours == 1 && minutes == 1)
            {
                return hours + " hour, " + minutes + " minute, and " + seconds + " seconds";
            }
            else if (hours == 1 && seconds == 1)
            {
                return hours + " hour, " + minutes + " minutes, and " + seconds + " second";
            }
            else if (minutes == 1 && seconds == 1)
            {
                return hours + " hours, " + minutes + " minute, and " + seconds + " second";
            }
            else if (hours == 1)
            {
                return hours + " hour, " + minutes + " minutes, and " + seconds + " seconds";
            }
            else if (minutes == 1)
            {
                return hours + " hours, " + minutes + " minute, and " + seconds + " seconds";
            }
            else if (seconds == 1)
            {
                return hours + " hours, " + minutes + " minutes, and " + seconds + " second";
            }
            else
            {
                return hours + " hours, " + minutes + " minutes, and " + seconds + " seconds";
            }
        }
    }

    public static String timeConversionYears(long inputSeconds)
    {
        long days = (inputSeconds / (3600 * 24));
        if (days < 1)
        {
            return "less than 1 day";
        }
        else if (days < 30)
        {
            return days + " days";
        }
        else
        {
            int daysInt = (int)Math.Floor((decimal)days);
            int months = daysInt / 30;
            if (daysInt < 365)
            {
                int remDays = daysInt % 30;
                if (remDays == 0)
                {
                    if (months == 1)
                    {
                        return months + " month";
                    }
                    else
                    {
                        return months + " months";
                    }
                }
                else
                {
                    if (months == 1 && remDays == 1)
                    {
                        return months + " month and " + remDays + " day";
                    }
                    else if (months == 1)
                    {
                        return months + " month and " + remDays + " days";
                    }
                    else if (remDays == 1)
                    {
                        return months + " months and " + remDays + " day";
                    }
                    else
                    {
                        return months + " months and " + remDays + " days";
                    }
                }
            }
            else
            {
                int years = months / 12;
                int remMonths = months % 12;
                if (remMonths == 0)
                {
                    if (years == 1)
                    {
                        return years + " year";
                    }
                    else
                    {
                        return years + " years";
                    }
                }
                else
                {
                    if (years == 1 && remMonths == 1)
                    {
                        return years + " year and " + remMonths + " month";
                    }
                    else if (years == 1)
                    {
                        return years + " year and " + remMonths + " months";
                    }
                    else if (remMonths == 1)
                    {
                        return years + " years and " + remMonths + " month";
                    }
                    else
                    {
                        return years + " years and " + remMonths + " months";
                    }
                }
            }
        }
    }

    public static int getRandomNumber(int x)
    {
        Random rand = new Random();
        return rand.Next(x);
    }

    public static Boolean checkIfUserIsOP(String user, String channel, String streamer, List<BotUser> users)
    {
        foreach (BotUser u in users)
        {
            if (u.username.Equals(user, StringComparison.InvariantCultureIgnoreCase))
            {
                if (u.mod == true)
                {
                    return true;
                }
                else
                {
                    String info = callURL("http://tmi.twitch.tv/group/user/" + streamer + "/chatters");
                    try
                    {
                        String info2 = info.Substring(info.IndexOf('['), info.IndexOf(']'));
                        if (info2.Contains(user))
                        {
                            u.mod = true;
                            return true;
                        }
                    }
                    catch (Exception)
                    {
                        return false;
                    }
                }
            }
        }
        return false;
    }
    
    public static String callURL(String uri)
    {
        try
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);
            if (uri.ToLower().Contains("kraken"))
            {
                request.Headers.Add("Client-ID", twitchClientID);
            }
            request.AutomaticDecompression = DecompressionMethods.GZip;

            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            using (Stream stream = response.GetResponseStream())
            using (StreamReader reader = new StreamReader(stream))
            {
                return reader.ReadToEnd();
            }
        }
        catch (Exception e1)
        {
            errorReport(e1);
            Debug.WriteLine(e1.ToString());
            return "";
        }
    }

    public static List<String> getAllViewers(String streamer)
    {
        List<String> users = new List<String>();
        String info = callURL("http://tmi.twitch.tv/group/user/" + streamer + "/chatters");
        try
        {
            var a = JsonConvert.DeserializeObject<dynamic>(info);
            a = a["chatters"];
            JArray viewers = a["viewers"];
            JArray staff = a["staff"];
            JArray admins = a["admins"];
            JArray global_mods = a["global_mods"];
            JArray moderators = a["moderators"];
            if (moderators != null)
            {
                int len = moderators.Count;
                String[] items = moderators.Select(jv => (String)jv).ToArray();
                for (int i = 0; i < len; i++)
                {
                    if (!users.Contains(items[i]))
                    {
                        users.Add(items[i].ToString());
                    }
                }
            }
            if (viewers != null)
            {
                int len = viewers.Count;
                String[] items = viewers.Select(jv => (String)jv).ToArray();
                for (int i = 0; i < len; i++)
                {
                    if (!users.Contains(items[i]))
                    {
                        users.Add(items[i].ToString());
                    }
                }
            }
            if (staff != null)
            {
                int len = staff.Count;
                String[] items = staff.Select(jv => (String)jv).ToArray();
                for (int i = 0; i < len; i++)
                {
                    if (!users.Contains(items[i]))
                    {
                        users.Add(items[i].ToString());
                    }
                }
            }
            if (admins != null)
            {
                int len = admins.Count;
                String[] items = admins.Select(jv => (String)jv).ToArray();
                for (int i = 0; i < len; i++)
                {
                    if (!users.Contains(items[i]))
                    {
                        users.Add(items[i].ToString());
                    }
                }
            }
            if (global_mods != null)
            {
                int len = global_mods.Count;
                String[] items = global_mods.Select(jv => (String)jv).ToArray();
                for (int i = 0; i < len; i++)
                {
                    if (!users.Contains(items[i]))
                    {
                        users.Add(items[i].ToString());
                    }
                }
            }
        }
        catch (Exception e1)
        {
            errorReport(e1);
            Debug.WriteLine(e1.ToString());
        }
        return users;
    }

    public static String getFollowingText(String message)
    {
        return message.Substring(message.IndexOf(" ") + 1);
    }
}