using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;

namespace todolist
{
    public sealed partial class EditPage : Page
    {
        private string name;
        private string date;
        private string time;
        private string content;
        private int id;

        public EditPage()
        {
            this.InitializeComponent();
            animationForEditPage();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            Item itemSend = e.Parameter as Item;
            if (itemSend != null)
            {
                m_title.Text = name = itemSend.name;
                id = itemSend.id;
                if (itemSend.date != null)
                {
                    m_calendarDatePicker.Date = DateTime.Parse(itemSend.date);
                }
                if (itemSend.timer != null)
                {
                    m_timePicker.Time = TimeSpan.Parse(itemSend.timer);
                }
                if (itemSend.content != null)
                {
                    m_txtBoxLineEdit.Text = itemSend.content;
                }
            }
        }

        private void calendarDatePicker_DateChanged(CalendarDatePicker sender, CalendarDatePickerDateChangedEventArgs args)
        {
            if (args.NewDate != null)
            {
                this.date = args.NewDate.Value.ToString();
            }
        }

        private void timePicker_TimeChanged(object sender, TimePickerValueChangedEventArgs args)
        {
            if (args.NewTime != null)
            {
                this.time = args.NewTime.ToString();
            }
        }

        private void buttonSaveIsClicked(object sender, RoutedEventArgs e)
        {
            if (!m_txtBoxLineEdit.Text.Trim().Equals(string.Empty))
            {
                this.content = m_txtBoxLineEdit.Text;
                m_txtBoxLineEdit.Text = "";
            }
            this.Frame.Navigate(typeof(MainPage), new Item(name, id, date, time, content));
        }

        private void buttonBackIsClicked(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(MainPage), new Item(name, id, date, time, content));
        }

        private async void DisplayDate(string SelectedDate)
        {
            ContentDialog noWifiDialog = new ContentDialog()
            {
                Title = "Selected date :",
                Content = SelectedDate,
                PrimaryButtonText = "Ok"
            };

            noWifiDialog.Margin = new Thickness(0, 20, 0, 0);
            await noWifiDialog.ShowAsync();
        }
        private void animationForEditPage()
        {
            TransitionCollection collection = new TransitionCollection();
            NavigationThemeTransition theme = new NavigationThemeTransition();
            theme.DefaultNavigationTransitionInfo = new CommonNavigationTransitionInfo();
            collection.Add(theme);
            this.Transitions = collection;
        }
    }
}

