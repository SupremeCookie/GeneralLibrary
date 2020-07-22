using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class UpdatableObject<T>
{
	public event Action<T> OnValueChanged;

	protected T pValue;
	public T Value
	{
		get { return pValue; }
		set
		{
			pValue = value;
			RaiseEvent(value);
		}
	}

	/// <summary>
	/// Calls the OnValueChanged event with a new value passed as a parameter.
	/// </summary>
	protected void RaiseEvent(T newValue)
	{
		if (OnValueChanged != null)
		{
			OnValueChanged(newValue);
		}
	}
}