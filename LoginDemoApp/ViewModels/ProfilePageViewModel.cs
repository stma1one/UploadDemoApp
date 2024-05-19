using LoginDemoApp.Models;
using LoginDemoApp.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace LoginDemoApp.ViewModels
{
    public class ProfilePageViewModel : ViewModelBase
    {
        private LoginDemoWebAPIProxy service;
     

        private string profileImage;

        public string ProfileImage { get => profileImage; set {profileImage = value; OnPropertyChanged(); } }

        public string Name { get { if (service != null&&service.LoggedInUser!=null) return service.LoggedInUser.Name; return null; } }
        
        public string Email { get { if (service != null&&service.LoggedInUser!=null) return service.LoggedInUser.Email; return null; } }
    

        
        public ICommand UploadImageCommand {  get; protected set; }
        public ProfilePageViewModel(LoginDemoWebAPIProxy proxy)
        {
            service = proxy;
           
            UploadImageCommand = new Command(async () => await UploadImage());
             if(service.LoggedInUser.Image!=null)
            {
                ProfileImage = $"{LoginDemoWebAPIProxy.ImageUrl}{service.LoggedInUser.Image}";
            }
        }

        private async Task UploadImage()
        {
            if (MediaPicker.Default.IsCaptureSupported)
            {
                FileResult photo = await MediaPicker.Default.CapturePhotoAsync();

                if (photo != null)
                {
                    // save the file into local storage

                    //string localFilePath = Path.Combine(FileSystem.CacheDirectory, photo.FileName);

                    //using Stream sourceStream = await photo.OpenReadAsync();
                    //using FileStream localFileStream = File.OpenWrite(localFilePath);

                    //await sourceStream.CopyToAsync(localFileStream);
               string filename=   await UploadProfileImage(photo);
                    service.LoggedInUser.Image = filename;
                    ProfileImage = null; 
                    
                    ProfileImage = $"{LoginDemoWebAPIProxy.ImageUrl}{service.LoggedInUser.Image}";
                }

            }
        }

        private async Task<string> UploadProfileImage(FileResult photo)
        {
            try
            {
               string filename= await service.UploadImage(photo);
                return filename;
            }
            catch(Exception ex) {  }
            return null;
        }
    }
}
