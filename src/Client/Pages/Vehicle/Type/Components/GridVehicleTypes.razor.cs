using CleanArchTemplate.Shared.Wrapper;
using Radzen;
using Radzen.Blazor;
using CleanArchTemplate.Shared.Responses;
using CleanArchTemplate.Client.Services;
using Microsoft.AspNetCore.Components;

namespace CleanArchTemplate.Client.Pages.Vehicle.Type.Components;
using VehicleTypeResponse = IdNameResponse;
public partial class GridVehicleTypes : ComponentBase
{
    RadzenDataGrid<VehicleTypeResponse> vehicleTypesGrid = null!;
    IEnumerable<VehicleTypeResponse> vehicleTypes = null!;
   
    VehicleTypeResponse vehicleTypeToInsert;
    VehicleTypeResponse vehicleTypeToUpdate;
    void Reset()
    {
        vehicleTypeToInsert = null!;
        vehicleTypeToUpdate = null!;
    }

    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();
        await GetVehicleTypesAsync();
    }

    async Task GetVehicleTypesAsync()
    {
        await Excecutor.Run(async () =>
        {
            var result = await VehicleTypeApiService.GetAsync();
            result.ThrowIfNotSucceeded();
            vehicleTypes = result.Items.AsQueryable();
        });
    }

    async Task EditRow(VehicleTypeResponse vehicleType)
    {
        vehicleTypeToUpdate = vehicleType;
        await vehicleTypesGrid.EditRow(vehicleType);
    }

    async Task OnUpdateRow(VehicleTypeResponse vehicle)
    {
        await Excecutor.Run(async () =>
        {
            LoadingService.Show();
            if (vehicle == vehicleTypeToInsert)
            {
                vehicleTypeToInsert = null!;
            }
            vehicleTypeToUpdate = null!;
            var result = await VehicleTypeApiService.EditAsync(vehicle.Id,new(vehicle.Name));
            result.ThrowIfNotSucceeded();
            
            await GetVehicleTypesAsync();
            NotificationService.Notify(new NotificationMessage { Severity = NotificationSeverity.Success, Detail = "Vehicle updated successfully" });
        });

    }
    async Task SaveRow(VehicleTypeResponse vehicle)
    {
        await vehicleTypesGrid.UpdateRow(vehicle);
    }

    void CancelEdit(VehicleTypeResponse vehicle)
    {
        if (vehicle == vehicleTypeToInsert)
        {
            vehicleTypeToInsert = null!;
        }

        vehicleTypeToUpdate = null!;
        vehicleTypesGrid.CancelEditRow(vehicle);
    }

    async Task DeleteRow(VehicleTypeResponse vehicle)
    {
        var confirmed = await DialogService.Confirm($"Are you sure to delete the vehicle type with the name '{vehicle.Name}'?");
        if(confirmed==false){
           
            return;
        }
            
        if (vehicle == vehicleTypeToInsert)
        {
            vehicleTypeToInsert = null!;
        }

        if (vehicle == vehicleTypeToUpdate)
        {
            vehicleTypeToUpdate = null!;
        }
        vehicleTypesGrid.CancelEditRow(vehicle);
        await OnDeleteRow(vehicle);
        await vehicleTypesGrid.Reload();
        
    }

    async Task InsertRow()
    {
        vehicleTypeToInsert = VehicleTypeResponse.Empty();
        await vehicleTypesGrid.InsertRow(vehicleTypeToInsert);
    }

    async Task OnCreateRow(VehicleTypeResponse vehicle)
    {
       await  Excecutor.Run(async () =>
        {
            LoadingService.Show();
            vehicleTypeToInsert = null!;
            var result = await VehicleTypeApiService.CreateAsync(new(vehicle.Name));
            result.ThrowIfNotSucceeded();
            await GetVehicleTypesAsync();

            NotificationService.Notify(new NotificationMessage { Severity = NotificationSeverity.Success, Detail = "Vehicle created successfully" });
        });
    }
    async Task OnDeleteRow(VehicleTypeResponse vehicle)
    {
      await  Excecutor.Run(async () =>
        {
            vehicleTypeToInsert = null!;
            var result = await VehicleTypeApiService.DeleteAsync(vehicle.Id);
            result.ThrowIfNotSucceeded();
            await GetVehicleTypesAsync();

            NotificationService.Notify(new NotificationMessage { Severity = NotificationSeverity.Success, Detail = "Vehicle deleted successfully" });
        });
    }
   
}