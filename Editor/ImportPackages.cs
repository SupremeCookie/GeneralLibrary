using UnityEditor;
using UnityEngine;

public class ImportPackages
{
	private enum Packages
	{
		Newtonsoft,


		Count,
	}


	[MenuItem("DKat/Import Packages/Import All")]
	private static void ImportAllPackages()
	{
		for (int i = 0; i < (int)Packages.Count; ++i)
		{
			ImportPackage((Packages)i);
		}
	}

	[MenuItem("DKat/Import Packages/NewtonSoft")]
	private static void ImportNewtonSoft()
	{
		ImportPackage(Packages.Newtonsoft);
	}



	private static void ImportPackage(Packages packageType)
	{
		switch (packageType)
		{
			case Packages.Newtonsoft:
			{
				Debug.LogWarning("TODO DK: Write implementation to copy newtonsoft zip to the library/packagesCache, and unzip it there.");
				Debug.LogWarning("TODO DK: Write implementation to add newtonsoft data to the packages manifest file.");
				break;
			}
		}
	}
}
