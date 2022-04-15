using Android.Webkit;

namespace SimplyNews
{
    public class CustomWebViewClient : WebViewClient
    {
        public override bool ShouldOverrideUrlLoading(WebView view, IWebResourceRequest request)
        {
            return false;
        }
    }
}