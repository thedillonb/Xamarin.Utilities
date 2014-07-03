using MonoTouch.UIKit;
using System.Drawing;
using System;
using System.Collections.Generic;

namespace Xamarin.Utilities.DialogElements
{
    public class SplitButtonElement : Element
    {
        public static UIColor CaptionColor = UIColor.Black;
        public static UIFont CaptionFont = UIFont.SystemFontOfSize(14f);
        public static UIColor TextColor = UIColor.LightGray;
        public static UIFont TextFont = UIFont.BoldSystemFontOfSize(14f);
        private readonly List<SplitButton> _buttons = new List<SplitButton>();

        public SplitButton AddButton(string caption, string text, Action tapped = null)
        {
            var btn = new SplitButton(caption, text);
            if (tapped != null)
                btn.TouchUpInside += (sender, e) => tapped();
            else
                btn.UserInteractionEnabled = false;

            _buttons.Add(btn);
            return btn;
        }

		public override UITableViewCell GetCell(UITableView tv)
		{
            var cell = tv.DequeueReusableCell("splitbuttonelement") as SplitButtonCell;
            if (cell == null)
            {
                cell = new SplitButtonCell();
                cell.SelectionStyle = UITableViewCellSelectionStyle.None;
                cell.SeparatorInset = UIEdgeInsets.Zero;
            }
            cell.SetButtons(tv, _buttons);
			return cell;
		}

        private class SplitButtonCell : UITableViewCell
        {
            private UIButton[] _buttons;
            private UIView[] _seperatorViews;

            public SplitButtonCell()
                : base(UITableViewCellStyle.Default, "splitbuttonelement")
            {
            }

            public void SetButtons(UITableView tableView, List<SplitButton> items)
            {
                if (_buttons != null)
                {
                    foreach (var btn in _buttons)
                    {
                        btn.RemoveFromSuperview();
                    }
                }

                _buttons = new UIButton[items.Count];

                for (var i = 0; i < items.Count; i++)
                {
                    _buttons[i] = items[i];
                    ContentView.Add(_buttons[i]);
                }

                if (_seperatorViews != null)
                {
                    foreach (var v in _seperatorViews)
                    {
                        v.RemoveFromSuperview();
                        v.Dispose();
                    }
                    _seperatorViews = null;
                }

                if (items.Count > 0)
                {
                    _seperatorViews = new UIView[items.Count - 1];
                    for (var i = 0; i < _seperatorViews.Length; i++)
                    {
                        _seperatorViews[i] = new UIView { BackgroundColor = tableView.SeparatorColor };
                        ContentView.Add(_seperatorViews[i]);
                    }
                }
            }


            public override void LayoutSubviews()
            {
                base.LayoutSubviews();

                if (_buttons != null)
                {
                    var width = this.Bounds.Width;
                    var space = width / (float)_buttons.Length;

                    for (var i = 0; i < _buttons.Length; i++)
                    {
                        _buttons[i].Frame = new RectangleF(i * space, 0, space - 0.5f, this.Bounds.Height);
                        _buttons[i].LayoutSubviews();

                        if (i != _buttons.Length - 1)
                            _seperatorViews[i].Frame = new RectangleF(_buttons[i].Frame.Right, 0, 0.5f, this.Bounds.Height);
                    }
                }
            }

        }


        public class SplitButton : UIButton
        {
            private readonly UILabel _caption;
            private readonly UILabel _text;

            public string Text
            {
                get { return _text.Text; }
                set 
                { 
                    _text.Text = value; 
                    this.SetNeedsDisplay();
                }
            }

            public SplitButton(string caption, string text)
                : base(UIButtonType.Custom)
            {
                AutosizesSubviews = true;

                _caption = new UILabel();
                _caption.TextColor = CaptionColor;
                _caption.Font = CaptionFont;
                _caption.Text = caption;
                this.Add(_caption);

                _text = new UILabel();
                _text.TextColor = TextColor;
                _text.Font = TextFont;
                _text.Text = text;
                this.Add(_text);

                this.TouchDown += (sender, e) => this.BackgroundColor = UIColor.FromWhiteAlpha(0.95f, 1.0f);
                this.TouchUpInside += (sender, e) => this.BackgroundColor = UIColor.White;
                this.TouchUpOutside += (sender, e) => this.BackgroundColor = UIColor.White;
            }
            public override void LayoutSubviews()
            {
                base.LayoutSubviews();

                _text.Frame = new RectangleF(12, 3, (int)Math.Floor(this.Bounds.Width) - 24f, (int)Math.Ceiling(TextFont.LineHeight) + 1);
                _caption.Frame = new RectangleF(12, _text.Frame.Bottom, (int)Math.Floor(this.Bounds.Width) - 24f, (int)Math.Ceiling(CaptionFont.LineHeight));
            }
        }

    }

}

