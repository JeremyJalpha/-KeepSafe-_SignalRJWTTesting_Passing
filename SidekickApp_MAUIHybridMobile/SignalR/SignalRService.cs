using Microsoft.AspNetCore.SignalR.Client;

namespace SidekickApp_MAUIHybridMobile.SignalR
{
    public class SignalRService
    {
        private HubConnection? _hubConnection;

        // Starts a connection to the specified hub with a JWT
        public async Task StartConnWithJWTAsync(string hubUrl, string jwt)
        {
            await StopConnectionAsync();

            _hubConnection = new HubConnectionBuilder()
                .WithUrl(hubUrl, options =>
                {
                    options.AccessTokenProvider = () => Task.FromResult<string?>(jwt);
                })
                .Build();

            try
            {
                await _hubConnection.StartAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error while starting connection: " + ex.Message);
            }
        }

        // Stops the current connection if it is active
        public async Task StopConnectionAsync()
        {
            if (_hubConnection != null && _hubConnection.State == HubConnectionState.Connected)
            {
                await _hubConnection.StopAsync();
                await _hubConnection.DisposeAsync();
                _hubConnection = null;
            }
        }

        // Send the GPS coordinates to the GPSLocationHub
        public async Task SendCoordsToGPSLocHub(int gpsCoords)
        {
            if (_hubConnection == null || _hubConnection.State != HubConnectionState.Connected)
            {
                throw new InvalidOperationException("Connection has not been started or is not active.");
            }

            await _hubConnection.InvokeAsync("SendCoordsToGPSLocHub", gpsCoords);
        }
    }
}