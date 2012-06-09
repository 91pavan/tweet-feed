using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using System.Xml.Linq;

namespace tweet_feed
{
    public partial class MainPage : PhoneApplicationPage
    {
        public string followers_count1;
        public string tweets_count1;
        public string url;
        public string desc;
        // Constructor
        public MainPage()
        {
            InitializeComponent();
        }

        private void Updatebutton_Click(object sender, RoutedEventArgs e)
        {
            if ((textBox1.Text.Equals("enter username..")) || (textBox1.Text.Equals("")))
            {
                textBox1.Text = ""; MessageBox.Show("Please type a proper username."); return;
            }
            progress.Visibility = System.Windows.Visibility.Visible;
            WebClient twitter = new WebClient();
            try
            {

                twitter.DownloadStringCompleted += new DownloadStringCompletedEventHandler(twitter_DownloadStringCompleted);
                twitter.DownloadStringAsync(new Uri("http://api.twitter.com/1/statuses/user_timeline.xml?screen_name=" + textBox1.Text));
                
            }
            catch
            {
                MessageBox.Show("Please check your connection");
            }
        }

        void twitter_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
        {

            if (e.Error != null)
            {
                MessageBox.Show("Unable to parse. Check username or network connection."); 
                progress.Visibility = System.Windows.Visibility.Collapsed; return;
            }


            try
            {
                
                XElement xmltweets = XElement.Parse(e.Result);
                progress.Visibility = System.Windows.Visibility.Collapsed;
                Tweetlist.ItemsSource = from tweet in xmltweets.Descendants("status")
                                        select new TwitterItem
                                        {
                                            ImageSource = tweet.Element("user").Element("profile_image_url").Value,
                                            Message = tweet.Element("text").Value,
                                            Username = tweet.Element("user").Element("screen_name").Value,
                                            Date = tweet.Element("created_at").Value,
                                            followers = tweet.Element("user").Element("followers_count").Value,
                                            description = tweet.Element("user").Element("description").Value,
                                            URL = tweet.Element("user").Element("url").Value,
                                            Retweets = tweet.Element("retweet_count").Value,
                                            Tweets_count = tweet.Element("user").Element("statuses_count").Value
                                        };
                followers_count1 = xmltweets.Element("status").Element("user").Element("followers_count").Value;
                url = xmltweets.Element("status").Element("user").Element("url").Value;
                desc = xmltweets.Element("status").Element("user").Element("description").Value;
                tweets_count1 = xmltweets.Element("status").Element("user").Element("statuses_count").Value;
            }
            catch
            {
                MessageBox.Show("Unable to parse. Check username");
            }
        }
        private void StackPanel_Tap(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Followers : " + followers_count1 + "\nURL : " + url + "\nDesciption : " + desc + "\nNo. Of Tweets : " + tweets_count1);
        }
        private void textBox1_GotFocus(object sender, RoutedEventArgs e)
        {
            if (textBox1.Text.Equals("enter username.."))
            {
                textBox1.Text = "";
            }
        }

        private void textBox1_LostFocus(object sender, RoutedEventArgs e)
        {
            if (textBox1.Text.Equals(""))
            {
                textBox1.Text = "enter username..";
            }
        }

        private void ApplicationBarIconButton_Click(object sender, EventArgs e)
        {
            if ((textBox1.Text.Equals("enter username.."))||(textBox1.Text.Equals("")))
            {
                textBox1.Text = ""; MessageBox.Show("Please type a proper username."); return;
            }
            progress.Visibility = System.Windows.Visibility.Visible;
            WebClient twitter = new WebClient();
            try
            {

                twitter.DownloadStringCompleted += new DownloadStringCompletedEventHandler(twitter_DownloadStringCompleted);
                twitter.DownloadStringAsync(new Uri("http://api.twitter.com/1/statuses/user_timeline.xml?screen_name=" + textBox1.Text));
            }
            catch
            {
                MessageBox.Show("Please check your connection");
            }
        }

        

        private void ApplicationBarIconButton_Click_2(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/about.xaml", UriKind.Relative));
        }

        private void ApplicationBarIconButton_Click_1(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/addid.xaml", UriKind.Relative));
        }
        
    }

    public class TwitterItem
    {
        public string Username { get; set; }
        public string Message { get; set; }
        public string ImageSource { get; set; }
        public string Date { get; set; }
        public string followers { get; set; }
        public string description { get; set; }
        public string URL { get; set; }
        public string Retweets { get; set; }
        public string Tweets_count { get; set; }
    }

    
}