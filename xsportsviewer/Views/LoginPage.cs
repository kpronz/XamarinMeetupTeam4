using System;
using Newtonsoft.Json;
using RestSharp;
using Xamarin.Forms;

namespace xsportsviewer
{
	public class LoginPage : ContentPage
	{
		public LoginPage()
		{
			var title = new Label
			{
				Text = "Login",
				FontSize = Device.GetNamedSize(NamedSize.Large, typeof(Label)),
				HorizontalOptions = LayoutOptions.CenterAndExpand,
			};

			var email = new Entry
			{
				Placeholder = "E-Mail",
			};

			var password = new Entry
			{
				Placeholder = "Password",
				IsPassword = true
			};

			var login = new Button
			{
				Text = "Login"
			};

			login.Clicked += (sender, e) =>
			{				
				Login(email.Text, password.Text);
			};

			Content = new StackLayout
			{
				Padding = 30,
				Spacing = 10,
				Children = { title, email, password, login }
			};
		}

		public void Login(string username, string password)
		{
			// We are using the RestSharp library which provides many useful
			// methods and helpers when dealing with REST.
			// We first create the request and add the necessary parameters
			var client = new RestClient("https://{YOUR-AUTH0-DOMAIN}.auth0.com");
			var request = new RestRequest("oauth/ro", Method.POST);
			request.AddParameter("client_id", "{YOUR-AUTH0-CLIENT-ID");
			request.AddParameter("username", username);
			request.AddParameter("password", password);
			request.AddParameter("connection", "{YOUR-CONNECTION-NAME-FOR-USERNAME-PASSWORD-AUTH}");
			request.AddParameter("grant_type", "password");
			request.AddParameter("scope", "openid");

			// We execute the request and capture the response
			// in a variable called `response`
			IRestResponse response = client.Execute(request);

			// Using the Newtonsoft.Json library we deserialaize the string into an object,
			// we have created a LoginToken class that will capture the keys we need
			LoginToken token = JsonConvert.DeserializeObject<LoginToken>(response.Content);

			// We check to see if we received an `id_token` and if we did make a secondary call
			// to get the user data. If we did not receive an `id_token` we can safely assume
			// that the authentication failed so we display an error message telling the user
			// to try again.
			if (token.id_token != null)
			{
				Application.Current.Properties["id_token"] = token.id_token;
				Application.Current.Properties["access_token"] = token.access_token;
				GetUserData(token.id_token);
			}
			else {
				DisplayAlert("Oh No!", "It's seems that you have entered an incorrect email or password. Please try again.", "OK");
			};
		}

		public void GetUserData(string token)
		{
			var client = new RestClient("https://{YOUR-AUTH0-DOMAIN}.auth0.com");
			var request = new RestRequest("tokeninfo", Method.GET);
			request.AddParameter("id_token", token);


			IRestResponse response = client.Execute(request);

			User user = JsonConvert.DeserializeObject<User>(response.Content);

			// Once the call executes, we capture the user data in the
			// `Application.Current` namespace which is globally available in Xamarin
			Application.Current.Properties["email"] = user.email;
			Application.Current.Properties["picture"] = user.picture;

			// Finally, we navigate the user the the Orders page
			Navigation.PushModalAsync(new HomePage());
		}

		public class LoginToken
		{
			public string id_token { get; set; }
			public string access_token { get; set; }
			public string token_type { get; set; }
		}

		public class User
		{
			public string name { get; set; }
			public string picture { get; set; }
			public string email { get; set; }
		}

	}
}


