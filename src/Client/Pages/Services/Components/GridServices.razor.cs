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
using CleanArchTemplate.Shared.Requests.Services;
using CleanArchTemplate.Shared.Responses.Services;
using CleanArchTemplate.Client.Pages.Services.Components;
using CleanArchTemplate.Shared.Responses.Routes;

namespace CleanArchTemplate.Client.Pages.Services.Components
{
    public partial class GridServices
    {
        RadzenDataGrid<ServiceResponse> ServicesGrid = null!;
        IEnumerable<ServiceResponse> Services = Enumerable.Empty<ServiceResponse>();
        IEnumerable<RouteResponse> Routes = Enumerable.Empty<RouteResponse>();
        ServiceResponse ServiceToInsert = null!;
        ServiceResponse ServiceToUpdate = null!;

        void Reset()
        {
            ServiceToInsert = null!;
            ServiceToUpdate = null!;
        }

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            await GetDefaultServicesAsync();
        }

        async Task GetDefaultServicesAsync()
        {
            await Excecutor.Run(async () =>
            {
                var result = await ServiceApiService.GetDefaultAsync();
                result.ThrowIfNotSucceeded();
                Services = result.Data.Services;
                Routes = result.Data.Routes;
            });
        }

        async Task EditRow(ServiceResponse Service)
        {
            ServiceToUpdate = Service;
            await ServicesGrid.EditRow(Service);
        }

        async Task OnUpdateRow(ServiceResponse Service)
        {
            await Excecutor.Run(async () =>
            {
                if (Service == ServiceToInsert)
                {
                    ServiceToInsert = null!;
                }

                ServiceToUpdate = null!;
                var result = await ServiceApiService.EditAsync(Service.Id, new());
                result.ThrowIfNotSucceeded();
                await GetDefaultServicesAsync();
                NotificationService.Notify(
                    new NotificationMessage
                    {
                        Severity = NotificationSeverity.Success,
                        Detail = "Service updated successfully"
                    }
                );
            });
        }

        async Task SaveRow(ServiceResponse Service)
        {
            await ServicesGrid.UpdateRow(Service);
        }

        void CancelEdit(ServiceResponse Service)
        {
            if (Service == ServiceToInsert)
            {
                ServiceToInsert = null!;
            }

            ServiceToUpdate = null!;
            ServicesGrid.CancelEditRow(Service);
        }

        async Task DeleteRow(ServiceResponse Service)
        {
            var confirmed = await DialogService.Confirm(
                $"Are you sure to delete the Service '{Service.Name}' ?"
            );
            if (confirmed == false)
            {
                return;
            }

            if (Service == ServiceToInsert)
            {
                ServiceToInsert = null!;
            }

            if (Service == ServiceToUpdate)
            {
                ServiceToUpdate = null!;
            }

            ServicesGrid.CancelEditRow(Service);
            await OnDeleteRow(Service);
            await ServicesGrid.Reload();
        }

        async Task InsertRow()
        {
            ServiceToInsert = new ServiceResponse();
            await ServicesGrid.InsertRow(ServiceToInsert);
        }

        async Task OnCreateRow(ServiceResponse Service)
        {
            await Excecutor.Run(async () =>
            {
                ServiceToInsert = null!;
                var result = await ServiceApiService.CreateAsync(new());
                result.ThrowIfNotSucceeded();
                await GetDefaultServicesAsync();
                NotificationService.Notify(
                    new NotificationMessage
                    {
                        Severity = NotificationSeverity.Success,
                        Detail = "Service created successfully"
                    }
                );
            });
        }

        async Task OnDeleteRow(ServiceResponse Service)
        {
            await Excecutor.Run(async () =>
            {
                ServiceToInsert = null!;
                var result = await ServiceApiService.DeleteAsync(Service.Id);
                result.ThrowIfNotSucceeded();
                await GetDefaultServicesAsync();
                NotificationService.Notify(
                    new NotificationMessage
                    {
                        Severity = NotificationSeverity.Success,
                        Detail = "Service deleted successfully"
                    }
                );
            });
        }
    }
}
