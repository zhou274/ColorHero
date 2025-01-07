using System.Collections.Generic;

namespace ROFLPlay.DiffuseX
{
	[System.Serializable]
	public class DiffuseData 
	{
		public DiffuseApp[] apps;
	}


	[System.Serializable]
	public class DiffuseApp
	{
		public string name;
		public string icon_url;
		public string image_url;
		public string market_url;
	}
}

