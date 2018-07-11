using System;
using System.Collections.Generic;
using System.Text;

namespace Nunit.Gui.Model
{
	public delegate void GenericEventHandler<T>(object sender, GenericEventArgs<T> eventArgs);

	public class GenericEventArgs<T> : EventArgs
	{
		public T Value { get; private set;  }

		public GenericEventArgs(T value)
		{
			Value = value;
		}
	}
}
