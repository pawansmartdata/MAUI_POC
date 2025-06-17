namespace MAUIAssessnentFrontend
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private async void OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem is MAUIAssessnentFrontend.Models.ItemDto selected)
            {
                await Navigation.PushAsync(new Views.DetailPage(selected.Id));
            }
        }
    }

}
