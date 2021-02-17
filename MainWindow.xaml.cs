using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using System.Xml.Linq;
namespace Warcraft
{



    public partial class MainWindow : Window
    {
        string token_battle_net;

        string local;
        bool need_update = false;
        Open_char_info char_info;
        public MainWindow()
        {


            InitializeComponent();
            Char_info_grid.Visibility = Visibility.Hidden;
            update_back();
            Update();
            add_user_info(Environment.UserName, DateTime.Now.ToString());
            if (!need_update)
            {
                update_client();                   //clent
               //autorizations_battle_net(); // server
            }


        }

        #region Функции клиента на получение информации с базы данных сервера
        public void update_client()
        {
            insert_main_info();
            if (server_status == "false")
            {
                insert_guild_main_info();
                insert_tech_talent_info();
                insert_conduit_info();
                insert_guild_member();
                insert_guild_wow_logs();
                DispatcherTimer timer = new DispatcherTimer();
                timer.Tick += new EventHandler(client_update);
                timer.Interval = new TimeSpan(0, 30, 0);
                timer.Start();
            }
            else if (server_status == "true")
            {
                MessageBox.Show("Сервер обновляет информацию, попробуйте позже");
            }

        }
        private void client_update(object sender, EventArgs e)
        {
            insert_main_info();
            if (server_status == "false")
            {
                insert_guild_main_info();
                insert_guild_member();
                insert_guild_wow_logs();
                DispatcherTimer timer = new DispatcherTimer();
                timer.Tick += new EventHandler(client_update);
                timer.Interval = new TimeSpan(0, 30, 0);
                timer.Start();
            }
            else if (server_status == "true")
            {
                MessageBox.Show("Сервер обновляет информацию, попробуйте позже");
            }
        }

        private void Update_guild_info_Click(object sender, RoutedEventArgs e)
        {
            insert_main_info();
            if (server_status == "false")
            {
                insert_guild_main_info();
                insert_guild_member();
                insert_guild_wow_logs();
                DispatcherTimer timer = new DispatcherTimer();
                timer.Tick += new EventHandler(client_update);
                timer.Interval = new TimeSpan(0, 30, 0);
                timer.Start();
            }
            else if (server_status == "true")
            {
                MessageBox.Show("Сервер обновляет информацию, попробуйте позже");
            }
        }
        #endregion

        #region Функции клиента визуальные
        public void update_back()//
        {
            Random rnd = new Random();
            int randoma = rnd.Next(1, 7);
            this.Background = new ImageBrush(ToBitmapImage(Warcraft.Properties.Resources.ResourceManager.GetObject("back_" + randoma.ToString()) as Bitmap));
        }
        #endregion

        #region Функция для сервера, получение токена для получения инфы
        public async void autorizations_battle_net()
        {
            try
            {
                local = System.Globalization.CultureInfo.CurrentCulture.ToString().Replace("-", "_");
                using (HttpClient httpClient = new HttpClient())
                {
                    httpClient.DefaultRequestHeaders.ConnectionClose = true;
                    using (HttpRequestMessage request = new HttpRequestMessage(new HttpMethod("POST"), "https://eu.battle.net/oauth/token"))
                    {
                        string base64authorization = Convert.ToBase64String(Encoding.ASCII.GetBytes("4e4c64f795ba43e89c232ecf5f01d4aa:T0AlNaK526jhXT7cEJsNmKCkqwEEqS2A"));
                        request.Headers.TryAddWithoutValidation("Authorization", $"Basic {base64authorization}");

                        request.Content = new StringContent("grant_type=client_credentials");
                        request.Content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/x-www-form-urlencoded");

                        HttpResponseMessage response = await httpClient.SendAsync(request);
                        Token_for_api my_token = JsonConvert.DeserializeObject<Token_for_api>(response.Content.ReadAsStringAsync().Result);
                        token_battle_net = my_token.access_token;
                       /*
                        delete_main_Status_update();
                        add__main_info_status("true");
                        delete_guild_info();
                        delete_guild_main_info();
                        delete_guild_wow_logs();
                        Update_guild_info_func();
                        Update_guild_activity();
                        Update_warcraftlogs_data();
                        delete_main_Status_update();
                        Thread.Sleep(2000);
                        add__main_info_status("false");
                        //Update_realm_info();
                        DispatcherTimer timer = new DispatcherTimer();
                        timer.Tick += new EventHandler(timer_Tick);
                        timer.Interval = new TimeSpan(0, 60, 0);
                        timer.Start();
                       */
                        //download_icons_TT_0();
                       // download_icons_0();
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



        private void timer_Tick(object sender, EventArgs e)
        {
            autorizations_battle_net();
        }
        #endregion

        #region Вспомогательные функции
        public static DateTime FromUnixTimeStampToDateTime(string unixTimeStamp) // конверстация времени
        {

            return DateTimeOffset.FromUnixTimeSeconds(long.Parse(unixTimeStamp) / 1000).LocalDateTime;
        }

        public static BitmapImage ToBitmapImage(Bitmap bitmap)// Конвертация изображения для таблицы
        {
            using (var memory = new MemoryStream())
            {
                bitmap.Save(memory, ImageFormat.Jpeg);
                memory.Position = 0;

                var bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.StreamSource = memory;
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.EndInit();
                bitmapImage.Freeze();

                return bitmapImage;
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        internal struct Win32Point
        {
            public Int32 X;
            public Int32 Y;
        };

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool GetCursorPos(ref Win32Point pt);

        public static System.Windows.Point GetMousePosition()
        {
            Win32Point point = new Win32Point();
            GetCursorPos(ref point);
            return new System.Windows.Point(point.X, point.Y);
        }
        #endregion

        #region Получение данных с серверов
        string lvl = null;
        string spec = null;
        string coven = null;
        string coven_lvl = null;
        string coven_soul = null;
        string last_login = null;
        string raid_progress;
        string mythic_score = "0.0";
        string guild_raid_progress;
        string gm;
        private void Character_info(string name)
        {

            try
            {
                WebRequest requestchar = WebRequest.Create("https://eu.api.blizzard.com/profile/wow/character/howling-fjord/" + name.ToLower() + "?namespace=profile-eu&locale=" + local + "&access_token=" + token_battle_net);

                WebResponse responcechar = requestchar.GetResponse();

                using (Stream stream1 = responcechar.GetResponseStream())

                {
                    using (StreamReader reader1 = new StreamReader(stream1))
                    {
                        string line = "";
                        while ((line = reader1.ReadLine()) != null)
                        {



                            Root_charackter_full_info character = JsonConvert.DeserializeObject<Root_charackter_full_info>(line);
                            if (line.Contains("\"covenant_progress\":"))
                            {
                                lvl = character.equipped_item_level.ToString();
                                spec = character.active_spec.id.ToString();
                                coven = character.covenant_progress.chosen_covenant.id.ToString();
                                last_login = character.last_login_timestamp.ToString();

                                coven_lvl = character.covenant_progress.renown_level.ToString();


                                Character_info_soul(name);
                            }

                            else
                            {
                                lvl = "error";
                                spec = "0";
                                coven = "0";
                                coven_lvl = "0";
                                last_login = "0";
                                coven_soul = "нет";
                                teh_telent = "";
                                name_soul = "";
                                conduits_id = "";
                                conduits_rank = "";
                            }

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
                    spec = "0";
                    coven = "0";
                    coven_lvl = "0";
                    coven_soul = "нет";
                    last_login = "0";
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                lvl = "error";
                spec = "0";
                coven = "0";
                coven_lvl = "0";
                coven_soul = "нет";
                last_login = "0";
            }










        }
        string teh_telent;
        //List<string> teh_talants = new List<string>();
        //List<Conduits_list> conduits = new List<Conduits_list>();
        string conduits_id;
        string conduits_rank;
        string name_soul;
        private void Character_info_soul(string name)
        {

            try
            {
                WebRequest requestchar = WebRequest.Create("https://eu.api.blizzard.com/profile/wow/character/howling-fjord/" + name.ToLower() + "/soulbinds?namespace=profile-eu&locale=" + local + "&access_token=" + token_battle_net);

                WebResponse responcechar = requestchar.GetResponse();

                using (Stream stream1 = responcechar.GetResponseStream())

                {
                    using (StreamReader reader1 = new StreamReader(stream1))
                    {
                        string line = "";
                        while ((line = reader1.ReadLine()) != null)
                        {



                            Soulbinds_API souls = JsonConvert.DeserializeObject<Soulbinds_API>(line);

                            if (line.Contains("\"soulbinds\":"))
                            {
                                foreach (Soulbinds_API_Soulbind soul in souls.soulbinds)
                                {
                                    if (soul.is_active == true)
                                    {
                                        teh_telent = "";
                                        conduits_id = "";
                                        conduits_rank = "";
                                        //MessageBox.Show(soul.soulbind.name.ToString());
                                        name_soul = soul.soulbind.name.ToString();
                                        foreach (Soulbinds_API_Trait trait in soul.traits)
                                        {
                                            if (trait.trait != null)
                                            {
                                                if (teh_telent == "")
                                                {
                                                    teh_telent = trait.trait.id.ToString();
                                                }
                                                else
                                                {
                                                    teh_telent = teh_telent + "," + trait.trait.id.ToString();
                                                }

                                                //  MessageBox.Show(trait.trait.name);
                                                //teh_talants.Add(trait.trait.id.ToString());
                                            }
                                            else if (trait.conduit_socket != null)
                                            {
                                                if (trait.conduit_socket.socket != null)
                                                {
                                                    if (conduits_id == "")
                                                    {
                                                        conduits_id = trait.conduit_socket.socket.conduit.id.ToString();
                                                        conduits_rank = trait.conduit_socket.socket.rank.ToString();
                                                    }
                                                    else
                                                    {
                                                        conduits_id = conduits_id + "," + trait.conduit_socket.socket.conduit.id.ToString();
                                                        conduits_rank = conduits_rank + "," + trait.conduit_socket.socket.rank.ToString();
                                                    }
                                                    // MessageBox.Show(trait.conduit_socket.socket.conduit.name);
                                                    //conduits.Add(new Conduits_list
                                                    // { id = trait.conduit_socket.socket.conduit.id.ToString(), rank = trait.conduit_socket.socket.rank.ToString() });
                                                }
                                                else
                                                {
                                                    if (conduits_id == "")
                                                    {
                                                        conduits_id = "0";
                                                        conduits_rank = "0";
                                                    }
                                                    else
                                                    {
                                                        conduits_id = conduits_id + "," + "0";
                                                        conduits_rank = conduits_rank + "," + "0";
                                                    }
                                                    // conduits.Add(new Conduits_list
                                                    // { id = "0", rank = "0" });
                                                }

                                            }
                                        }
                                    }

                                }
                            }

                            coven_soul = "нет";


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
                    coven_soul = "нет";
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                coven_soul = "нет";
            }








        }

        private void Character_raid_progress(string name)
        {

            try
            {
                WebRequest requestchar = WebRequest.Create("https://raider.io/api/v1/characters/profile?region=eu&realm=howling-fjord&name=" + name + "&fields=mythic_plus_scores%2Craid_progression");

                WebResponse responcechar = requestchar.GetResponse();

                using (Stream stream1 = responcechar.GetResponseStream())

                {
                    using (StreamReader reader1 = new StreamReader(stream1))
                    {
                        string line = "";
                        while ((line = reader1.ReadLine()) != null)
                        {



                            RaiderIO_info character = JsonConvert.DeserializeObject<RaiderIO_info>(line);

                            raid_progress = character.raid_progression.CastleNathria.summary;
                            mythic_score = character.mythic_plus_scores.all;
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
                    // MessageBox.Show("Status Code : {0}", ((HttpWebResponse)e.Response).StatusCode.ToString());
                    raid_progress = "0/0";
                    mythic_score = "0.0";
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                //MessageBox.Show(e.Message);
                raid_progress = "0/0";
                mythic_score = "0.0";
            }
        }

        private void Guild_raid_progress()
        {

            try
            {
                WebRequest requestchar = WebRequest.Create("https://raider.io/api/v1/guilds/profile?region=eu&realm=howling-fjord&name=%D0%A1%D0%B5%D1%80%D0%B4%D1%86%D0%B5%20%D0%B3%D1%80%D0%B5%D1%85%D0%B0&fields=raid_progression%2Craid_rankings");

                WebResponse responcechar = requestchar.GetResponse();

                using (Stream stream1 = responcechar.GetResponseStream())

                {
                    using (StreamReader reader1 = new StreamReader(stream1))
                    {
                        string line = "";
                        while ((line = reader1.ReadLine()) != null)
                        {



                            RaiderIO_guild_info rio_guild = JsonConvert.DeserializeObject<RaiderIO_guild_info>(line);

                            guild_raid_progress = rio_guild.raid_progression.CastleNathria.summary;
                            //  mythic_score = character.mythic_plus_scores.all;
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

                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);

            }
        }


        private void Update_guild_info_func()
        {
            try
            {
                WebRequest request = WebRequest.Create("https://eu.api.blizzard.com/data/wow/guild/howling-fjord/%D1%81%D0%B5%D1%80%D0%B4%D1%86%D0%B5-%D0%B3%D1%80%D0%B5%D1%85%D0%B0/roster?namespace=profile-eu&locale=" + local + "&access_token=" + token_battle_net);
                WebResponse responce = request.GetResponse();

                using (Stream stream = responce.GetResponseStream())

                {
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        string line = "";
                        while ((line = reader.ReadLine()) != null)
                        {
                            int index = 0;


                            Root_guild guild = JsonConvert.DeserializeObject<Root_guild>(line);
                            foreach (Member_guild character in guild.members)
                            {
                                if (character.rank == 0)
                                {
                                    gm = character.character.name;
                                }
                                Character_info(character.character.name);
                                Character_raid_progress(character.character.name);

                                //Uri uri_spec = new Uri(Warcraft.Properties.Resources.);//@Environment.CurrentDirectory + "/Resources/spec/" + spec.ToString() + ".jpg");
                                // BitmapImage bmp_spec = ToBitmapImage(Warcraft.Properties.Resources.ResourceManager.GetObject("spec_" +spec) as Bitmap);



                                //   Uri uri_class = new Uri(@Environment.CurrentDirectory + "/Resources/class/" + character.character.playable_class.id.ToString() + ".jpg");
                                //BitmapImage bmp_class = ToBitmapImage(Warcraft.Properties.Resources.ResourceManager.GetObject("class_" + character.character.playable_class.id.ToString()) as Bitmap);//new System.Windows.Media.Imaging.BitmapImage(uri_class);
                                add_guild_info(character.character.name, character.character.level.ToString(), character.character.playable_class.id.ToString(), spec, character.rank.ToString(), lvl, raid_progress, mythic_score, coven, coven_lvl, name_soul, teh_telent, conduits_id, conduits_rank, last_login);
                                //users.Add(new User() { Name = character.character.name, Level = character.character.level.ToString(), Class = bmp_class, Spec = bmp_spec, Rank = character.rank.ToString(), Ilvl = lvl , Raid_progress = raid_progress, MythicScore = mythic_score });
                                index++;

                            }
                            Guild_raid_progress();
                            add_guild_main_info(guild.guild.name, gm, guild_raid_progress, DateTime.Now.ToString());
                            // guild_members_count.Content = index;
                            // guild_data_members.ItemsSource = users;
                            // guild_name_label.Content = guild.guild.name;
                            //ICollectionView cvTasks = CollectionViewSource.GetDefaultView(guild_data_members.ItemsSource);
                            // if (cvTasks != null && cvTasks.CanSort == true)
                            // {
                            //     cvTasks.SortDescriptions.Clear();
                            //     cvTasks.SortDescriptions.Add(new SortDescription("Rank", ListSortDirection.Ascending));
                            //
                            //   }
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

                WebRequest requesta = WebRequest.Create("https://eu.api.blizzard.com/data/wow/guild/howling-fjord/%D1%81%D0%B5%D1%80%D0%B4%D1%86%D0%B5-%D0%B3%D1%80%D0%B5%D1%85%D0%B0/activity?namespace=profile-eu&locale=" + local + "&access_token=" + token_battle_net);
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

                                    guild_activity.AppendText("Дата : " + FromUnixTimeStampToDateTime(activity.activities[i].timestamp).ToString() + "\n" + "\n");

                                }
                                else if (activity.activities[i].encounter_completed != null)
                                {
                                    guild_activity.AppendText("-----------------------" + "\n");
                                    guild_activity.AppendText("Активность гильдии : " + activity.activities[i].encounter_completed.encounter.name + "\n");

                                    guild_activity.AppendText(activity.activities[i].encounter_completed.mode.name + "\n");
                                    guild_activity.AppendText("Время : " + FromUnixTimeStampToDateTime(activity.activities[i].timestamp).ToString() + "\n" + "\n");
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
                WebRequest request = WebRequest.Create("https://eu.api.blizzard.com/data/wow/connected-realm/1615?namespace=dynamic-eu&locale=" + local + "&access_token=" + token_battle_net);
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


                        int index = 1;
                        line = "{ \"logs\": " + line + "}";

                        //  List<Logs> logs_list = new List<Logs>();
                        Warcraftlogs logs = JsonConvert.DeserializeObject<Warcraftlogs>(line);


                        foreach (Logs_all log in logs.logs)
                        {


                            add_guild_wow_logs(index, log.title.ToString(), log.start.ToString(), log.end.ToString(), log.owner.ToString(), "https://ru.warcraftlogs.com/reports/" + log.id.ToString());
                            index++;
                            // logs_list.Add(new Logs() { Dungeon = log.title, Date_start = FromUnixTimeStampToDateTime(log.start).ToString(), Date_end = FromUnixTimeStampToDateTime(log.end).ToString(), Downloader = log.owner, Link = "https://ru.warcraftlogs.com/reports/" + log.id });

                        }


                        //  logs_datagrid.ItemsSource = logs_list;



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
        #endregion

        #region Функции открывания ссылок из таблиц клиента
        private void open_form_links_characters(object sender, RoutedEventArgs e)
        {
            // int parse = guild_data_members.SelectedIndex;
            // int selectedColumn = guild_data_members.CurrentCell.Column.DisplayIndex;
            var selectedCell = guild_data_members.SelectedCells[0];
            var cellContent = selectedCell.Column.GetCellContent(selectedCell.Item);
            // if (cellContent is TextBlock)
            // {
            //     MessageBox.Show((cellContent as TextBlock).Text);
            // }
            // Hyperlink link = (Hyperlink)e.OriginalSource;
            //MessageBox.Show(link.NavigateUri.OriginalString);

            char_info = new Open_char_info();
            //char_info.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            char_info.name = (cellContent as TextBlock).Text;// link.NavigateUri.OriginalString;
            char_info.Left = GetMousePosition().X;

            char_info.Top = GetMousePosition().Y;
            char_info.Show();


            // Process.Start("https://worldofwarcraft.com/ru-ru/character/eu/howling-fjord/" + link.NavigateUri.OriginalString);
        }
        private void Open_wowlogs_link(object sender, RoutedEventArgs e)
        {
            Hyperlink link = (Hyperlink)e.OriginalSource;
            Process.Start(link.NavigateUri.AbsoluteUri);
        }

        private string conduit_rank_ti_ilvl(string rank)
        {
            string ilvl = null;
            if (rank == "1")
            {
                ilvl = "145";
            }
            else if (rank == "2")
            {
                ilvl = "158";
            }
            else if (rank == "3")
            {
                ilvl = "171";
            }
            else if (rank == "4")
            {
                ilvl = "184";
            }
            else if (rank == "5")
            {
                ilvl = "200";
            }
            else if (rank == "6")
            {
                ilvl = "213";
            }
            else if (rank == "7")
            {
                ilvl = "226";
            }

            return ilvl;
        }
        private string insert_tech_talent_discription(string id)
        {
            DataRow[] rows = dTable_tech_talent_info.Select();
            string tooltip_tt = null;
            for (int i = 0; i < rows.Length; i++)
            {
                if (rows[i]["id"].ToString() == id)
                {
                    tooltip_tt = rows[i]["название"].ToString() + "\n" + rows[i]["описание"].ToString() + "\n" + rows[i]["cast_time"].ToString();
                    return tooltip_tt;
                }
            }
            return "Нет описания";
            }

        private string insert_conduit_discription(string id, string rank)
        {
            DataRow[] rows = dTable_conduit_info.Select();
            string tooltip_conduit = null;
            for (int i = 0; i < rows.Length; i++)
            {
                if (rows[i]["id"].ToString() == id)
                {
                    tooltip_conduit = rows[i]["name"].ToString() + "\n" + rows[i][rank].ToString() + "\n" + rows[i]["cast_time"].ToString();
                    return tooltip_conduit;
                }
            }
            return "Нет описания";
        }
        private void open_soul_form(object sender, RoutedEventArgs e)
        {
            string t1;
            string t2;
            string t3;
            string t4;
            //int parse = guild_data_members.SelectedIndex;
            //int selectedColumn = guild_data_members.CurrentCell.Column.DisplayIndex;
            var selectedCell = guild_data_members.SelectedCells[0];
            var cellContent = selectedCell.Column.GetCellContent(selectedCell.Item);
            DataRow[] rows = dTable_members.Select();
            
            string[] teh_talents = null;
            string[] conduits = null;
            string[] conduits_rank = null;
            for (int i = 0; i < rows.Length; i++)
            {

                if (rows[i]["Имя"].ToString() == (cellContent as TextBlock).Text)
                {
                    if (rows[i]["Имя Проводника"].ToString() != "")
                    {

                        #region Tesh Talents
                        teh_talents = rows[i]["Талант"].ToString().Split(new char[] { ',' });


                        if (teh_talents.Length >= 1)
                        {
                            teh_talent_1.Source = ToBitmapImage(Warcraft.Properties.Resources.ResourceManager.GetObject("tesh_talent_" + teh_talents[0]) as Bitmap);
                            teh_talent_1.ToolTip = insert_tech_talent_discription(teh_talents[0]);
                        }
                        else
                        {
                            teh_talent_1.Source = null;
                            teh_talent_1.ToolTip = "Нет описания";
                        }
                        if (teh_talents.Length >= 2)
                        {
                            teh_talent_2.Source = ToBitmapImage(Warcraft.Properties.Resources.ResourceManager.GetObject("tesh_talent_" + teh_talents[1]) as Bitmap);
                            teh_talent_2.ToolTip = insert_tech_talent_discription(teh_talents[1]);
                        }
                        else
                        {
                            teh_talent_2.Source = null;
                            teh_talent_2.ToolTip = "Нет описания";
                        }
                        if (teh_talents.Length >= 3)
                        {
                            teh_talent_3.Source = ToBitmapImage(Warcraft.Properties.Resources.ResourceManager.GetObject("tesh_talent_" + teh_talents[2]) as Bitmap);
                            teh_talent_3.ToolTip = insert_tech_talent_discription(teh_talents[2]);
                        }
                        else
                        {
                            teh_talent_3.Source = null;
                            teh_talent_3.ToolTip = "Нет описания";
                        }
                        if (teh_talents.Length >= 4)
                        {
                            teh_talent_4.Source = ToBitmapImage(Warcraft.Properties.Resources.ResourceManager.GetObject("tesh_talent_" + teh_talents[3]) as Bitmap);
                            teh_talent_4.ToolTip = insert_tech_talent_discription(teh_talents[3]);
                        }
                        else
                        {
                            teh_talent_4.Source = null;
                            teh_talent_4.ToolTip = "Нет описания";
                        }
                        #endregion
                        #region conduits
                        conduits = rows[i]["Проводник"].ToString().Split(new char[] { ',' });
                        conduits_rank = rows[i]["Проводник ранг"].ToString().Split(new char[] { ',' });

                        if (conduits.Length >= 1)
                        {
                            Conduit_1.Source = ToBitmapImage(Warcraft.Properties.Resources.ResourceManager.GetObject("counduit_" + conduits[0]) as Bitmap);
                            rank_conuit_label_1.Content = conduit_rank_ti_ilvl(conduits_rank[0]);
                            Conduit_1.ToolTip = insert_conduit_discription(conduits[0], conduits_rank[0]);
                        }
                        else
                        {
                            Conduit_1.Source = null;
                            rank_conuit_label_1.Content = "";
                            Conduit_1.ToolTip = "Нет описания";
                        }
                        if (conduits.Length >= 2)
                        {
                            Conduit_2.Source = ToBitmapImage(Warcraft.Properties.Resources.ResourceManager.GetObject("counduit_" + conduits[1]) as Bitmap);
                            rank_conuit_label_2.Content = conduit_rank_ti_ilvl(conduits_rank[1]);
                            Conduit_2.ToolTip = insert_conduit_discription(conduits[1], conduits_rank[1]);
                        }
                        else
                        {
                            Conduit_2.Source = null;
                            rank_conuit_label_2.Content = "";
                            Conduit_2.ToolTip = "Нет описания";
                        }
                        if (conduits.Length >= 3)
                        {
                            Conduit_3.Source = ToBitmapImage(Warcraft.Properties.Resources.ResourceManager.GetObject("counduit_" + conduits[2]) as Bitmap);
                            rank_conuit_label_3.Content = conduit_rank_ti_ilvl(conduits_rank[2]);
                            Conduit_3.ToolTip = insert_conduit_discription(conduits[2], conduits_rank[2]);
                        }
                        else
                        {
                            Conduit_3.Source = null;
                            rank_conuit_label_3.Content = "";
                            Conduit_3.ToolTip = "Нет описания";
                        }
                        if (conduits.Length >= 4)
                        {
                            Conduit_4.Source = ToBitmapImage(Warcraft.Properties.Resources.ResourceManager.GetObject("counduit_" + conduits[3]) as Bitmap);
                            rank_conuit_label_4.Content = conduit_rank_ti_ilvl(conduits_rank[3]);
                            Conduit_4.ToolTip = insert_conduit_discription(conduits[3], conduits_rank[3]);
                        }
                        else
                        {
                            Conduit_4.Source = null;
                            rank_conuit_label_4.Content = "";
                            Conduit_4.ToolTip = "Нет описания";
                        }
                        #endregion
                        Soul_name.Content = rows[i]["Имя Проводника"].ToString();
                        Char_info_grid.Visibility = System.Windows.Visibility.Visible;
                    }else
                    {
                        MessageBox.Show("Не выбран медиум у персонажа!");
                    }
                }



            }

            // t1 = teh_talents[1];




        }


        public TimeSpan getRelativeDateTime(DateTime date)
        {
            TimeSpan ts = DateTime.Now - date;
            return ts;
          
        }

        private string relative_time(DateTime date)
        {
            TimeSpan ts = DateTime.Now - date;
              if (ts.TotalMinutes < 1)//seconds ago
                return "just now";
            if (ts.TotalHours < 1)//min ago
                return (int)ts.TotalMinutes == 1 ? "1 Minute ago" : (int)ts.TotalMinutes + " Minutes ago";
            if (ts.TotalDays < 1)//hours ago
                return (int)ts.TotalHours == 1 ? "1 Hour ago" : (int)ts.TotalHours + " Hours ago";
            if (ts.TotalDays < 7)//days ago
                return (int)ts.TotalDays == 1 ? "1 Day ago" : (int)ts.TotalDays + " Days ago";
            if (ts.TotalDays < 30.4368)//weeks ago
                return (int)(ts.TotalDays / 7) == 1 ? "1 Week ago" : (int)(ts.TotalDays / 7) + " Weeks ago";
            if (ts.TotalDays < 365.242)//months ago
                return (int)(ts.TotalDays / 30.4368) == 1 ? "1 Month ago" : (int)(ts.TotalDays / 30.4368) + " Months ago";
            //years ago
            return (int)(ts.TotalDays / 365.242) == 1 ? "1 Year ago" : (int)(ts.TotalDays / 365.242) + " Years ago"; 
        }
        #endregion

        #region Получение данных с сервера клиентом

        DataTable dTable_members = new DataTable();
        DataTable dTable_tech_talent_info = new DataTable();
        DataTable dTable_conduit_info = new DataTable();

        public void insert_conduit_info()
        {
            try
            {
                string MyConnection2 = "datasource=185.130.83.244;port=3306;username=warcraft;password=Dwqi7mbEziT6jphl;database=guild_info;CharSet=utf8";
                //Display query  
                string Query = "select * from guild_info.wow_conduit;";
                MySqlConnection MyConn2 = new MySqlConnection(MyConnection2);
                MySqlCommand MyCommand2 = new MySqlCommand(Query, MyConn2);
                //  MyConn2.Open();  
                //For offline connection we weill use  MySqlDataAdapter class.  
                MySqlDataAdapter MyAdapter = new MySqlDataAdapter();
                MyAdapter.SelectCommand = MyCommand2;
                dTable_conduit_info.Clear();
                MyAdapter.Fill(dTable_conduit_info);


                MyConn2.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }
        public void insert_tech_talent_info()
        {
            try
            {
                string MyConnection2 = "datasource=185.130.83.244;port=3306;username=warcraft;password=Dwqi7mbEziT6jphl;database=guild_info;CharSet=utf8";
                //Display query  
                string Query = "select * from guild_info.wow_tech_talant;";
                MySqlConnection MyConn2 = new MySqlConnection(MyConnection2);
                MySqlCommand MyCommand2 = new MySqlCommand(Query, MyConn2);
                //  MyConn2.Open();  
                //For offline connection we weill use  MySqlDataAdapter class.  
                MySqlDataAdapter MyAdapter = new MySqlDataAdapter();
                MyAdapter.SelectCommand = MyCommand2;
                dTable_tech_talent_info.Clear();
                MyAdapter.Fill(dTable_tech_talent_info);
                
                            
                MyConn2.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }
        public void insert_guild_member()
        {
            try
            {
                string MyConnection2 = "datasource=185.130.83.244;port=3306;username=warcraft;password=Dwqi7mbEziT6jphl;database=guild_info;CharSet=utf8";
                //Display query  
                string Query = "select * from guild_info.guild_members;";
                MySqlConnection MyConn2 = new MySqlConnection(MyConnection2);
                MySqlCommand MyCommand2 = new MySqlCommand(Query, MyConn2);
                //  MyConn2.Open();  
                //For offline connection we weill use  MySqlDataAdapter class.  
                MySqlDataAdapter MyAdapter = new MySqlDataAdapter();
                MyAdapter.SelectCommand = MyCommand2;
                dTable_members.Clear();
                MyAdapter.Fill(dTable_members);
                DataRow[] rows = dTable_members.Select();
                List<User> users = new List<User>();
                for (int i = 0; i < rows.Length; i++)
                {


                    string str_class = rows[i]["Класс"].ToString();
                    string str_spec = rows[i]["Спек"].ToString();
                    string str_coven = rows[i]["Ковенант"].ToString();
                    string last_login_game = null;
                    if (rows[i]["В игре"].ToString() != "0")
                    {
                        if (getRelativeDateTime(FromUnixTimeStampToDateTime(rows[i]["В игре"].ToString())).TotalDays <= 14)
                        {
                            last_login_game = relative_time(FromUnixTimeStampToDateTime(rows[i]["В игре"].ToString()));//.ToString();
                            int item_level;
                            if (rows[i]["Илвл"].ToString() == "error")
                            {
                                item_level = 0;
                            }
                            else
                            {
                                item_level = Convert.ToInt32(rows[i]["Илвл"].ToString());
                            }
                            BitmapImage bmp_class = ToBitmapImage(Warcraft.Properties.Resources.ResourceManager.GetObject("class_" + str_class) as Bitmap);
                            BitmapImage bmp_spec = ToBitmapImage(Warcraft.Properties.Resources.ResourceManager.GetObject("spec_" + str_spec) as Bitmap);
                            BitmapImage bmp_coven = ToBitmapImage(Warcraft.Properties.Resources.ResourceManager.GetObject("coven_" + str_coven) as Bitmap);
                            if (!last_login_game.Contains("Month ago"))
                            {
                                users.Add(new User() { Name = rows[i]["Имя"].ToString(), Level = Convert.ToInt32(rows[i]["Уровень"].ToString()), Class = bmp_class, Spec = bmp_spec, Rank = Convert.ToInt32(rows[i]["Ранг"].ToString()), Ilvl = item_level, Raid_progress = rows[i]["Рейд"].ToString(), MythicScore = Convert.ToDouble(rows[i]["R.I.O."].ToString().Replace(".", ",")), Coven = bmp_coven, Coven_lvl = Convert.ToInt32(rows[i]["Известность"].ToString()), Coven_soul = last_login_game, link = "Просмотр" });

                            }
                        }
                        
                    }
                    else
                    {
                        last_login_game = "0";
                    }
                    

                    //MessageBox.Show(users[i].Name + "\n" + users[i].Level + "\n" + users[i].Class + "\n" + users[i].Spec + "\n" + users[i].Rank + "\n" + users[i].Ilvl + "\n" + users[i].Raid_progress + "\n" + users[i].MythicScore + "\n" + users[i].Coven + "\n" + users[i].Coven_lvl + "\n" + users[i].Coven_soul + "\n");

                }
                guild_members_count.Content = rows.Length;
                guild_data_members.ItemsSource = users;



                ICollectionView cvTasks = CollectionViewSource.GetDefaultView(guild_data_members.ItemsSource);
                if (cvTasks != null && cvTasks.CanSort == true)
                {
                    cvTasks.SortDescriptions.Clear();
                    cvTasks.SortDescriptions.Add(new SortDescription("Rank", ListSortDirection.Ascending));
                }

                // guild_data_members.ItemsSource = dTable.DefaultView;
                //guild_data_members.DataContext = dTable; // here i have assign dTable object to the dataGridView1 object to display data.               
                MyConn2.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        public void insert_guild_wow_logs()
        {
            try
            {
                string MyConnection2 = "datasource=185.130.83.244;port=3306;username=warcraft;password=Dwqi7mbEziT6jphl;database=guild_info;CharSet=utf8";
                //Display query  
                string Query = "select * from guild_info.guild_wow_logs;";
                MySqlConnection MyConn2 = new MySqlConnection(MyConnection2);
                MySqlCommand MyCommand2 = new MySqlCommand(Query, MyConn2);
                //  MyConn2.Open();  
                //For offline connection we weill use  MySqlDataAdapter class.  
                MySqlDataAdapter MyAdapter = new MySqlDataAdapter();
                MyAdapter.SelectCommand = MyCommand2;
                DataTable dTable = new DataTable();
                MyAdapter.Fill(dTable);
                DataRow[] rows = dTable.Select();
                List<Logs> logs = new List<Logs>();
                for (int i = 0; i < rows.Length; i++)
                {



                    logs.Add(new Logs() { ID = Convert.ToInt32(rows[i]["id"]), Dungeon = rows[i]["Dungeon"].ToString(), Date_start = FromUnixTimeStampToDateTime(rows[i]["Date_start"].ToString()).ToString(), Date_end = FromUnixTimeStampToDateTime(rows[i]["Date_end"].ToString()).ToString(), Downloader = rows[i]["Downloader"].ToString(), Link = rows[i]["Link"].ToString() });


                }

                logs_datagrid.ItemsSource = logs;

                ICollectionView cvTasks = CollectionViewSource.GetDefaultView(logs_datagrid.ItemsSource);
                if (cvTasks != null && cvTasks.CanSort == true)
                {
                    cvTasks.SortDescriptions.Clear();
                    cvTasks.SortDescriptions.Add(new SortDescription("ID", ListSortDirection.Ascending));
                }



                MyConn2.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }
        public void insert_guild_main_info()
        {
            try
            {
                string MyConnection2 = "datasource=185.130.83.244;port=3306;username=warcraft;password=Dwqi7mbEziT6jphl;database=guild_info;CharSet=utf8";
                //Display query  
                string Query = "select * from guild_info.guild_main_info;";
                MySqlConnection MyConn2 = new MySqlConnection(MyConnection2);
                MySqlCommand MyCommand2 = new MySqlCommand(Query, MyConn2);
                //  MyConn2.Open();  
                //For offline connection we weill use  MySqlDataAdapter class.  
                MySqlDataAdapter MyAdapter = new MySqlDataAdapter();
                MyAdapter.SelectCommand = MyCommand2;
                DataTable dTable = new DataTable();
                MyAdapter.Fill(dTable);
                DataRow[] rows = dTable.Select();

                for (int i = 0; i < rows.Length; i++)
                {
                    guild_name_label.Content = rows[i]["guild_name"].ToString();
                    guild_master_label.Content = rows[i]["guild_lead"].ToString();
                    Guild_progress_label.Content = rows[i]["guild_raid_progress"].ToString();
                    last_update_time.Content = "Последнее обновление на сервере в " + rows[i]["last_update"] + "(+4 мск)";
                }







                MyConn2.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }
        string server_status;
        public void insert_main_info()
        {
            try
            {
                string MyConnection2 = "datasource=185.130.83.244;port=3306;username=warcraft;password=Dwqi7mbEziT6jphl;database=guild_info;CharSet=utf8";
                //Display query  
                string Query = "select * from guild_info.main;";
                MySqlConnection MyConn2 = new MySqlConnection(MyConnection2);
                MySqlCommand MyCommand2 = new MySqlCommand(Query, MyConn2);
                //  MyConn2.Open();  
                //For offline connection we weill use  MySqlDataAdapter class.  
                MySqlDataAdapter MyAdapter = new MySqlDataAdapter();
                MyAdapter.SelectCommand = MyCommand2;
                DataTable dTable = new DataTable();
                MyAdapter.Fill(dTable);
                DataRow[] rows = dTable.Select();

                for (int i = 0; i < rows.Length; i++)
                {
                    server_status = rows[i]["update_info"].ToString();
                }







                MyConn2.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        #endregion

        #region Отправка данных сервера в базу данных
        private void add_guild_main_info(string guild_name, string guild_lead, string guild_raid_progress, string last_update)//добавляем в базу данных использование фичи
        {
            var thread = new Thread(() =>
            {

                string connectionString = "datasource=185.130.83.244;port=3306;username=warcraft;password=Dwqi7mbEziT6jphl;database=guild_info;CharSet=utf8";
                string insert = "INSERT INTO guild_main_info(`guild_name`, `guild_lead`, `guild_raid_progress`, `last_update`) " +
                "VALUES ('" + guild_name + "', '" + guild_lead + "', '" + guild_raid_progress + "', '" + last_update + "')";
                // Which could be translated manually to :
                // INSERT INTO user(`id`, `first_name`, `last_name`, `address`) VALUES (NULL, 'Bruce', 'Wayne', 'Wayne Manor')


                MySqlConnection databaseConnection = new MySqlConnection(connectionString);
                MySqlCommand commandDatabase = new MySqlCommand(insert, databaseConnection);

                commandDatabase.CommandTimeout = 10;

                try
                {
                    databaseConnection.Open();

                    MySqlDataReader myReader = commandDatabase.ExecuteReader();

                    //MessageBox.Show("User succesfully registered");

                    databaseConnection.Close();
                }
                catch (Exception ex)
                {
                    // Show any error message.
                    // MessageBox.Show(ex.Message);


                }
            });

            thread.Start();
        }

        private void add_guild_info(string name, string lvl, string game_class, string spec, string rang, string ilvl, string raid, string mythic, string coven, string coven_lvl, string name_soul, string teh_talent, string conduits_id, string conduits_rank, string last_login)//добавляем в базу данных использование фичи
        {
            var thread = new Thread(() =>
            {

                string connectionString = "datasource=185.130.83.244;port=3306;username=warcraft;password=Dwqi7mbEziT6jphl;database=guild_info;CharSet=utf8";
                string insert = "INSERT INTO guild_members(`Имя`, `Уровень`, `Класс`, `Спек`, `Ранг` , `Илвл`, `Рейд`, `R.I.O.`, `Ковенант`, `Известность`, `В игре`, `Имя Проводника`, `Проводник`, `Проводник ранг`, `Талант`) " +
                "VALUES ('" + name + "', '" + lvl + "', '" + game_class + "', '" + spec + "','" + rang + "', '" + ilvl + "', '" + raid + "', '" + mythic + "', '" + coven + "', '" + coven_lvl + "', '" + last_login + "', '" + name_soul + "', '" + conduits_id + "', '" + conduits_rank + "', '" + teh_talent + "')";
                // Which could be translated manually to :
                // INSERT INTO user(`id`, `first_name`, `last_name`, `address`) VALUES (NULL, 'Bruce', 'Wayne', 'Wayne Manor')


                MySqlConnection databaseConnection = new MySqlConnection(connectionString);
                MySqlCommand commandDatabase = new MySqlCommand(insert, databaseConnection);

                commandDatabase.CommandTimeout = 10;

                try
                {
                    databaseConnection.Open();

                    MySqlDataReader myReader = commandDatabase.ExecuteReader();

                    //MessageBox.Show("User succesfully registered");

                    databaseConnection.Close();
                }
                catch (Exception ex)
                {
                    // Show any error message.
                    // MessageBox.Show(ex.Message);


                }
            });

            thread.Start();
        }
        private void add_guild_wow_logs(int i, string dungeon, string date_start, string date_end, string downloader, string link)//добавляем в базу данных использование фичи
        {
            var thread = new Thread(() =>
            {

                string connectionString = "datasource=185.130.83.244;port=3306;username=warcraft;password=Dwqi7mbEziT6jphl;database=guild_info;CharSet=utf8";
                string insert = "INSERT INTO guild_wow_logs(`id`, `Dungeon`, `Date_start`, `Date_end`, `Downloader`, `Link`) " +
                "VALUES ('" + i + "', '" + dungeon + "', '" + date_start + "', '" + date_end + "', '" + downloader + "', '" + link + "')";
                // Which could be translated manually to :
                // INSERT INTO user(`id`, `first_name`, `last_name`, `address`) VALUES (NULL, 'Bruce', 'Wayne', 'Wayne Manor')


                MySqlConnection databaseConnection = new MySqlConnection(connectionString);
                MySqlCommand commandDatabase = new MySqlCommand(insert, databaseConnection);

                commandDatabase.CommandTimeout = 10;

                try
                {
                    databaseConnection.Open();

                    MySqlDataReader myReader = commandDatabase.ExecuteReader();

                    //MessageBox.Show("User succesfully registered");

                    databaseConnection.Close();
                }
                catch (Exception ex)
                {
                    // Show any error message.
                    // MessageBox.Show(ex.Message);


                }
            });

            thread.Start();
        }

        private void add__main_info_status(string status)//добавляем в базу данных использование фичи
        {
            var thread = new Thread(() =>
            {

                string connectionString = "datasource=185.130.83.244;port=3306;username=warcraft;password=Dwqi7mbEziT6jphl;database=guild_info;CharSet=utf8";
                string insert = "INSERT INTO main(`update_info`) " +
                "VALUES ('" + status + "')";
                // Which could be translated manually to :
                // INSERT INTO user(`id`, `first_name`, `last_name`, `address`) VALUES (NULL, 'Bruce', 'Wayne', 'Wayne Manor')


                MySqlConnection databaseConnection = new MySqlConnection(connectionString);
                MySqlCommand commandDatabase = new MySqlCommand(insert, databaseConnection);

                commandDatabase.CommandTimeout = 10;

                try
                {
                    databaseConnection.Open();

                    MySqlDataReader myReader = commandDatabase.ExecuteReader();

                    //MessageBox.Show("User succesfully registered");

                    databaseConnection.Close();
                }
                catch (Exception ex)
                {
                    // Show any error message.
                    // MessageBox.Show(ex.Message);


                }
            });

            thread.Start();
        }

        #endregion

        #region Очистка базы данных
        private void delete_guild_info()
        {
            var thread = new Thread(() =>
            {
                string connectionString = "datasource=185.130.83.244;port=3306;username=warcraft;password=Dwqi7mbEziT6jphl;database=guild_info;CharSet=utf8";

                string delete = "DELETE FROM guild_members";


                MySqlConnection databaseConnection = new MySqlConnection(connectionString);

                MySqlCommand commandDatabasedelete = new MySqlCommand(delete, databaseConnection);

                commandDatabasedelete.CommandTimeout = 10;


                try
                {
                    databaseConnection.Open();
                    MySqlDataReader myReaderdelete = commandDatabasedelete.ExecuteReader();


                    //MessageBox.Show("User succesfully registered");

                    databaseConnection.Close();
                }
                catch (Exception ex)
                {
                    // Show any error message.
                    // MessageBox.Show(ex.Message);


                }
            });

            thread.Start();
        }

        private void delete_guild_main_info()
        {
            var thread = new Thread(() =>
            {
                string connectionString = "datasource=185.130.83.244;port=3306;username=warcraft;password=Dwqi7mbEziT6jphl;database=guild_info;CharSet=utf8";


                string delete2 = "DELETE FROM guild_main_info";


                MySqlConnection databaseConnection = new MySqlConnection(connectionString);


                MySqlCommand commandDatabasedelete2 = new MySqlCommand(delete2, databaseConnection);

                commandDatabasedelete2.CommandTimeout = 10;


                try
                {
                    databaseConnection.Open();
                    MySqlDataReader myReaderdelete = commandDatabasedelete2.ExecuteReader();

                    //MessageBox.Show("User succesfully registered");

                    databaseConnection.Close();
                }
                catch (Exception ex)
                {
                    // Show any error message.
                    // MessageBox.Show(ex.Message);


                }
            });

            thread.Start();
        }
        private void delete_guild_wow_logs()
        {
            var thread = new Thread(() =>
            {
                string connectionString = "datasource=185.130.83.244;port=3306;username=warcraft;password=Dwqi7mbEziT6jphl;database=guild_info;CharSet=utf8";


                string delete3 = "DELETE FROM guild_wow_logs";

                MySqlConnection databaseConnection = new MySqlConnection(connectionString);


                MySqlCommand commandDatabasedelete3 = new MySqlCommand(delete3, databaseConnection);
                commandDatabasedelete3.CommandTimeout = 10;


                try
                {
                    databaseConnection.Open();
                    MySqlDataReader myReaderdelete = commandDatabasedelete3.ExecuteReader();

                    //MessageBox.Show("User succesfully registered");

                    databaseConnection.Close();
                }
                catch (Exception ex)
                {
                    // Show any error message.
                    // MessageBox.Show(ex.Message);


                }
            });

            thread.Start();
        }

        private void delete_main_Status_update()
        {
            var thread = new Thread(() =>
            {
                string connectionString = "datasource=185.130.83.244;port=3306;username=warcraft;password=Dwqi7mbEziT6jphl;database=guild_info;CharSet=utf8";


                string delete3 = "DELETE FROM main";

                MySqlConnection databaseConnection = new MySqlConnection(connectionString);


                MySqlCommand commandDatabasedelete3 = new MySqlCommand(delete3, databaseConnection);
                commandDatabasedelete3.CommandTimeout = 10;


                try
                {
                    databaseConnection.Open();
                    MySqlDataReader myReaderdelete = commandDatabasedelete3.ExecuteReader();

                    //MessageBox.Show("User succesfully registered");

                    databaseConnection.Close();
                }
                catch (Exception ex)
                {
                    // Show any error message.
                    // MessageBox.Show(ex.Message);


                }
            });

            thread.Start();
        }
        #endregion

        #region Updater
        public bool testSite(string url)
        {

            Uri uri = new Uri(url);
            try
            {
                HttpWebRequest httpWebRequest = (HttpWebRequest)HttpWebRequest.Create(uri);

                HttpWebResponse httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            }
            catch
            {
                return false;
            }
            return true;
        }

        public bool CheckVersion()
        {
            int FileMajorPart = 0;
            int FileMinorPart = 0;
            int FileBuildPart = 0;
            int FilePrivatePart = 0;

            XDocument versionxml = null;
            versionxml = XDocument.Load("http://185.130.83.244/Warcraft/Version.xml");
            XElement FileMajor = versionxml.Element("version").Element("FileMajorPart");
            XElement FileMinor = versionxml.Element("version").Element("FileMinorPart");
            XElement FileBuild = versionxml.Element("version").Element("FileBuildPart");
            XElement FilePrivate = versionxml.Element("version").Element("FilePrivatePart");

            FileMajorPart = Int16.Parse(FileMajor.Value);
            FileMinorPart = Int16.Parse(FileMinor.Value);
            FileBuildPart = Int16.Parse(FileBuild.Value);
            FilePrivatePart = Int16.Parse(FilePrivate.Value);

            if (FileMajorPart > FileVersionInfo.GetVersionInfo(@Environment.CurrentDirectory + "/Warcraft.exe").FileMajorPart)
            {

                return true;
            }
            if (FileMinorPart > FileVersionInfo.GetVersionInfo(@Environment.CurrentDirectory + "/Warcraft.exe").FileMinorPart)
            {

                return true;
            }
            if (FileBuildPart > FileVersionInfo.GetVersionInfo(@Environment.CurrentDirectory + "/Warcraft.exe").FileBuildPart)
            {

                return true;
            }
            if (FilePrivatePart > FileVersionInfo.GetVersionInfo(@Environment.CurrentDirectory + "/Warcraft.exe").FilePrivatePart)
            {

                return true;
            }
            return false;

        }

        public void Update()
        {
            if (File.Exists(@Environment.CurrentDirectory + "/delete.exe"))
            {
                try
                {
                    File.Delete(@Environment.CurrentDirectory + "/delete.exe");
                }
                catch (Exception e)
                {
                    MessageBox.Show("Не могу удалить");
                }

            }


            WebClient updater_dl = new WebClient();

            if (testSite("http://185.130.83.244"))
            {



                if (CheckVersion())
                {
                    need_update = true;
                    try
                    {

                        try
                        {


                            File.Move(@Environment.CurrentDirectory + "/Warcraft.exe", @Environment.CurrentDirectory + "/delete.exe");

                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Не смог переименовать файл warcraft.exe" + ex.ToString());
                        }
                        Thread.Sleep(3000);
                        updater_dl.DownloadFileAsync(new Uri("http://185.130.83.244/Warcraft/Warcraft.exe"), @Environment.CurrentDirectory + "/Warcraft.exe");




                        Thread.Sleep(1000);
                        MessageBox.Show("Приложение обновленно.\nНажмите \"ОК\" для перезапуска");
                        Process.Start(@Environment.CurrentDirectory + "/Warcraft.exe");
                        Close();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Не смог обновить приложение ^(" + ex);
                    }
                }
                else
                {

                }
            }
            else
            {

            }

        }

        #endregion

        #region Отправка клиентом данных в базу данных
        private void add_user_info(string name, string date)//добавляем в базу данных использование фичи
        {
            var thread = new Thread(() =>
            {

                string connectionString = "datasource=185.130.83.244;port=3306;username=warcraft;password=Dwqi7mbEziT6jphl;database=guild_info;CharSet=utf8";
                string insert = "INSERT INTO users(`name`, `date`) " +
                "VALUES ('" + name + "', '" + date + "')";
                // Which could be translated manually to :
                // INSERT INTO user(`id`, `first_name`, `last_name`, `address`) VALUES (NULL, 'Bruce', 'Wayne', 'Wayne Manor')


                MySqlConnection databaseConnection = new MySqlConnection(connectionString);
                MySqlCommand commandDatabase = new MySqlCommand(insert, databaseConnection);

                commandDatabase.CommandTimeout = 10;

                try
                {
                    databaseConnection.Open();

                    MySqlDataReader myReader = commandDatabase.ExecuteReader();

                    //MessageBox.Show("User succesfully registered");

                    databaseConnection.Close();
                }
                catch (Exception ex)
                {
                    // Show any error message.
                    // MessageBox.Show(ex.Message);


                }
            });

            thread.Start();
        }
        #endregion

        #region Звгрузка описания проводников
        string conduit_id;
        private void download_icons_0()
        {

            try
            {
                WebRequest requestchar = WebRequest.Create("https://eu.api.blizzard.com/data/wow/covenant/conduit/index?namespace=static-eu&locale=" + local + "&access_token=" + token_battle_net);

                WebResponse responcechar = requestchar.GetResponse();

                using (Stream stream1 = responcechar.GetResponseStream())

                {
                    using (StreamReader reader1 = new StreamReader(stream1))
                    {
                        string line = "";
                        while ((line = reader1.ReadLine()) != null)
                        {



                            Root_Conduit conduit = JsonConvert.DeserializeObject<Root_Conduit>(line);

                            foreach (Conduit con in conduit.conduits)
                            {
                                conduit_id = con.id.ToString();
                                download_icons_1(con.key.href);

                            }


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

                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);

            }
        }

        private void download_icons_1(string link)
        {

            try
            {
                WebRequest requestchar = WebRequest.Create(link + "&locale=" + local + "&access_token=" + token_battle_net);

                WebResponse responcechar = requestchar.GetResponse();

                using (Stream stream1 = responcechar.GetResponseStream())

                {
                    using (StreamReader reader1 = new StreamReader(stream1))
                    {
                        string line = "";
                        while ((line = reader1.ReadLine()) != null)
                        {



                            Root_conduit_info conduit_info = JsonConvert.DeserializeObject<Root_conduit_info>(line);
                            //download_icons_2(conduit_info.ranks[1].spell_tooltip.spell.key.href);
                            add_conduit_info(conduit_info.id.ToString(), conduit_info.name.ToString(), conduit_info.ranks[0].spell_tooltip.description.ToString(),
                                conduit_info.ranks[1].spell_tooltip.description.ToString(), conduit_info.ranks[2].spell_tooltip.description.ToString(),
                                conduit_info.ranks[3].spell_tooltip.description.ToString(), conduit_info.ranks[4].spell_tooltip.description.ToString(),
                                conduit_info.ranks[5].spell_tooltip.description.ToString(), conduit_info.ranks[6].spell_tooltip.description.ToString(),
                                conduit_info.ranks[7].spell_tooltip.description.ToString(), conduit_info.ranks[8].spell_tooltip.description.ToString(),
                                conduit_info.ranks[9].spell_tooltip.description.ToString(), conduit_info.ranks[10].spell_tooltip.description.ToString()
                                , conduit_info.ranks[11].spell_tooltip.description.ToString(), conduit_info.ranks[12].spell_tooltip.description.ToString(),
                                conduit_info.ranks[13].spell_tooltip.description.ToString(), conduit_info.ranks[14].spell_tooltip.description.ToString(),
                                conduit_info.ranks[0].spell_tooltip.cast_time.ToString());




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

                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);

            }
        }

        private void download_icons_2(string link)
        {

            try
            {
                WebRequest requestchar = WebRequest.Create(link + "&locale=" + local + "&access_token=" + token_battle_net);

                WebResponse responcechar = requestchar.GetResponse();

                using (Stream stream1 = responcechar.GetResponseStream())

                {
                    using (StreamReader reader1 = new StreamReader(stream1))
                    {
                        string line = "";
                        while ((line = reader1.ReadLine()) != null)
                        {



                            Root_conduit_info_spell conduit_info = JsonConvert.DeserializeObject<Root_conduit_info_spell>(line);
                            download_icons_3(conduit_info.media.key.href);



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

                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);

            }
        }

        private void download_icons_3(string link)
        {

            try
            {
                WebRequest requestchar = WebRequest.Create(link + "&locale=" + local + "&access_token=" + token_battle_net);

                WebResponse responcechar = requestchar.GetResponse();

                using (Stream stream1 = responcechar.GetResponseStream())

                {
                    using (StreamReader reader1 = new StreamReader(stream1))
                    {
                        string line = "";
                        while ((line = reader1.ReadLine()) != null)
                        {



                            Root_conduit_icon conduit_info = JsonConvert.DeserializeObject<Root_conduit_icon>(line);
                            foreach (Asset_conduit_icon asset in conduit_info.assets)
                            {
                                WebClient download = new WebClient();
                                download.DownloadFile(asset.value, "C:/conduit/counduit_" + conduit_id + ".jpeg");
                            }



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

                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);

            }
        }

        
        private void add_conduit_info(string id, string name, string rank1, string rank2, string rank3, string rank4, string rank5, 
            string rank6, string rank7, string rank8, string rank9, string rank10, string rank11, string rank12, string rank13, string rank14, string rank15, string cast_time)//добавляем в базу данных использование фичи
        {
            var thread = new Thread(() =>
            {

                string connectionString = "datasource=185.130.83.244;port=3306;username=warcraft;password=Dwqi7mbEziT6jphl;database=guild_info;CharSet=utf8";
                string insert = "INSERT INTO wow_conduit(`id`, `name`, `1`, `2`, `3`, `4`, `5`, `6`, `7`, `8`, `9`, `10`, `11`, `12`, `13`, `14`, `15`, `cast_time`) " +
                "VALUES ('" + id + "', '" + name + "', '" + rank1 + "', '" + rank2 + "', '" + rank3 + "', '" + rank4 + "', '" + rank5 + "', '"
                + rank6 + "', '" + rank7 + "', '" + rank8 + "', '" + rank9 + "', '" + rank10 + "', '" + rank11 + "', '" + rank12 + "', '"
                + rank13 + "', '" + rank14 + "', '" + rank15 + "', '" + cast_time + "')";
                // Which could be translated manually to :
                // INSERT INTO user(`id`, `first_name`, `last_name`, `address`) VALUES (NULL, 'Bruce', 'Wayne', 'Wayne Manor')


                MySqlConnection databaseConnection = new MySqlConnection(connectionString);
                MySqlCommand commandDatabase = new MySqlCommand(insert, databaseConnection);

                commandDatabase.CommandTimeout = 10;

                try
                {
                    databaseConnection.Open();

                    MySqlDataReader myReader = commandDatabase.ExecuteReader();

                    //MessageBox.Show("User succesfully registered");

                    databaseConnection.Close();
                }
                catch (Exception ex)
                {
                    // Show any error message.
                    // MessageBox.Show(ex.Message);


                }
            });

            thread.Start();
        }
#endregion

        #region Загрузка тех талантов ковенантов
        string tech_talent_id;
        private void download_icons_TT_0()
        {

            try
            {
                WebRequest requestchar = WebRequest.Create("https://eu.api.blizzard.com/data/wow/tech-talent/index?namespace=static-eu&locale=" + local + "&access_token=" + token_battle_net);

                WebResponse responcechar = requestchar.GetResponse();

                using (Stream stream1 = responcechar.GetResponseStream())

                {
                    using (StreamReader reader1 = new StreamReader(stream1))
                    {
                        string line = "";
                        while ((line = reader1.ReadLine()) != null)
                        {



                            Root_tech_talent tal = JsonConvert.DeserializeObject<Root_tech_talent>(line);

                            foreach (Talent_tech_talent talen in tal.talents)
                            {
                                //tech_talent_id = talen.id.ToString();
                                //download_icons_TT_3(talen.id.ToString());
                                download_icons_TT_info(talen.id.ToString());
                            }


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

                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);

            }
        }

        private void download_icons_TT_3(string id)
        {

            try
            {
                WebRequest requestchar = WebRequest.Create("https://eu.api.blizzard.com/data/wow/media/tech-talent/" + id + "?namespace=static-eu&locale=" + local + "&access_token=" + token_battle_net);

                WebResponse responcechar = requestchar.GetResponse();

                using (Stream stream1 = responcechar.GetResponseStream())

                {
                    using (StreamReader reader1 = new StreamReader(stream1))
                    {
                        string line = "";
                        while ((line = reader1.ReadLine()) != null)
                        {



                            Media_Tesh_talent conduit_info = JsonConvert.DeserializeObject<Media_Tesh_talent>(line);
                            foreach (Asset_Media_Tesh_talent asset in conduit_info.assets)
                            {
                                WebClient download = new WebClient();
                                download.DownloadFile(asset.value, "C:/tech_talent/tesh_talent_" + id + ".jpeg");
                            }



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

                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);

            }
        }

        private void download_icons_TT_info(string id)
        {

            try
            {
                WebRequest requestchar = WebRequest.Create("https://eu.api.blizzard.com/data/wow/tech-talent/" + id + "?namespace=static-eu&locale=" + local + "&access_token=" + token_battle_net);

                WebResponse responcechar = requestchar.GetResponse();

                using (Stream stream1 = responcechar.GetResponseStream())

                {
                    using (StreamReader reader1 = new StreamReader(stream1))
                    {
                        string line = "";
                        while ((line = reader1.ReadLine()) != null)
                        {



                            Telent_tech_info tech_talent = JsonConvert.DeserializeObject<Telent_tech_info>(line);
                            add_tech_talent_info(tech_talent.id.ToString(), tech_talent.name.ToString(), tech_talent.spell_tooltip.description.ToString(), tech_talent.spell_tooltip.cast_time.ToString());



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

                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);

            }
        }
        private void add_tech_talent_info(string id, string name, string discription, string cast_time)//добавляем в базу данных использование фичи
        {
            var thread = new Thread(() =>
            {

                string connectionString = "datasource=185.130.83.244;port=3306;username=warcraft;password=Dwqi7mbEziT6jphl;database=guild_info;CharSet=utf8";
                string insert = "INSERT INTO wow_tech_talant(`id`, `название`, `описание`, `cast_time`) " +
                "VALUES ('" + id + "', '" + name + "', '" + discription + "', '" + cast_time + "')";
                // Which could be translated manually to :
                // INSERT INTO user(`id`, `first_name`, `last_name`, `address`) VALUES (NULL, 'Bruce', 'Wayne', 'Wayne Manor')


                MySqlConnection databaseConnection = new MySqlConnection(connectionString);
                MySqlCommand commandDatabase = new MySqlCommand(insert, databaseConnection);

                commandDatabase.CommandTimeout = 10;

                try
                {
                    databaseConnection.Open();

                    MySqlDataReader myReader = commandDatabase.ExecuteReader();

                    //MessageBox.Show("User succesfully registered");

                    databaseConnection.Close();
                }
                catch (Exception ex)
                {
                    // Show any error message.
                    // MessageBox.Show(ex.Message);


                }
            });

            thread.Start();
        }
        #endregion
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Char_info_grid.Visibility = System.Windows.Visibility.Hidden;
        }
    }
}
