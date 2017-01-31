using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace todolist
{
    public sealed partial class MessageDialog : UserControl
    {
        private TaskCompletionSource<bool> taskCompletionSource;
        private string textMessage;

        public MessageDialog(string label)
        {
            Label = label;
            this.InitializeComponent();
        }

        public Task<bool> ShowAsync()
        {
            InitFields();
            m_Popup.IsOpen = true;
            taskCompletionSource = new TaskCompletionSource<bool>();
            return taskCompletionSource.Task;
        }

        public void InitFields()
        {
            m_Rect1.Height = Window.Current.Bounds.Height;
            m_Rect1.Width = Window.Current.Bounds.Width;
            m_Rect2.Width = Window.Current.Bounds.Width;
            m_TextBlock.Text = Label;
        }

        public string Label
        {
            get { return textMessage; }
            set { textMessage = value; }
        }

        private void OkClicked(object sender, RoutedEventArgs e)
        {
            taskCompletionSource.SetResult(true);
            m_Popup.IsOpen = false;
        }

        private void CancelClicked(object sender, RoutedEventArgs e)
        {
            taskCompletionSource.SetResult(false);
            m_Popup.IsOpen = false;
        }
    }
}
