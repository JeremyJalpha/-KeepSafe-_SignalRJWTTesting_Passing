using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace SidekickApp_WebAPI.SignalR
{
    [Authorize]
    public class GPSLocationHub : Hub<ILocationClient>
    {
        public async Task SendCoordsToGPSLocHub(int coords)
        {
            //Do some stuff with the Coordinates
            await HandleCoordinates(coords);
        }

        private Task HandleCoordinates(int coords)
        {
            // Implement your logic to handle a valid JWT here
            return Task.CompletedTask; // This is just a placeholder
        }
    }

    public interface ILocationClient
    {
        int GetLastKnownLoc();
    }
}

