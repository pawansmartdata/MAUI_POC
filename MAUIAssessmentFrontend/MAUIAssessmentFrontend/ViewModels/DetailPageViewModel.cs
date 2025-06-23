using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using MAUIAssessmentFrontend.Models;
using MAUIAssessmentFrontend.Services.Interfaces;
using MAUIAssessmentFrontend.Views;
using System;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MAUIAssessmentFrontend.ViewModels
{
    public class DetailPageViewModel : INotifyPropertyChanged
    {
        public ItemResponseDto Item { get; set; }
        public ICommand EditCommand { get; }
        public ICommand DeleteCommand { get; }


        private int _id;
        public int Id { get => _id; set { _id = value; OnPropertyChanged(nameof(Id)); } }

        private string _name;
        public string Name { get => _name; set { _name = value; OnPropertyChanged(nameof(Name)); } }

        private string _description;
        public string Description { get => _description; set { _description = value; OnPropertyChanged(nameof(Description)); } }

        private string _itemImageUrl;
        public string ItemImageUrl { get => _itemImageUrl; set { _itemImageUrl = value; OnPropertyChanged(nameof(ItemImageUrl)); } }

        private double _latitude;
        public double Latitude { get => _latitude; set { _latitude = value; OnPropertyChanged(nameof(Latitude)); } }

        private double _longitude;
        public double Longitude { get => _longitude; set { _longitude = value; OnPropertyChanged(nameof(Longitude)); } }

        public string MapHtml { get; set; }

        private readonly IItemService _itemService;

        public DetailPageViewModel(IItemService itemService)
        {
            _itemService = itemService;
            EditCommand = new Command(OnEdit);
            DeleteCommand = new Command(async () => await OnDelete());
            
           
        }

        //public async Task InitializeAsync(int itemId)
        //{
        //    var item = await _itemService.GetItemByIdAsync(itemId);

        //    Id = item.Id;
        //    Name = item.Name;
        //    Description = item.Description;
        //    ItemImageUrl = item.ItemImageUrl;
        //    Latitude = item.Latitude;
        //    Longitude = item.Longitude;

        //    MapHtml = $@"
        //<iframe width='100%' height='100%' frameborder='0' style='border:0'
        //src='https://www.google.com/maps/embed/v1/view?key=YOUR_KEY
        //&center={Latitude},{Longitude}&zoom=14' allowfullscreen></iframe>";

        //    OnPropertyChanged(nameof(MapHtml));
        //}


        private async void OnEdit()
        {
            if (Item != null)
            {
                await Shell.Current.GoToAsync($"{nameof(EditItemPage)}?itemId={Item.Id}");
            }
        }

        //private async void OnEdit()
        //{
        //    if (Item != null)
        //        await Shell.Current.GoToAsync($"{nameof(EditItemPage)}?itemId={Item.Id}");
        //}

        private async Task OnDelete()
        {
            var confirm = await Application.Current.MainPage.DisplayAlert(
                "Delete",
                "Are you sure you want to delete this item?",
                "Yes", "No"
            );

            if (!confirm) return;

            var success = await _itemService.DeleteItemAsync(Item.Id);

            if (success)
            {
                await Toast.Make("Deleted Successful!", ToastDuration.Short).Show();
                await GoBack();
            }
            else
            {
                await Toast.Make("Deleted Unsuccessful!", ToastDuration.Short).Show();
            }
        }

        public async Task InitializeAsync(int itemId)
        {
            // Item = await _itemService.GetItemByIdAsync(4);
            Id = itemId;
            Item = await _itemService.GetItemByIdAsync(itemId);
            Console.WriteLine("GetByIdResp"+Item);
            MapHtml = $@"
                <iframe width='100%' height='100%' frameborder='0' style='border:0'
                src='https://www.google.com/maps/embed/v1/view?key=YOUR_KEY
                &center={Item.Latitude},{Item.Longitude}&zoom=14' allowfullscreen>
                </iframe>";

            OnPropertyChanged(nameof(Item));
            OnPropertyChanged(nameof(MapHtml));
        }

        // ✅ Fix: INotifyPropertyChanged implementation
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        private async Task GoBack()
        {
            await Shell.Current.GoToAsync("//MainPage"); 
        }

    }
}
