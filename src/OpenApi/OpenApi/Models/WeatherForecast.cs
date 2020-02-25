using System;

namespace OpenApi.Models
{
	/// <summary>
	/// Represents a weather forecast.
	/// </summary>
	public class WeatherForecast
	{
		/// <summary>
		/// The date this forecast applies to.
		/// </summary>
		/// <example>2019-09-30T14:03:04.8060171+02:00</example>
		public DateTime Date { get; set; }

		/// <summary>
		/// The forecast temperature in degrees Celsius.
		/// </summary>
		/// <example>25</example>
		public int TemperatureC { get; set; }

		/// <summary>
		/// The forecast temperature in degrees Fahrenheit.
		/// </summary>
		/// <example>77</example>
		public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);

		/// <summary>
		/// A brief textual description of the forecast.
		/// </summary>
		/// <example>Mild</example>
		public string Summary { get; set; }
	}
}
