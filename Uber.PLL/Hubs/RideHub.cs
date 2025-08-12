using Microsoft.AspNetCore.SignalR;

public class RideHub : Hub
{
    public async Task SendRideRequestToDriver(string driverId, object rideDetails)
    {
        await Clients.User(driverId).SendAsync("ReceiveRideRequest", rideDetails);
    }

    public async Task DriverResponse(string rideId, bool accepted)
    {
        // TODO: Update ride status in DB here
        if (accepted)
            await Clients.Group($"ride_{rideId}").SendAsync("RideAccepted", rideId);
        else
            await Clients.Group($"ride_{rideId}").SendAsync("RideDeclined", rideId);
    }
}
