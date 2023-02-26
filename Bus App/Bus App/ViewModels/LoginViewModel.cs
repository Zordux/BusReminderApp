using Bus_App.Views;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using System.Security.Cryptography;

namespace Bus_App.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        public Command LoginCommand { get; }

        
        public LoginViewModel()
        {
            LoginCommand = new Command(OnLoginClicked);
        }

        private async void OnLoginClicked(object obj)
        {
            //changes the page that is shown after the button is pushed 
            await Shell.Current.GoToAsync($"//{nameof(AboutPage)}");
        }
    }
}
