using dotMorten.Xamarin.Forms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Twilio.TwiML;
using Twilio.Types;
using Twilio.Rest.Api.V2010.Account;
using Twilio;
using MySqlConnector;

namespace Bus_App.Views
{
    public partial class AboutPage : ContentPage
    {
        //Setting A list For All The Addresses
        private List<string> AddressList;
        string finalAddress = "";
        string studentNum = "";
        //Setting mysql connection as a variable 
        MySqlConnection connection = new MySqlConnection(" datasource=DataBaseLink;username=UsernameHere;password=Passwordhere");//<---- data is not shown because its private info
        MySqlCommand command;
        MySqlDataReader mdr;

        public AboutPage()
        {
            InitializeComponent();
            GetStudentAddress();   
        }
        //Methods For The Suggestion Box
        private void GetStudentAddress()
        {
            //Uses info from Data\StudentDataBase.txt to be displayed in the suggestion box
            using(var address = typeof(AboutPage).Assembly.GetManifestResourceStream("BusApp.Data.StudentDataBase.txt"))
            {
                AddressList = new StreamReader(address).ReadToEnd().Split('\n').Select(t=> t.Trim()).ToList();
            }
        }
        private void Address_TextChanged(object sender, dotMorten.Xamarin.Forms.AutoSuggestBoxTextChangedEventArgs e)
        {
            AutoSuggestBox input = (AutoSuggestBox)sender;  
            input.ItemsSource = GetSuggestions(input.Text);
        }
        private List<string> GetSuggestions(string input)
        {
            return string.IsNullOrWhiteSpace(input) ? null : AddressList.Where(a=> a.StartsWith(input, StringComparison.InvariantCultureIgnoreCase)).ToList();
        }
        //Uses Twilio Api To Send Text Message To The Student If They Have A Phone # With There address
        private void Button_Clicked(object sender, EventArgs e)
        {
            finalAddress = "\""+Address.Text.ToString()+"\"";//Used for formate purposes
            //Connects to sql server
            connection.Open();
            string selectQuery = $"SELECT * FROM busaddress.Addresses WHERE Address={finalAddress}";
            command = new MySqlCommand(selectQuery, connection);
            mdr = command.ExecuteReader();

            if (mdr.Read())
            {
                //Grabs the phonenumber that belongs with the inputed address from suggestion Box
                studentNum = mdr.GetString("PhoneNumber");
                sendSms();
                connection.Close(); 
            }
            else
            {
                //Failed to find the inputed number
                DisplayAlert("Alert", "This Address Does Not Have A Number. Please Continue To Next Stop", "OK");
                connection.Close();
            }
        }
        //Twilio api method to send message
        public void sendSms()
        {
            //Twilio api
            var accountSid = "";//<--- Empty because the string data is private info
            var authToken = "";//<--- Empty because the string data is private info
            TwilioClient.Init(accountSid, authToken);

            var to = new PhoneNumber(studentNum);
            var from = new PhoneNumber("");//<--- Empty because the string data is private info
            //Data formated to send the sms message
            var message = MessageResource.Create(
             to: to,
             from: from,
             body: "Your next for the bus stop. Please be ready for your pick up");
            DisplayAlert("Alert", "Message Sent", "OK");
        }

    } 
}