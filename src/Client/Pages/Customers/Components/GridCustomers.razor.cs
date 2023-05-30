using CleanArchTemplate.Shared.Wrapper;
using Radzen;
using Radzen.Blazor;
using CleanArchTemplate.Shared.Responses.Customers;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Security.Claims;

namespace CleanArchTemplate.Client.Pages.Customer.Components;

public partial class GridCustomers
{
    RadzenDataGrid<CustomerResponse> CustomersGrid = null!;
    IEnumerable<CustomerResponse> Customers = null!;
   
   

    CustomerResponse CustomerToInsert = null!;
    CustomerResponse CustomerToUpdate = null!;
    void Reset()
    {
        CustomerToInsert = null!;
        CustomerToUpdate = null!;
    }

    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();
        await GetCustomersAsync();
    }

    async Task GetCustomersAsync()
    {
        LoadingService.Show();
        try
        {
            var result = await CustomerApiService.GetAsync();
            result.ThrowIfNotSucceded();
            Customers = result.Items.AsQueryable();
        }
        catch (Exception ex)
        {
            NotificationService.Notify(new NotificationMessage { Severity = NotificationSeverity.Error, Detail = ex.Message });
            
        }
        finally{
            LoadingService.Hide();
        }
    }

    async Task EditRow(CustomerResponse Customer)
    {
        CustomerToUpdate = Customer;
        await CustomersGrid.EditRow(Customer);
    }

    async Task OnUpdateRow(CustomerResponse Customer)
    {
        try
        {
            LoadingService.Show();
            if (Customer == CustomerToInsert)
            {
                CustomerToInsert = null!;
            }
            CustomerToUpdate = null!;
            var result = await CustomerApiService.EditAsync(Customer.Id,new(Customer.PlateNumber, Customer.Brand, Customer.Model, Customer.Type));
            result.ThrowIfNotSucceded();
            
            await GetCustomersAsync();
            NotificationService.Notify(new NotificationMessage { Severity = NotificationSeverity.Success, Detail = "Customer updated successfully" });
        }
        catch (Exception ex)
        {
            NotificationService.Notify(new NotificationMessage { Severity = NotificationSeverity.Error, Detail = ex.Message });
        }
        finally {
            LoadingService.Hide();
        }

    }
    async Task SaveRow(CustomerResponse Customer)
    {
        await CustomersGrid.UpdateRow(Customer);
    }

    void CancelEdit(CustomerResponse Customer)
    {
        if (Customer == CustomerToInsert)
        {
            CustomerToInsert = null;
        }

        CustomerToUpdate = null;
        CustomersGrid.CancelEditRow(Customer);
    }

    async Task DeleteRow(CustomerResponse Customer)
    {
        var confirmed = await DialogService.Confirm($"Are you sure to delete the Customer with the place '{Customer.PlateNumber}'?");
        if(confirmed==false){
           
            return;
        }
            
        if (Customer == CustomerToInsert)
        {
            CustomerToInsert = null;
        }

        if (Customer == CustomerToUpdate)
        {
            CustomerToUpdate = null;
        }
        CustomersGrid.CancelEditRow(Customer);
        await OnDeleteRow(Customer);
        await CustomersGrid.Reload();
        
    }

    async Task InsertRow()
    {
        CustomerToInsert = new CustomerResponse();
        await CustomersGrid.InsertRow(CustomerToInsert);
    }

    async Task OnCreateRow(CustomerResponse Customer)
    {
        try
        {
            LoadingService.Show();
            CustomerToInsert = null!;
            var result = await CustomerApiService.CreateAsync(new(Customer.PlateNumber, Customer.Brand, Customer.Model, Customer.Type));
            result.ThrowIfNotSucceded();
            await GetCustomersAsync();
           
            NotificationService.Notify(new NotificationMessage { Severity = NotificationSeverity.Success, Detail = "Customer created successfully" });
        }
        catch (Exception ex)
        {
            await CustomersGrid.EditRow(Customer);
            NotificationService.Notify(new NotificationMessage { Severity = NotificationSeverity.Error, Detail = ex.Message });
        }
        finally {
            LoadingService.Hide();
        }
    }
    async Task OnDeleteRow(CustomerResponse Customer)
    {
        try
        {
            LoadingService.Show();
            CustomerToInsert = null!;
            var result = await CustomerApiService.DeleteAsync(Customer.Id);
            result.ThrowIfNotSucceded();
            await GetCustomersAsync();
           
            NotificationService.Notify(new NotificationMessage { Severity = NotificationSeverity.Success, Detail = "Customer deleted successfully" });
        }
        catch (Exception ex)
        {
            await CustomersGrid.EditRow(Customer);
            NotificationService.Notify(new NotificationMessage { Severity = NotificationSeverity.Error, Detail = ex.Message });
        }
        finally {
            LoadingService.Hide();
        }
    }
    async Task ResoreRow(CustomerResponse Customer)
    {
        try
        {
            var confirmed = await DialogService.Confirm($"Are you sure to restore the Customer with the place '{Customer.PlateNumber}'?");
            if (!confirmed.Value) return;
            LoadingService.Show();
            CustomerToInsert = null!;
            var result = await CustomerApiService.RestoreAsync(Customer.Id);
            result.ThrowIfNotSucceded();
            await GetCustomersAsync();
            NotificationService.Notify(new NotificationMessage { Severity = NotificationSeverity.Success, Detail = "Customer restored successfully" });
        }
        catch (Exception ex)
        {
            await CustomersGrid.EditRow(Customer);
            NotificationService.Notify(new NotificationMessage { Severity = NotificationSeverity.Error, Detail = ex.Message });
        }
        finally {
            LoadingService.Hide();
        }
    }
    
}