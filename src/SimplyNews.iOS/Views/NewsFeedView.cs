using System;
using MvvmCross.Platforms.Ios.Views;
using SimplyNews.Core.ViewModels;
using WebKit;
using UIKit;
using CoreGraphics;
using ObjCRuntime;

namespace SimplyNews.iOS.Views
{
    public class NewsFeedView : MvxViewController<NewsFeedViewModel>, IUIGestureRecognizerDelegate
    {
        WKWebView webView;
        UIPickerView categoryPickerView;
        CategoryPickerModel categoryPickerModel;
        UITextField categoryTextField;

        private string _html;
        public string Html
        {
            get => _html;
            set
            {
                _html = value;
                webView.LoadHtmlString(_html, null);
            }
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            View.BackgroundColor = UIColor.White;
            View.UserInteractionEnabled = true;
            try
            {
                var safeLayout = View.SafeAreaLayoutGuide;
                var frame = safeLayout.LayoutFrame;
                                
                CreateCategoryTextField();
                NavigationItem.TitleView = categoryTextField;

                categoryPickerView = new UIPickerView();
                categoryPickerModel = new CategoryPickerModel();
                categoryPickerModel.ValueChanged += CategoryPickerModel_ValueChanged;
                categoryPickerView.Model = categoryPickerModel;

                var wvPrefs = new WKPreferences
                {
                    JavaScriptEnabled = true
                };
                var wvConfig = new WKWebViewConfiguration
                {
                    Preferences = wvPrefs
                };

                webView = new WKWebView(frame, wvConfig)
                {
                    UserInteractionEnabled = true,
                    AllowsBackForwardNavigationGestures = true
                };
                Add(webView);

                var categoriesToolBar = new UIToolbar(new CGRect(0, 0, 300, 30))
                {
                    UserInteractionEnabled = true
                };
                UIBarButtonItem doneAccountsButton = new UIBarButtonItem()
                {
                    Title = "Done",
                    Style = UIBarButtonItemStyle.Done
                };
                doneAccountsButton.Clicked += (sender, e) => 
                {
                    categoryTextField.EndEditing(true);
                    categoryTextField.ResignFirstResponder();
                };

                UIBarButtonItem flexibleSpace = new UIBarButtonItem(UIBarButtonSystemItem.FlexibleSpace);
                categoriesToolBar.SetItems(new UIBarButtonItem[] { flexibleSpace, doneAccountsButton, flexibleSpace }, false);

                categoryTextField.InputView = categoryPickerView;
                categoryTextField.InputAccessoryView = categoriesToolBar;

                UIBarButtonItem backButton = new UIBarButtonItem()
                {
                    Title = "SimplyNews",
                    Style = UIBarButtonItemStyle.Plain,
                    TintColor = UIColor.SystemBlueColor
                };
                
                NavigationItem.LeftBarButtonItem = backButton;

                UITextView waitMessageView = new UITextView()
                {
                    BackgroundColor = UIColor.SystemBlueColor,
                    TextColor = UIColor.White,
                    Text = "Loading content...",
                    Font = UIFont.SystemFontOfSize(17f),
                    TextAlignment = UITextAlignment.Center
                };
                waitMessageView.Layer.CornerRadius = 3;
                waitMessageView.SizeToFit();
                Add(waitMessageView);

                nfloat centerX = frame.Width / 2 - waitMessageView.Frame.Width / 2;
                nfloat centerY = frame.Height / 2 - waitMessageView.Frame.Height / 2;
                var center = waitMessageView.Frame;
                center.X = centerX;
                center.Y = centerY;
                waitMessageView.Frame = center;

                var leftSwipeGesture = new UISwipeGestureRecognizer { Delegate = this };
                leftSwipeGesture.Direction = UISwipeGestureRecognizerDirection.Left;
                leftSwipeGesture.AddTarget(() => NavigateBackOrForward(leftSwipeGesture));
                webView.AddGestureRecognizer(leftSwipeGesture);

                var rightSwipeGesture = new UISwipeGestureRecognizer { Delegate = this };
                rightSwipeGesture.Direction = UISwipeGestureRecognizerDirection.Right;
                rightSwipeGesture.AddTarget(() => NavigateBackOrForward(rightSwipeGesture));
                webView.AddGestureRecognizer(rightSwipeGesture);

                var set = CreateBindingSet();
                set.Bind(categoryPickerModel).For(v => v.Items).To(vm => vm.NewsCategories).OneTime();
                set.Bind(categoryTextField).To(vm => vm.SelectedCategory);
                set.Bind(doneAccountsButton).To(vm => vm.CategorySelectedCommand);
                set.Bind(this).For(v => v.Html).To(vm => vm.NewsContent);
                set.Bind(waitMessageView).For(v => v.Hidden).To(vm => vm.IsNotBusy);
                set.Apply();

                ViewModel.CategorySelectedCommand.Execute();
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        private void CreateCategoryTextField()
        {
            categoryTextField = new UITextField(new CGRect(0, 0, 0, NavigationController.NavigationBar.Frame.Height - 5))
            {
                Font = UIFont.SystemFontOfSize(17f),
                TextAlignment = UITextAlignment.Center,
                TextColor = UIColor.White,
                TintColor = UIColor.Clear,
                BackgroundColor = UIColor.SystemGreenColor
            };
            categoryTextField.Layer.CornerRadius = 4;
            UIView paddingRect = new UIView(new CGRect(0, 0, 5, 20));
            categoryTextField.LeftViewMode = UITextFieldViewMode.Always;
            categoryTextField.RightViewMode = UITextFieldViewMode.Always;
            categoryTextField.LeftView = paddingRect;
            categoryTextField.RightView = paddingRect;
            categoryTextField.UserInteractionEnabled = true;
            categoryTextField.AddGestureRecognizer(new UITapGestureRecognizer((action) => { categoryTextField.BecomeFirstResponder(); }));
            categoryTextField.ShouldChangeCharacters = (textField, range, replacementString) => { return false; };
            categoryTextField.TranslatesAutoresizingMaskIntoConstraints = false;
        }

        private void CategoryPickerModel_ValueChanged(object sender, EventArgs e)
        {
            categoryTextField.Text = categoryPickerModel.SelectedValue;
            ViewModel.SelectedCategory = categoryPickerModel.SelectedValue;
        }

        private void NavigateBackOrForward(UISwipeGestureRecognizer swipeGestureRecognizer)
        {
            if (swipeGestureRecognizer.Direction == UISwipeGestureRecognizerDirection.Right)
            {
                if (webView.CanGoBack)
                    webView.GoBack();
            }

            if (swipeGestureRecognizer.Direction == UISwipeGestureRecognizerDirection.Left)
            {
                if (webView.CanGoForward)
                    webView.GoForward();
            }

        }
    }
}
