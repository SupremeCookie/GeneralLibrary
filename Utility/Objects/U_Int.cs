using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class U_Int : UpdatableObject<int>, IU_Object
{
	public U_Int() { }
	public U_Int(int defaultValue)
	{
		pValue = defaultValue;
	}
}