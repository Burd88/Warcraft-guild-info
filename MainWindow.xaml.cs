using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Windows;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace Warcraft
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    /// 



    public partial class MainWindow : Window
    {
        string token_battle_net;
        string token_warcraftlogs;
        string local;
        public MainWindow()
        {

            // System.Threading.Thread.Sleep(2000);
            InitializeComponent();
            local = System.Globalization.CultureInfo.CurrentCulture.ToString();
            // guild_activity.Text = local;
            autorizations_battle_net();
            //autorizations_warcraftlogs();
        }
        public async void autorizations_battle_net()
        {
            try
            {
                using (var httpClient = new HttpClient())
                {
                    using (var request = new HttpRequestMessage(new HttpMethod("POST"), "https://us.battle.net/oauth/token"))
                    {
                        var base64authorization = Convert.ToBase64String(Encoding.ASCII.GetBytes("4e4c64f795ba43e89c232ecf5f01d4aa:T0AlNaK526jhXT7cEJsNmKCkqwEEqS2A"));
                        request.Headers.TryAddWithoutValidation("Authorization", $"Basic {base64authorization}");

                        request.Content = new StringContent("grant_type=client_credentials");
                        request.Content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/x-www-form-urlencoded");

                        var response = await httpClient.SendAsync(request);
                        Token_for_api my_token = JsonConvert.DeserializeObject<Token_for_api>(response.Content.ReadAsStringAsync().Result);
                        token_battle_net = my_token.access_token;
                        Update_guild_info.IsEnabled = true;
                        Update_guild_info_func();
                        Update_guild_activity();
                        Update_warcraftlogs_data();
                        Update_realm_info();
                        var timer = new DispatcherTimer();
                        timer.Tick += new EventHandler(timer_Tick);
                        timer.Interval = new TimeSpan(0, 30, 0);
                        timer.Start();
                    }
                }
            }
            catch (WebException e)
            {
                if (e.Status == WebExceptionStatus.ProtocolError)
                {
                    MessageBox.Show("Ошибка статуса запроса:  " + e);
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("Ошибка приложения:  " + e);
            }
        }

        public async void autorizations_warcraftlogs()
        {
            using (var httpClient = new HttpClient())
            {
                using (var request = new HttpRequestMessage(new HttpMethod("POST"), "https://ru.warcraftlogs.com/oauth/token"))
                {
                    var base64authorization = Convert.ToBase64String(Encoding.ASCII.GetBytes("c2c9093c70e642ac6ec003d4b0904c33:929fe6c5-8428-47b1-a1cd-33ad96cf0b58"));
                    request.Headers.TryAddWithoutValidation("Authorization", $"Basic {base64authorization}");

                    request.Content = new StringContent("grant_type=client_credentials");
                    request.Content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/x-www-form-urlencoded");

                    var response = await httpClient.SendAsync(request);
                    Token_for_api my_token = JsonConvert.DeserializeObject<Token_for_api>(response.Content.ReadAsStringAsync().Result);
                    token_warcraftlogs = my_token.access_token;
                    Update_guild_info.IsEnabled = true;
                    Update_guild_info_func();
                    Update_guild_activity();



                }
            }
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            Update_guild_info_func();
            Update_guild_activity();
            Update_warcraftlogs_data();
            Update_realm_info();
        }
        private void Real_info_click(object sender, RoutedEventArgs e)
        {
            Update_realm_info();

        }
        public static DateTime FromUnixTimeStampToDateTime(string unixTimeStamp)
        {

            return DateTimeOffset.FromUnixTimeSeconds(long.Parse(unixTimeStamp) / 1000).LocalDateTime;
        }
        string lvl = null;
        private void Character_info(string name)
        {

            try
            {
                WebRequest requestchar = WebRequest.Create("https://eu.api.blizzard.com/profile/wow/character/howling-fjord/" + name.ToLower() + "?namespace=profile-eu&locale=ru_RU&access_token=" + token_battle_net);

                WebResponse responcechar = requestchar.GetResponse();

                using (Stream stream1 = responcechar.GetResponseStream())

                {
                    using (StreamReader reader1 = new StreamReader(stream1))
                    {
                        string line = "";
                        while ((line = reader1.ReadLine()) != null)
                        {



                            Root_charackter_full_info character = JsonConvert.DeserializeObject<Root_charackter_full_info>(line);
                            lvl = character.equipped_item_level.ToString();
                        }
                    }
                }
                responcechar.Close();
            }
            catch (WebException e)
            {
                if (e.Status == WebExceptionStatus.ProtocolError)
                {
                    Console.WriteLine("Status Code : {0}", ((HttpWebResponse)e.Response).StatusCode);
                    Console.WriteLine("Status Description : {0}", ((HttpWebResponse)e.Response).StatusDescription);
                    lvl = "error";
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                lvl = "error";
            }








        }
        private void Update_guild_info_func()
        {
            try
            {
                WebRequest request = WebRequest.Create("https://eu.api.blizzard.com/data/wow/guild/howling-fjord/%D1%81%D0%B5%D1%80%D0%B4%D1%86%D0%B5-%D0%B3%D1%80%D0%B5%D1%85%D0%B0/roster?namespace=profile-eu&locale=ru_RU&access_token=" + token_battle_net);
                WebResponse responce = request.GetResponse();

                using (Stream stream = responce.GetResponseStream())

                {
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        string line = "";
                        while ((line = reader.ReadLine()) != null)
                        {
                            int index = 0;

                            List<User> users = new List<User>();
                            Root_guild guild = JsonConvert.DeserializeObject<Root_guild>(line);
                            foreach (Member_guild character in guild.members)
                            {
                                if (character.rank == 0)
                                {
                                    guild_master_label.Content = character.character.name;
                                }
                                Character_info(character.character.name);
                                Uri uri = new Uri(@Environment.CurrentDirectory + "/Resources/class/" + character.character.playable_class.id.ToString() + ".jpg");
                                BitmapImage bmp = new System.Windows.Media.Imaging.BitmapImage(uri);

                                users.Add(new User() { Name = character.character.name, Level = character.character.level.ToString(), Class = bmp, Rank = character.rank.ToString(), Ilvl = lvl });
                                index++;

                            }
                            guild_members_count.Content = index;
                            guild_data_members.ItemsSource = users;
                            guild_name_label.Content = guild.guild.name;
                            ICollectionView cvTasks = CollectionViewSource.GetDefaultView(guild_data_members.ItemsSource);
                            if (cvTasks != null && cvTasks.CanSort == true)
                            {
                                cvTasks.SortDescriptions.Clear();
                                cvTasks.SortDescriptions.Add(new SortDescription("Rank", ListSortDirection.Ascending));

                            }
                        }
                    }
                }
                responce.Close();
            }
            catch (WebException e)
            {
                if (e.Status == WebExceptionStatus.ProtocolError)
                {
                    MessageBox.Show("Ошибка статуса запроса:  " + e);
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("Ошибка приложения:  " + e);
            }
        }
        private void Update_guild_activity()
        {
            try
            {

                WebRequest requesta = WebRequest.Create("https://eu.api.blizzard.com/data/wow/guild/howling-fjord/%D1%81%D0%B5%D1%80%D0%B4%D1%86%D0%B5-%D0%B3%D1%80%D0%B5%D1%85%D0%B0/activity?namespace=profile-eu&locale=ru_RU&access_token=" + token_battle_net);
                WebResponse responcea = requesta.GetResponse();

                using (Stream stream2 = responcea.GetResponseStream())

                {
                    using (StreamReader reader2 = new StreamReader(stream2))
                    {
                        string line = "";
                        while ((line = reader2.ReadLine()) != null)
                        {



                            Activity_all activity = JsonConvert.DeserializeObject<Activity_all>(line);



                            for (int i = 0; i < activity.activities.Count; i++)
                            {
                                if (activity.activities[i].character_achievement != null)
                                {
                                    guild_activity.AppendText("-----------------------" + "\n");
                                    guild_activity.AppendText("Имя персонажа : " + activity.activities[i].character_achievement.character.name + "\n");

                                    guild_activity.AppendText("Активность : " + activity.activities[i].character_achievement.achievement.name + "\n");

                                    guild_activity.AppendText("Дата : " + FromUnixTimeStampToDateTime(activity.activities[i].timestamp).ToString() + "\n" + "-----------------------" + "\n");

                                }
                                else if (activity.activities[i].encounter_completed != null)
                                {
                                    guild_activity.AppendText("-----------------------" + "\n");
                                    guild_activity.AppendText("Активность гильдии : " + activity.activities[i].encounter_completed.encounter.name + "\n");

                                    guild_activity.AppendText(activity.activities[i].encounter_completed.mode.name + "\n");
                                    guild_activity.AppendText("Время : " + FromUnixTimeStampToDateTime(activity.activities[i].timestamp).ToString() + "\n" + "-----------------------" + "\n");
                                }





                            }


                        }
                    }
                }
                responcea.Close();
            }
            catch (WebException e)
            {
                if (e.Status == WebExceptionStatus.ProtocolError)
                {
                    MessageBox.Show("Ошибка статуса запроса:  " + e);
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("Ошибка приложения:  " + e);
            }
        }
        private void Update_realm_info()
        {
            try
            {
                WebRequest request = WebRequest.Create("https://eu.api.blizzard.com/data/wow/connected-realm/1615?namespace=dynamic-eu&locale=ru_RU&access_token=" + token_battle_net);
                WebResponse responce = request.GetResponse();

                using (Stream stream = responce.GetResponseStream())

                {
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        string line = "";
                        while ((line = reader.ReadLine()) != null)
                        {
                            Root_realm realm = JsonConvert.DeserializeObject<Root_realm>(line);
                            foreach (Realm_realm r in realm.realms)
                            {
                                realm_name_label.Content = r.name;
                                region_label.Content = r.region.name;
                                time_zone_label.Content = r.timezone;
                                language_label.Content = r.category;

                            }

                            status_label.Content = realm.status.name;
                        }
                    }
                }
                responce.Close();
            }
            catch (WebException e)
            {
                if (e.Status == WebExceptionStatus.ProtocolError)
                {
                    MessageBox.Show("Ошибка статуса запроса:  " + e);
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("Ошибка приложения:  " + e);
            }
        }
        private void Update_warcraftlogs_data()
        {

            try
            {

                WebRequest request = WebRequest.Create("https://www.warcraftlogs.com/v1/reports/guild/%D0%A1%D0%B5%D1%80%D0%B4%D1%86%D0%B5%20%D0%B3%D1%80%D0%B5%D1%85%D0%B0/howling-fjord/eu?api_key=c2c9093c70e642ac6ec003d4b0904c33");
                WebResponse responce = request.GetResponse();

                using (Stream stream = responce.GetResponseStream())

                {
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        string line = reader.ReadToEnd();



                        line = "{ \"logs\": " + line + "}";

                        List<Logs> logs_list = new List<Logs>();
                        Warcraftlogs logs = JsonConvert.DeserializeObject<Warcraftlogs>(line);


                        foreach (Logs_all log in logs.logs)
                        {




                            logs_list.Add(new Logs() { Dungeon = log.title, Date_start = FromUnixTimeStampToDateTime(log.start).ToString(), Date_end = FromUnixTimeStampToDateTime(log.end).ToString(), Downloader = log.owner, Link = "https://ru.warcraftlogs.com/reports/" + log.id });

                        }


                        logs_datagrid.ItemsSource = logs_list;



                    }
                }
                responce.Close();
            }
            catch (WebException e)
            {
                if (e.Status == WebExceptionStatus.ProtocolError)
                {
                    MessageBox.Show("Ошибка статуса запроса:  " + e);
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("Ошибка приложения:  " + e);
            }
        }
        private void DG_Hyperlink_Click(object sender, RoutedEventArgs e)
        {
            Hyperlink link = (Hyperlink)e.OriginalSource;
            Process.Start(link.NavigateUri.AbsoluteUri);
        }



        private void Update_guild_info_Click(object sender, RoutedEventArgs e)
        {
            Update_guild_info_func();
            Update_guild_activity();

        }


    }
}
