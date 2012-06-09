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
using System.Xml.Linq;
using System.IO;
using System.IO.IsolatedStorage;

namespace tweet_feed
{
    public partial class addid : UserControl
    {
        public addid()
        {
            InitializeComponent();
        }

        private void Addid_GotFocus(object sender, RoutedEventArgs e)
        {
            if (Addid.Text.Equals("add handle.."))
            {
                Addid.Text = "";
            }
        }

        private void Addid_LostFocus(object sender, RoutedEventArgs e)
        {
            if (Addid.Text.Equals(""))
            {
                Addid.Text = "add handle..";
            }
        }

        private void addimage_Tap(object sender, GestureEventArgs e)
        {
            if ((Addid.Text.Equals("add handle..")) || (Addid.Text.Equals("")))
            {
                Addid.Text = ""; MessageBox.Show("Please type a proper handle."); return;
            }
            
            WebClient twitter = new WebClient();
            try
            {
                progress.Visibility = System.Windows.Visibility.Visible;
                twitter.DownloadStringCompleted += new DownloadStringCompletedEventHandler(twitter_DownloadStringCompleted);
                twitter.DownloadStringAsync(new Uri("http://api.twitter.com/1/statuses/user_timeline.xml?screen_name=" + Addid.Text));

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
                MessageBox.Show("Unable to parse. Check handle or network connection.");  return;
            }


            try
            {
                XElement xmltweets = XElement.Parse(e.Result);
                saveimage.Visibility = System.Windows.Visibility.Visible;
                progress.Visibility = System.Windows.Visibility.Collapsed;
            }
            catch
            {
                MessageBox.Show("Unable to parse. Check handle");
            }

        }

        private void saveimage_Tap(object sender, GestureEventArgs e)
        {
            IsolatedStorageFile myFile = IsolatedStorageFile.GetUserStoreForApplication();
            StreamWriter writer = new StreamWriter(new IsolatedStorageFileStream("settings.txt",FileMode.OpenOrCreate,FileAccess.Write,FileShare.Write,myFile));
            writer.Write(Addid.Text+",");
            writer.Close();
        }


    }
}
