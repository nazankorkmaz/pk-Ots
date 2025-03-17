using Microsoft.AspNetCore.Mvc;

namespace Ots.Api.Controllers;


public class GeoLocation
{
    public int Id { get; set; }
    public decimal Lat { get; set; }
    public decimal Long { get; set; }
}

[Route("api/[controller]")] // controllerin ismini alir: GeoLocationController
[ApiController]  // atrribute kodun isleyisiyisi hakkinda runtimeda bilgi verir.  parametre alabilir . abstract classlardır
//[NonController]
public class GeoLocationController : ControllerBase
{
    private List<GeoLocation> list;

    public GeoLocationController()
    {
        list = new List<GeoLocation>();
        var rng = new Random();

        list.Add(new GeoLocation { Id = 1, Lat = rng.Next(900, 1400), Long = rng.Next(900, 1400) });
        list.Add(new GeoLocation { Id = 2, Lat = rng.Next(900, 1400), Long = rng.Next(900, 1400) });
        list.Add(new GeoLocation { Id = 3, Lat = rng.Next(900, 1400), Long = rng.Next(1500, 3400) });
        list.Add(new GeoLocation { Id = 4, Lat = rng.Next(900, 1400), Long = rng.Next(1500, 3400) });
        list.Add(new GeoLocation { Id = 5, Lat = rng.Next(1500, 3400), Long = rng.Next(900, 1400) });
        list.Add(new GeoLocation { Id = 6, Lat = rng.Next(1500, 3400), Long = rng.Next(900, 1400) });
        list.Add(new GeoLocation { Id = 7, Lat = rng.Next(1500, 3400), Long = rng.Next(1500, 3400) });
        list.Add(new GeoLocation { Id = 8, Lat = rng.Next(1500, 3400), Long = rng.Next(1500, 3400) });
    }

    [HttpGet("GetByIdQuery")]
    public GeoLocation GetByIdQuery([FromQuery] int? id)
    {
        var location = list.FirstOrDefault(x => x.Id == id);
        return location;
    }

    [HttpGet("GetByIdRoute/{id}")]
    public GeoLocation GetByIdRoute([FromRoute] int? id)
    {
        var location = list.FirstOrDefault(x => x.Id == id);
        return location;
    }


    [HttpGet("ByIdQuery")]
    public string ByIdQuery([FromQuery] int? id,[FromQuery] string? name, [FromQuery] string? surname, [FromQuery] List<string> city)
    {
         return $"Id: {id}, Name: {name}, Surname: {surname}, City: {string.Join(",", city)}";
    }

    [HttpGet("ByIdRoute/{id}/{name}/{surname}")]
    public string ByIdRoute([FromRoute] int? id, [FromRoute] string? name, [FromRoute] string? surname)
    {
        return $"Id: {id}, Name: {name}, Surname: {surname}";
    }

    [HttpPost("Post")]
    public string Post([FromBody] GeoLocation location)
    {
        return $"Id: {location.Id}, Lat: {location.Lat}, Long: {location.Long}";
    }

}