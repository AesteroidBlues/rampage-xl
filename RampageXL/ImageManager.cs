using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using RampageXL.Shape;

namespace RampageXL
{
	public enum ImageName
	{
		player_standing_L00,
		player_standing_R00,
		player_walking_L00,
		player_walking_L01,
		player_walking_L02,
		player_walking_R00,
		player_walking_R01,
		player_walking_R02,
		player_attack_R01,
		player_attack_R02,
		player_attack_R03,
		player_attack_L01,
		player_attack_L02,
		player_attack_L03
	}

	class ImageManager
	{
		private static Dictionary<ImageName, Image> images;

		public static void Init()
		{
			images = new Dictionary<ImageName, Image>();
		}

		public static void LoadImage(ImageName name, string path) 
		{
			Image newImage = new Image(path);
			newImage.SetName(name);
			images.Add(name, newImage);

			// Create a rectangle object, make it have 0 alpha, and stream it over to CalVR to pre-load the textures
			Rectangle r = new Rectangle(0, 0, newImage.width, newImage.height);
			r.setTexture(path);
			r.Hide();
			Mugic.MugicObjectManager.Register(r);
		}

		public static Image GetImage(ImageName name)
		{
			return images[name];
		}
	}
}
