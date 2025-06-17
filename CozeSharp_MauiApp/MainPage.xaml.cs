using CozeSharp_MauiApp.PageModels;
using CozeSharp_MauiApp.Services;
using System.Threading.Tasks;

namespace CozeSharp_MauiApp
{
    public partial class MainPage : ContentPage
    {
        int count = 0;

        private readonly CozeAgnetService _cozeAgnetService;

        public MainPage(CozeAgnetService cozeAgnetService)
        {
            InitializeComponent();
            _cozeAgnetService = cozeAgnetService;
            BindingContext = _cozeAgnetService;
        }

        private void ImageButton_Pressed(object sender, EventArgs e)
        {
            _ = Task.Run(async () =>
            {
                await _cozeAgnetService.Agent.StartRecording();
            });
        }

        private void ImageButton_Released(object sender, EventArgs e)
        {
            _ = Task.Run(async () =>
            {
                await _cozeAgnetService.Agent.StopRecording();
            });
        }
    }
}
