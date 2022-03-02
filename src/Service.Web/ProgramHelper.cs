using System;

namespace Service.Web
{
	public class ProgramHelper
	{
		public const string JwtSecretDefaultEnviromentVariable = "JWT_SECRET";
		public const string JwtAudienceDefaultEnviromentVariable = "JWT_AUDIENCE";
		public const string SettingsFileName = ".myjeteducation";

		public static string LoadJwtSecret(string enviromentVariable = JwtSecretDefaultEnviromentVariable)
		{
			string value = Environment.GetEnvironmentVariable(enviromentVariable);

			if (string.IsNullOrWhiteSpace(value))
			{
				ShowError($"ERROR! Please configure environment variable: {enviromentVariable}!");
				return null;
			}

			if (value.Length <= 15)
			{
				ShowError($"ERROR! Length of environment variable {enviromentVariable} must be greater or equal than 16 symbols!");
				return null;
			}

			return value;
		}

		public static string LoadJwtAudience(string enviromentVariable = JwtAudienceDefaultEnviromentVariable)
		{
			string value = Environment.GetEnvironmentVariable(enviromentVariable);

			if (string.IsNullOrWhiteSpace(value))
			{
				ShowError($"ERROR! Please configure environment variable: {enviromentVariable}!");
				return null;
			}

			return value;
		}

		private static void ShowError(string message)
		{
			Console.WriteLine(message);
			throw new Exception(message);
		}
	}
}