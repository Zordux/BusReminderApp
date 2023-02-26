using Bus_App.ViewModels;
using BusApp.Data;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Twilio.Rest.Chat.V2.Service.User;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Bus_App.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginPage : ContentPage
    {
        //Setting mysql connection as a variable 
        MySqlConnection connection = new MySqlConnection(" datasource=DataBaseLink;username=UsernameHere;password=Passwordhere");//<---- data is not shown because its private info
        MySqlCommand command;
        MySqlDataReader mdr;

        string finalUsername;
        public LoginPage()
        {
            InitializeComponent();
            this.BindingContext = new LoginViewModel();
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            try
            {   //Formats username inputed by text box
                finalUsername = "\"" + Username.Text.ToString() + "\"";
                try
                {
                   connection.Open();
                }
                catch
                {
                    //Server error
                    await DisplayAlert("Alert", "Could Not Connect To Server Repot This.", "OK");
                }
                //Connects to sql server
                string selectQuery = $"SELECT * FROM busaddress.busLogin WHERE username={finalUsername}";
                command = new MySqlCommand(selectQuery, connection);
                mdr = command.ExecuteReader();

                if (mdr.Read())//(mdr.Read())
                {
                    //Encrypts password inputed and then matches it with encrypted password on server
                    //Enctpyting method can be found at Data\Crypting.cs
                    if (Crypting.ToSha256(Password.Text.ToString()) == mdr.GetString("password"))
                    {
                        //User name and encrypted password matches with server
                        await DisplayAlert("Alert", "Login confirmed.", "OK");
                        connection.Close();
                        await Shell.Current.GoToAsync($"//{nameof(AboutPage)}");
                    }
                    else
                    {
                        //Wrong password for username
                        await DisplayAlert("Alert", "Worng Username Or Password.", "OK");
                        connection.Close();
                    }
                }
                else
                {
                    //User name was not on server
                    await DisplayAlert("Alert", "No Username Found", "OK");
                    connection.Close();
                }
            }
            catch
            {
                //nothing was inputed in text box
                await DisplayAlert("Alert", "A Text Box is Empty", "OK");
            }
            
            
        }
 
    }
}