using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace ms.Controllers;

[ApiController]
[Produces("application/json")]
[Route("[controller]")]
public class PlayerBetsController : ControllerBase
{
	private readonly ILogger<PlayerBetsController> _logger;

	public PlayerBetsController(ILogger<PlayerBetsController> logger)
	{
		_logger = logger;
	}

	[HttpGet(Name = "GetWeatherForecastt")]
	public IEnumerable<PlayerBets> Get()
	{
		return Enumerable.Range(1, 5).Select(index => new PlayerBets
		{
			//Date = DateTime.Now.AddDays(index)
			//TemperatureC = Random.Shared.Next(-20, 55),
			//Summary = Summaries[Random.Shared.Next(Summaries.Length)]
		})
		.ToArray();
	}

	[HttpGet]
	[ProducesResponseType(typeof(PaginatedItemsViewModel<PlayerBets>), (int)HttpStatusCode.OK)]
	[ProducesResponseType(typeof(IEnumerable<PlayerBets>), (int)HttpStatusCode.OK)]
	[ProducesResponseType((int)HttpStatusCode.BadRequest)]
	public async Task<IActionResult> Get(string topic, [FromBody]PlayerDetails details)
	{
		string serializedDetails = JsonConvert.SerializeObject(details);
		using (var producer = new ProducerBuilder<Null, string>(_config).Build())
		{
			await producer.ProduceAsync(topic, new Message<Null, string> { Value = serializedDetails });
			producer.Flush(TimeSpan.FromSeconds(10));
			return Ok(true);
		}
	}
}

public class PlayerDetails
{
	public string? start_date { get; set; }

	public string? end_date { get; set; }

	public string? user_id { get; set; }

}