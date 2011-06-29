﻿using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

using Clarity.Phone.Extensions;

using Coding4Fun.Phone.Controls.Helpers;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

namespace Coding4Fun.Phone.Controls
{
    public abstract class PopUp<T, TPopUpResult> : Control
    {
        private DialogService _popUp;
		private PhoneApplicationPage _startingPage;
        private bool _alreadyFired;
        private bool _hasHookedUpGestureWatcher;

    	protected PopUp()
		{
			Dispatcher.BeginInvoke(() => ApplyTemplate());
		}

        public bool IsOpen { get { return _popUp != null && _popUp.IsOpen; } }
        public bool IsAppBarVisible { get; set; }
        internal IApplicationBar AppBar { get; set; }
        protected internal bool IsBackKeyOverride { get; set; }

        protected DialogService.AnimationTypes AnimationType { get; set; }
        public event EventHandler<PopUpEventArgs<T, TPopUpResult>> Completed;

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            if (!_hasHookedUpGestureWatcher)
            {
                GestureHelper.WireUpGestureEvents(HasGesturesDisabled, this);
                _hasHookedUpGestureWatcher = true;
            }

            if (_popUp != null)
            {
                _popUp.SetAlignmentsOnOverlay(HorizontalAlignment, VerticalAlignment);
            }
        }

        public virtual void OnCompleted(PopUpEventArgs<T, TPopUpResult> result)
        {
            _alreadyFired = true;
            
            if (Completed != null)
                Completed(this, result);
            
            if(_popUp != null)
                _popUp.Hide();
        }

        

		public virtual void Show()
		{
            _popUp = new DialogService
            {
                AnimationType = AnimationType,
                Child = this,
                BackgroundBrush = Overlay,
                IsBackKeyOverride = IsBackKeyOverride
            };

            if (!IsAppBarVisible)
            {
				_startingPage = _popUp.Page;

                AppBar = _popUp.Page.ApplicationBar;
                _popUp.Page.ApplicationBar = null;
            }

		    _popUp.Closed += _popUp_Closed;

			Dispatcher.BeginInvoke(() => _popUp.Show());
		}

    	protected virtual TPopUpResult GetOnClosedValue()
        {
            return default(TPopUpResult);
        }

        public void Hide()
        {
            _popUp_Closed(this, null);
        }

        void _popUp_Closed(object sender, EventArgs e)
        {
            if (_popUp != null)
            {
				if (!IsAppBarVisible && _popUp.Page == _startingPage && AppBar != null)
				{
                    _startingPage.ApplicationBar = AppBar;
				}

                _popUp.Child = null;
                _popUp = null;
            }

			if (!_alreadyFired)
			{
				OnCompleted(new PopUpEventArgs<T, TPopUpResult> {PopUpResult = GetOnClosedValue()});
				return;
			}
        }

        public Brush Overlay
        {
            get { return (Brush)GetValue(OverlayProperty); }
            set { SetValue(OverlayProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Overlay.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty OverlayProperty =
            DependencyProperty.Register("Overlay", typeof(Brush), typeof(PopUp<T, TPopUpResult>), new PropertyMetadata(Application.Current.Resources["PhoneSemitransparentBrush"]));

        public bool HasGesturesDisabled
        {
            get { return (bool)GetValue(HasGesturesDisabledProperty); }
            set { SetValue(HasGesturesDisabledProperty, value); }
        }

        // Using a DependencyProperty as the backing store for HasGesturesDisabled.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty HasGesturesDisabledProperty =
            DependencyProperty.Register("HasGesturesDisabled", typeof(bool), typeof(PopUp<T, TPopUpResult>), new PropertyMetadata(true, OnHasGesturesDisabledChanged));

        private static void OnHasGesturesDisabledChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            var sender = ((PopUp<T, TPopUpResult>)o);

            if (sender != null && e.NewValue != e.OldValue)
                GestureHelper.WireUpGestureEvents((bool)e.NewValue, sender);
        }
    }
}