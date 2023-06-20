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
using CleanArchTemplate.Client.Services;
using CleanArchTemplate.Shared.Enums;
using CleanArchTemplate.Shared.Wrapper;
using CleanArchTemplate.Shared.Models;
using CleanArchTemplate.Shared.Mapping;
using CleanArchTemplate.Shared.Requests;
using CleanArchTemplate.Shared.Responses;
using Radzen;
using Radzen.Blazor;
using CleanArchTemplate.Client.Pages.Vehicle.Components;
using CleanArchTemplate.Shared.Responses.Vehicles;
using CleanArchTemplate.Shared.Requests.Vehicles;
using CleanArchTemplate.Client.Pages.Vehicle.Type;

namespace CleanArchTemplate.Client.Pages.Vehicle.Models.Components;

public partial class GridModels
{
    RadzenDataGrid<ModelResponse> ModelsGrid = null !;
    IQueryable<ModelResponse> Models = null!;
    IQueryable<BrandResponse> Brands = null!;
    IQueryable<IdNameResponse> VehicleTypes = null!;
    ModelResponse? ModelToInsert;
    ModelResponse? ModelToUpdate;
    void Reset()
    {
        ModelToInsert = null !;
        ModelToUpdate = null !;
    }

    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();
        await GetModelDefaultAsync();
    }

    async Task GetModelDefaultAsync()
    {
        await Excecutor.Run(async () =>
        {
            var result = await ModelApiService.GetDefaultAsync();
            result.ThrowIfNotSucceeded();
            Models = result.Data.Models.AsQueryable();
            Brands = result.Data.Brands.AsQueryable();
            VehicleTypes = result.Data.VehicleTypes.AsQueryable();
        });
    }

    async Task EditRow(ModelResponse Model)
    {
        ModelToUpdate = Model;
        await ModelsGrid.EditRow(Model);
    }

    async Task OnUpdateRow(ModelResponse Model)
    {
        await Excecutor.Run(async () =>
        {
            LoadingService.Show();
            if (Model == ModelToInsert)
            {
                ModelToInsert = null !;
            }

            ModelToUpdate = null !;
            var result = await ModelApiService.EditAsync(Model.Id, new(Model.Name, Model.Year,Model.BrandId,Model.TypeId));
            //Todo:Passing right parameter
            result.ThrowIfNotSucceeded();
            await GetModelDefaultAsync();
            NotificationService.Notify(new NotificationMessage { Severity = NotificationSeverity.Success, Detail = "Model updated successfully" });
        });
    }

    async Task SaveRow(ModelResponse Model)
    {
        await ModelsGrid.UpdateRow(Model);
    }

    void CancelEdit(ModelResponse Model)
    {
        if (Model == ModelToInsert)
        {
            ModelToInsert = null !;
        }

        ModelToUpdate = null !;
        ModelsGrid.CancelEditRow(Model);
    }

    async Task DeleteRow(ModelResponse Model)
    {
        var confirmed = await DialogService.Confirm($"Are you sure to delete the Model with the name '{Model.Name}'?");
        if (confirmed == false)
        {
            return;
        }

        if (Model == ModelToInsert)
        {
            ModelToInsert = null !;
        }

        if (Model == ModelToUpdate)
        {
            ModelToUpdate = null !;
        }

        ModelsGrid.CancelEditRow(Model);
        await OnDeleteRow(Model);
        await ModelsGrid.Reload();
    }

    async Task InsertRow()
    {
        ModelToInsert = new ModelResponse();
        await ModelsGrid.InsertRow(ModelToInsert);
    }

    async Task OnCreateRow(ModelResponse Model)
    {
        await Excecutor.Run(async () =>
        {
            LoadingService.Show();
            ModelToInsert = null !;
            var result = await ModelApiService.CreateAsync(new(Model.Name, Model.Year, Model.BrandId, Model.TypeId));
            //Todo:Passing parameters to constructor
            result.ThrowIfNotSucceeded();
            await GetModelDefaultAsync();
            NotificationService.Notify(new NotificationMessage { Severity = NotificationSeverity.Success, Detail = "Model created successfully" });
        });
    }

    async Task OnDeleteRow(ModelResponse Model)
    {
        await Excecutor.Run(async () =>
        {
            ModelToInsert = null !;
            var result = await ModelApiService.DeleteAsync(Model.Id);
            result.ThrowIfNotSucceeded();
            await GetModelDefaultAsync();
            NotificationService.Notify(new NotificationMessage { Severity = NotificationSeverity.Success, Detail = "Model deleted successfully" });
        });
    }
}