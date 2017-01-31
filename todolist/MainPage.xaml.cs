using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;

namespace todolist
{
    public sealed partial class MainPage : Page
    {
        private static int lastIdSelected = -1;
        private LocalDatabase db = new LocalDatabase();


        public MainPage()
        {
            this.InitializeComponent();
            db.createDb();
            refreshView();
            animationForMainPage();
        }


        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            Item item = e.Parameter as Item;
            if (item != null)
            {
                db.updateItem(item.id, item.date, item.timer, item.content);
                refreshView();
            }
        }


        private void clickOnCell(object sender, RoutedEventArgs e)
        {
            lastIdSelected = m_ListBox.SelectedIndex + 1;
            Item item = db.getItem(lastIdSelected);
            if (item != null)
            {
                Debug.WriteLine("CLICKED ON CELL : " + lastIdSelected);
                db.updateItem(item);
            }
        }


        private void addOnClick(object sender, RoutedEventArgs e)
        {
            if (!m_txtBoxLineEdit.Text.Trim().Equals(string.Empty))
            {
                Item item = new Item(m_txtBoxLineEdit.Text.Trim());
                db.addItem(item);
                this.Frame.Navigate(typeof(EditPage), item);
                m_txtBoxLineEdit.Text = "";
            }
        }


        private async void removeOnClick(object sender, RoutedEventArgs e)
        {
            List<Item> list = m_ListBox.Items.OfType<Item>().ToList();
            int countIsCheckedIntoList = 0;
            string messageString = null;

            foreach (var tmp in list)
            {               
                if (tmp.isChecked)
                {
                    countIsCheckedIntoList++;
                }
            }

            if (countIsCheckedIntoList > 1)
            {
                messageString = "Do you want to delete those items ?";
            }
            else if (countIsCheckedIntoList == 1)
            {
                messageString = "Do you want to delete this item ?";
            }

            if (messageString != null)
            {
                MessageDialog msg = new MessageDialog(messageString);
                bool res = await msg.ShowAsync();
                if (res == true)
                {
                    foreach (var item in list)
                    {
                        if (item.isChecked)
                        {
                            db.deleteItem(item.id);
                        }
                    }
                    refreshView();
                }
            }
        }


        private void editOnClick(object sender, RoutedEventArgs e)
        {
            Item item = db.getItem(lastIdSelected);
            if (item != null)
            {
                this.Frame.Navigate(typeof(EditPage), item);
            }
        }


        private async void deleteOnClick(object sender, RoutedEventArgs e)
        {
            MessageDialog msg = new MessageDialog("This action will delete all currents items.\nContinue ?");
            bool res = await msg.ShowAsync();
            if (res == true)
            {
                db.deleteAllItem();
                refreshView();
            }
        }


        private void selectAllOnClick(object sender, RoutedEventArgs e)
        {
            db.un_selectAllItem();
            refreshView();
        }


        private void settingsOnClick(Object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(SettingsPage), null);
        }


        private void animationForMainPage()
        {
            TransitionCollection collection = new TransitionCollection();
            NavigationThemeTransition theme = new NavigationThemeTransition();
            theme.DefaultNavigationTransitionInfo = new ContinuumNavigationTransitionInfo();
            collection.Add(theme);
            this.Transitions = collection;
        }


        private void refreshView()
        {
            m_ListBox.ItemsSource = db.getObservableItem();
        }
    }
}

