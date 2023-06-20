using CleanArchTemplate.Shared.Wrapper;
using Radzen;
using Radzen.Blazor;
using CleanArchTemplate.Shared.Responses.Vehicles;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Security.Claims;

namespace CleanArchTemplate.Client.Pages.Vehicle.Components;

public partial class GridVehicles
{
    RadzenDataGrid<VehicleResponse> vehiclesGrid = null!;
    IEnumerable<VehicleResponse> vehicles = null!;
    IEnumerable<ModelResponse> Models =null!;
    List<string> Status = new()
    {
        "Available",
        "Maintenance",
        "OutOfService"
    };

    VehicleResponse vehicleToInsert = null!;
    VehicleResponse vehicleToUpdate = null!;
    void Reset()
    {
        vehicleToInsert = null!;
        vehicleToUpdate = null!;
    }

    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();
        await GetDefaultVehiclesAsync();
    }

    async Task GetDefaultVehiclesAsync()
    {
        LoadingService.Show();
        try
        {
            var result = await VehicleApiService.GetDefaultAsync();
            result.ThrowIfNotSucceeded();
            vehicles = result.Data.Vehicles.AsQueryable();
            Models = result.Data.Models.AsQueryable();
        }
        catch (Exception ex)
        {
            NotificationService.Notify(new NotificationMessage { Severity = NotificationSeverity.Error, Detail = ex.Message });
            
        }
        finally{
            LoadingService.Hide();
        }
    }

    async Task EditRow(VehicleResponse vehicle)
    {
        vehicleToUpdate = vehicle;
        await vehiclesGrid.EditRow(vehicle);
    }

    async Task OnUpdateRow(VehicleResponse vehicle)
    {
        try
        {
            LoadingService.Show();
            if (vehicle == vehicleToInsert)
            {
                vehicleToInsert = null!;
            }
            vehicleToUpdate = null!;
            var result = await VehicleApiService.EditAsync(vehicle.Id,new(vehicle.PlateNumber,vehicle.ModelId!.Value,vehicle.Color!));
            result.ThrowIfNotSucceeded();
            
            await GetDefaultVehiclesAsync();
            NotificationService.Notify(new NotificationMessage { Severity = NotificationSeverity.Success, Detail = "Vehicle updated successfully" });
        }
        catch (Exception ex)
        {
            NotificationService.Notify(new NotificationMessage { Severity = NotificationSeverity.Error, Detail = ex.Message });
        }
        finally {
            LoadingService.Hide();
        }

    }
    async Task SaveRow(VehicleResponse vehicle)
    {
        await vehiclesGrid.UpdateRow(vehicle);
    }

    void CancelEdit(VehicleResponse vehicle)
    {
        if (vehicle == vehicleToInsert)
        {
            vehicleToInsert = null;
        }

        vehicleToUpdate = null;
        vehiclesGrid.CancelEditRow(vehicle);
    }

    async Task DeleteRow(VehicleResponse vehicle)
    {
        var confirmed = await DialogService.Confirm($"Are you sure to delete the vehicle with the place '{vehicle.PlateNumber}'?");
        if(confirmed==false){
           
            return;
        }
            
        if (vehicle == vehicleToInsert)
        {
            vehicleToInsert = null;
        }

        if (vehicle == vehicleToUpdate)
        {
            vehicleToUpdate = null;
        }
        vehiclesGrid.CancelEditRow(vehicle);
        await OnDeleteRow(vehicle);
        await vehiclesGrid.Reload();
        
    }

    async Task InsertRow()
    {
        vehicleToInsert = new VehicleResponse();
        await vehiclesGrid.InsertRow(vehicleToInsert);
    }

    async Task OnCreateRow(VehicleResponse vehicle)
    {
        try
        {
            LoadingService.Show();
            vehicleToInsert = null!;
            var result = await VehicleApiService.CreateAsync(new(vehicle.PlateNumber, vehicle.ModelId!.Value, vehicle.Color!));
            result.ThrowIfNotSucceeded();
            await GetDefaultVehiclesAsync();
           
            NotificationService.Notify(new NotificationMessage { Severity = NotificationSeverity.Success, Detail = "Vehicle created successfully" });
        }
        catch (Exception ex)
        {
            await vehiclesGrid.EditRow(vehicle);
            NotificationService.Notify(new NotificationMessage { Severity = NotificationSeverity.Error, Detail = ex.Message });
        }
        finally {
            LoadingService.Hide();
        }
    }
    async Task OnDeleteRow(VehicleResponse vehicle)
    {
        try
        {
            LoadingService.Show();
            vehicleToInsert = null!;
            var result = await VehicleApiService.DeleteAsync(vehicle.Id);
            result.ThrowIfNotSucceeded();
            await GetDefaultVehiclesAsync();
           
            NotificationService.Notify(new NotificationMessage { Severity = NotificationSeverity.Success, Detail = "Vehicle deleted successfully" });
        }
        catch (Exception ex)
        {
            await vehiclesGrid.EditRow(vehicle);
            NotificationService.Notify(new NotificationMessage { Severity = NotificationSeverity.Error, Detail = ex.Message });
        }
        finally {
            LoadingService.Hide();
        }
    }
    async Task ResoreRow(VehicleResponse vehicle)
    {
        try
        {
            var confirmed = await DialogService.Confirm($"Are you sure to restore the vehicle with the place '{vehicle.PlateNumber}'?");
            if (!confirmed.Value) return;
            LoadingService.Show();
            vehicleToInsert = null!;
            var result = await VehicleApiService.RestoreAsync(vehicle.Id);
            result.ThrowIfNotSucceeded();
            await GetDefaultVehiclesAsync();
            NotificationService.Notify(new NotificationMessage { Severity = NotificationSeverity.Success, Detail = "Vehicle restored successfully" });
        }
        catch (Exception ex)
        {
            await vehiclesGrid.EditRow(vehicle);
            NotificationService.Notify(new NotificationMessage { Severity = NotificationSeverity.Error, Detail = ex.Message });
        }
        finally {
            LoadingService.Hide();
        }
    }
    
}