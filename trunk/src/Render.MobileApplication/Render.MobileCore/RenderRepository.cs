using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Reactive.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Akavache;
using AutoMapper;
using Humanizer;
using Render.API.Client;
using Punchclock;
using Splat;

namespace Render.MobileCore
{
    public class RenderRepository
    {
        private const string ClassName = "RenderRepository";

		private readonly OperationQueue downloadQueue = new OperationQueue(5);
		private readonly HttpClient downloadImageClient = new HttpClient();

		//protected readonly DateTimeOffset CacheTimeSpanShort = DateTimeOffset.Now.AddMinutes(10.0d);
		//protected readonly DateTimeOffset CacheTimeSpanLong = DateTimeOffset.Now.AddMinutes(120.0d);

		private const string 
			AccountKey = "Account", 
			EstimateKey = "Estimate_{0}", EstimateListKey = "EstimateList", EstimateContactListKey = "EstimateContactList",
			ContactListKey = "ContactList", 
			VehicleKey = "Vehicle_{0}",
			PhotoCategoriesKey = "PhotoCategories", RateMatriciesKey = "RateMatricies";

        public bool HasAuthentication
        {
            get
            {
                var storedUsername = Settings.Username;
                var storedPassword = Settings.Password;

                return !string.IsNullOrEmpty(storedUsername) && !string.IsNullOrEmpty(storedPassword);
            }
        }

        private static RenderRepository _current;
        public static RenderRepository Current
        {
            get { return _current ?? (_current = new RenderRepository()); }
        }
			
        private RenderRepository()
        {
			BlobCache.ApplicationName = ClassName;
			BlobCache.EnsureInitialized();

			/*
			Mapper.CreateMap<Models.AccountModel, ViewModels.Settings.Profile> ()
				.ForMember(dest => dest.AccountId, opt => opt.MapFrom(src => src.Id))
				.ForMember(dest => dest.StreetAddress1, opt => opt.MapFrom(src => src.Address1))
				.ForMember(dest => dest.StreetAddress2, opt => opt.MapFrom(src => src.Address2));
			*/
        }

		public void MapToUpdate<T>(T modelIn, T modelOut){
			Mapper.Map<T, T>(modelIn, modelOut);
		}

		public async Task Shutdown(){
			await BlobCache.Shutdown ();
		}

		/*
        public async Task<Boolean> AuthenticateUserAsync(string username, string password, CancellationToken cancellationToken = default (CancellationToken))
        {
			using (var apiClient = await BuildApiClientAsync().ConfigureAwait(false))
            {
                await apiClient.InitializeClientAsync(username, password).ConfigureAwait(false);

                var response = await apiClient.Login(cancellationToken).ConfigureAwait(false);

                //We will only wait for 5 seconds for a login
//				if (await Task.WhenAny(loginTask, Task.Delay(Constants.Api.ApiDefaultTimeout, cancellationToken)).ConfigureAwait(false) != loginTask)
//                    return false;

//				var response = await loginTask.ConfigureAwait(false);
				await LogApiExceptionsAsync(response).ConfigureAwait(false);

                if (!response.Success) return false;

				await BlobCache.LocalMachine.InsertObject (AccountKey, response.Result);

                Settings.Username = username;
                Settings.PasswordIsTemporary = false;
                Settings.Password = password;

                return response.Success;
            }
        }
        */

        private async Task<APIClient> BuildApiClientAsync()
        {
			return await Task.Run (async () => {
				var apiClient = new APIClient(Constants.Api.ApiBase);

				var storedUsername = Settings.Username;
				var storedPassword = Settings.Password;

				if (!string.IsNullOrEmpty(storedUsername) && !string.IsNullOrEmpty(storedPassword))
					await apiClient.InitializeClientAsync(storedUsername, storedPassword).ConfigureAwait(false);

				return apiClient;
			}).ConfigureAwait (false);


        }

    }
}
