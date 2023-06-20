using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using System.Net.Http;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Routing;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.Web.Virtualization;
using Microsoft.AspNetCore.Components.WebAssembly.Http;
using Microsoft.JSInterop;
using CleanArchTemplate.Client;
using CleanArchTemplate.Client.Authentication;
using CleanArchTemplate.Client.Extensions;
using CleanArchTemplate.Client.Shared;
using CleanArchTemplate.Client.Shared.Layouts;
using CleanArchTemplate.Client.Shared.Components;
using CleanArchTemplate.Client.Storages;
using CleanArchTemplate.Client.Services;
using CleanArchTemplate.Shared.Enums;
using CleanArchTemplate.Shared.Wrapper;
using CleanArchTemplate.Shared.Models;
using CleanArchTemplate.Shared.Mapping;
using CleanArchTemplate.Shared.Requests;
using CleanArchTemplate.Shared.Responses;
using Radzen;
using Radzen.Blazor;
using CleanArchTemplate.Shared.Requests.Routes;
using CleanArchTemplate.Shared.Responses.Routes;
using CleanArchTemplate.Shared.Responses.Vehicles;

namespace CleanArchTemplate.Client.Pages.Routes.Components
{
    public partial class GridRoutes
    {
        RadzenDataGrid<RouteResponse> RoutesGrid = null !;
        IEnumerable<RouteResponse> Routes =Enumerable.Empty<RouteResponse>();
        IEnumerable<VehicleResponse> Vehicles= Enumerable.Empty<VehicleResponse>();
        IEnumerable<string> Places = Enumerable.Empty<string>();
       
        RouteResponse RouteToInsert = null !;
        RouteResponse RouteToUpdate = null !;
        void Reset()
        {
            RouteToInsert = null !;
            RouteToUpdate = null !;
        }

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            await GetDefaultRoutesAsync();
        }

        async Task GetDefaultRoutesAsync()
        {
            await Excecutor.Run(async () => {
                var result = await RouteApiService.GetDefaultAsync();
                result.ThrowIfNotSucceeded();
                Routes = result.Data.Routes;
                Vehicles = result.Data.Vehicles;
                Places = result.Data.Places;
            });
           
          
        }

        async Task EditRow(RouteResponse Route)
        {
            RouteToUpdate = Route;
            await RoutesGrid.EditRow(Route);
        }

        async Task OnUpdateRow(RouteResponse Route)
        {
            await Excecutor.Run(async() =>
            {
                if (Route == RouteToInsert)
                {
                    RouteToInsert = null !;
                }

                RouteToUpdate = null !;
                var result = await RouteApiService.EditAsync(Route.Id, new(Route.Origin,Route.Destination,Route.Distance,Route.EstimatedTime,Route.Amount,Route.VehicleId));
                result.ThrowIfNotSucceeded();
                await GetDefaultRoutesAsync();
                NotificationService.Notify(new NotificationMessage { Severity = NotificationSeverity.Success, Detail = "Route updated successfully" });
          
            });
        }

        async Task SaveRow(RouteResponse Route)
        {
            await RoutesGrid.UpdateRow(Route);
        }

        void CancelEdit(RouteResponse Route)
        {
            if (Route == RouteToInsert)
            {
                RouteToInsert = null!;
            }

            RouteToUpdate = null!;
            RoutesGrid.CancelEditRow(Route);
        }

        async Task DeleteRow(RouteResponse Route)
        {
            var confirmed = await DialogService.Confirm($"Are you sure to delete the Route from '{Route.Origin}' to '{Route.Destination}' that cost {Route.Amount} ?");
            if (confirmed == false)
            {
                return;
            }

            if (Route == RouteToInsert)
            {
                RouteToInsert = null!;
            }

            if (Route == RouteToUpdate)
            {
                RouteToUpdate = null!;
            }

            RoutesGrid.CancelEditRow(Route);
            await OnDeleteRow(Route);
            await RoutesGrid.Reload();
        }

        async Task InsertRow()
        {
            RouteToInsert = new RouteResponse();
            await RoutesGrid.InsertRow(RouteToInsert);
        }

        async Task OnCreateRow(RouteResponse Route)
        {
            await Excecutor.Run(async () => { 
            
                RouteToInsert = null !;
                var result = await RouteApiService.CreateAsync(new(Route.Origin,Route.Destination,Route.Distance,Route.EstimatedTime,Route.Amount,Route.VehicleId));
                result.ThrowIfNotSucceeded();
                await GetDefaultRoutesAsync();
                NotificationService.Notify(new NotificationMessage { Severity = NotificationSeverity.Success, Detail = "Route created successfully" });
            });
        }

        async Task OnDeleteRow(RouteResponse Route)
        {
            await Excecutor.Run(async () =>
            {

                RouteToInsert = null!;
                var result = await RouteApiService.DeleteAsync(Route.Id);
                result.ThrowIfNotSucceeded();
                await GetDefaultRoutesAsync();
                NotificationService.Notify(new NotificationMessage { Severity = NotificationSeverity.Success, Detail = "Route deleted successfully" });
            });
        }

    }
}