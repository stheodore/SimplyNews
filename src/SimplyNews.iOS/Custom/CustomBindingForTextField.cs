using Foundation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UIKit;

namespace SimplyNews.iOS.Custom
{
    class CustomBindingForTextField
    {
    }
    public class CustomBindingForTextField : MvxPropertyInfoTargetBinding<MyView>
    {
        // used to figure out whether a subscription to MyPropertyChanged
        // has been made
        private bool _subscribed;

        public override MvxBindingMode DefaultMode => MvxBindingMode.TwoWay;

        public MyViewMyPropertyTargetBinding(object target, PropertyInfo targetPropertyInfo)
            : base(target, targetPropertyInfo)
        {
        }

        // describes how to set MyProperty on MyView
        protected override void SetValueImpl(object target, object value)
        {
            var view = target as MyView;
            if (view == null) return;

            view.MyProperty = (string)value;
        }

        // is called when we are ready to listen for change events
        public override void SubscribeToEvents()
        {
            var myView = View;
            if (myView == null)
            {
                MvxBindingTrace.Trace(MvxTraceLevel.Error, "Error - MyView is null in MyViewMyPropertyTargetBinding");
                return;
            }

            _subscribed = true;
            myView.MyPropertyChanged += HandleMyPropertyChanged;
        }

        private void HandleMyPropertyChanged(object sender, EventArgs e)
        {
            var myView = View;
            if (myView == null) return;

            FireValueChanged(myView.MyProperty);
        }

        // clean up
        protected override void Dispose(bool isDisposing)
        {
            base.Dispose(isDisposing);

            if (isDisposing)
            {
                var myView = View;
                if (myView != null && _subscribed)
                {
                    myView.MyPropertyChanged -= HandleMyPropertyChanged;
                    _subscribed = false;
                }
            }
        }
    }
}