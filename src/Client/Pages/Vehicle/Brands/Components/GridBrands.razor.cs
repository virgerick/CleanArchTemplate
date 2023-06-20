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

namespace CleanArchTemplate.Client.Pages.Vehicle.Brands.Components;

public partial class GridBrands
{
    RadzenDataGrid<BrandResponse> brandsGrid = null !;
    IQueryable<BrandResponse> brands = null !;
    BrandResponse? brandToInsert;
    BrandResponse? brandToUpdate;
    void Reset()
    {
        brandToInsert = null !;
        brandToUpdate = null !;
    }

    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();
        await GetBrandsAsync();
    }

    async Task GetBrandsAsync()
    {
        await Excecutor.Run(async () =>
        {
            var result = await BrandApiService.GetAsync();
            result.ThrowIfNotSucceeded();
            brands = result.Items.AsQueryable();
        });
    }

    async Task EditRow(BrandResponse Brand)
    {
        brandToUpdate = Brand;
        await brandsGrid.EditRow(Brand);
    }

    async Task OnUpdateRow(BrandResponse brand)
    {
        await Excecutor.Run(async () =>
        {
            LoadingService.Show();
            if (brand == brandToInsert)
            {
                brandToInsert = null !;
            }

            brandToUpdate = null !;
            var result = await BrandApiService.EditAsync(brand.Id, new(brand.Name, brand.Logo));
            //Todo:Passing right parameter
            result.ThrowIfNotSucceeded();
            await GetBrandsAsync();
            NotificationService.Notify(new NotificationMessage { Severity = NotificationSeverity.Success, Detail = "brand updated successfully" });
        });
    }

    async Task SaveRow(BrandResponse brand)
    {
        await brandsGrid.UpdateRow(brand);
    }

    void CancelEdit(BrandResponse brand)
    {
        if (brand == brandToInsert)
        {
            brandToInsert = null !;
        }

        brandToUpdate = null !;
        brandsGrid.CancelEditRow(brand);
    }

    async Task DeleteRow(BrandResponse brand)
    {
        var confirmed = await DialogService.Confirm($"Are you sure to delete the brand with the name '{brand.Name}'?");
        if (confirmed == false)
        {
            return;
        }

        if (brand == brandToInsert)
        {
            brandToInsert = null !;
        }

        if (brand == brandToUpdate)
        {
            brandToUpdate = null !;
        }

        brandsGrid.CancelEditRow(brand);
        await OnDeleteRow(brand);
        await brandsGrid.Reload();
    }

    async Task InsertRow()
    {
        brandToInsert = new BrandResponse();
        await brandsGrid.InsertRow(brandToInsert);
    }

    async Task OnCreateRow(BrandResponse brand)
    {
        await Excecutor.Run(async () =>
        {
            LoadingService.Show();
            brandToInsert = null !;
            var result = await BrandApiService.CreateAsync(new(brand.Name,brand.Logo!));
            //Todo:Passing parameters to constructor
            result.ThrowIfNotSucceeded();
            await GetBrandsAsync();
            NotificationService.Notify(new NotificationMessage { Severity = NotificationSeverity.Success, Detail = "brand created successfully" });
        });
    }

    async Task OnDeleteRow(BrandResponse brand)
    {
        await Excecutor.Run(async () =>
        {
            brandToInsert = null !;
            var result = await BrandApiService.DeleteAsync(brand.Id);
            result.ThrowIfNotSucceeded();
            await GetBrandsAsync();
            NotificationService.Notify(new NotificationMessage { Severity = NotificationSeverity.Success, Detail = "brand deleted successfully" });
        });
    }
}