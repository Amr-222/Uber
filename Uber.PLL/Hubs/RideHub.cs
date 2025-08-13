using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

[Authorize] // both user & driver should be authenticated
public class RideHub : Hub
{
    // Client calls this right after page loads to join a ride group
    public Task JoinRideGroup(string rideGroupId)
        => Groups.AddToGroupAsync(Context.ConnectionId, rideGroupId);

    public Task LeaveRideGroup(string rideGroupId)
        => Groups.RemoveFromGroupAsync(Context.ConnectionId, rideGroupId);

    // Driver will call these from the Driver dashboard UI
    public async Task AcceptRide(string rideGroupId, int rideDbId)
    {
        // tell the user
        await Clients.Group(rideGroupId).SendAsync("RideAccepted", rideDbId);
    }

    public async Task RejectRide(string rideGroupId, int rideDbId)
    {
        await Clients.Group(rideGroupId).SendAsync("RideRejected", rideDbId);
    }
}
