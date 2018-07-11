using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using NUnit.UiKit.Elements;

namespace NUnit.Gui.Presenters
{
    public class ButtonElement : ControlElement<Button>, ICommand
    {
        public ButtonElement(Button button)
            : base(button)
        {
            Control.Click += delegate { Execute?.Invoke(); };
        }

        public string ToolTipText
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        public event CommandHandler Execute;
    }
}
