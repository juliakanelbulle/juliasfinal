using System;
using System.IO;
using System.Text.Json;

//Detta är min klass där highscore och nuvarande poäng personer har som spelar programeras
namespace juliasfinal.Verktyg.Saves
{
	class SparatResultat
	{
		public static Hiscore Load()
		{
			if (!File.Exists(Constants.SAVE_FILE))
			{
				return new Hiscore();
			}

			try
			{
				var json = File.ReadAllText(Constants.SAVE_FILE);
				return JsonSerializer.Deserialize<Hiscore>(json);
			}
			catch (Exception)
			{
				return new Hiscore();
			}
		}

		public static void Save(Hiscore hiscore)
		{
			var json = JsonSerializer.Serialize(hiscore);
			File.WriteAllText(Constants.SAVE_FILE, json);
		}
	}

	class Hiscore
	{
		public int PersonalBest { get; set; }
	}
}