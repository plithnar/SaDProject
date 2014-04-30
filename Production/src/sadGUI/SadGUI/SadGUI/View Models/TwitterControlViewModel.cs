using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TweetSharp;

namespace SadGUI.View_Models
{
    public class TwitterControlViewModel : ViewModelBase
    {
        private string m_tweet = "";

        private const int MaxTweetSize  = 140;

        //TODO: Temporary key and sec. currently linked to plithnarc
        private const string cKey       = "MDxLkSYA2qtBAHsYl7EMwPnrc";
        private const string cSec       = "5e6iGylpBhH3kf33b5uSaM7VRKDEa0a0SFHSNZ91ealAgp0EMG";
        private const string tokn       = "2434659338-RoUGA69naFCdCfaqhcUnNf5HwaNK5JlQnSqnsLr";
        private const string tSec       = "ckOYDjszSzrHMdcW1emNRkxp6UO0o0VNRwtRqUFjIZBYd";

        private TwitterService m_service;

        private static TwitterControlViewModel m_twitter;

        public string TweetText
        {
            get 
            { 
                return m_tweet;
            }
            private set 
            { 
                m_tweet = value; 
                OnPropertyChanged("TweetText"); 
            }
        }

        public static TwitterControlViewModel Twitter
        {
            get
            {
                if(m_twitter == null)
                {
                    m_twitter = new TwitterControlViewModel();
                }
                return m_twitter;
            }
        }

        public void Tweet(string value)
        {
            TweetText = value;
            //m_service.SendTweet(new SendTweetOptions {Status = TweetText});
        }

        private TwitterControlViewModel()
        {
            TweetText = "";
            m_service = new TwitterService(cKey, cSec, tokn, tSec);
        }
    }
}
