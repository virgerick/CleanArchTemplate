namespace CleanArchTemplate.Shared.Responses.Vehicles;
public class VehicleResponse {
    public VehicleResponse()
    {

    }
    public VehicleResponse(Guid id, string plateNumber, string brand, string model, string type, string status)
    {
        Id = id;
        PlateNumber = plateNumber;
        Brand = brand;
        Model = model;
        Type = type;
        Status = status;
    }

   public Guid Id{get;set;}
   public string PlateNumber{get;set;}
   public string Brand{get;set;}
   public string Model{get;set;}
   public string Type{get;set;}
   public string Status { get; set; }
}
/*
  protected override Task OnInitializedAsync()
    {
        base.OnInitializedAsync();
        return GetVehiclesAsync();
    }

    async Task GetVehiclesAsync()
    {
        try
        {
            var result = await VehicleApiService.GetAsync();
            result.ThrowIfNotSucceded();
            Vehicles = result.Items.AsQueryable();
        }
        catch (Exception ex)
        {
            NotificationService.Notify(new NotificationMessage
            {
                Severity = NotificationSeverity.Error,
                Detail = ex.Message


            }); ;
        }
    }

 */
/*
 <RadzenDataGrid @ref="dataGridVehicle" Data="@Vehicles" TItem="VehicleResponse" AllowVirtualization="true" 
                AllowFiltering="true" FilterPopupRenderMode="PopupRenderMode.OnDemand" FilterCaseSensitivity="FilterCaseSensitivity.CaseInsensitive" LogicalFilterOperator="LogicalFilterOperator.Or"
                AllowSorting="true">
    <Columns>
        <RadzenDataGridColumn TItem="VehicleResponse" Property="Id" Title="Id" />
        <RadzenDataGridColumn TItem="VehicleResponse" Property="PlateNumber" Title="Plate Number" />
        <RadzenDataGridColumn TItem="VehicleResponse" Property="Brand" Title="Brand" />
        <RadzenDataGridColumn TItem="VehicleResponse" Property="Model" Title="Model" />
        <RadzenDataGridColumn TItem="VehicleResponse" Property="Type" Title="Type" />
        <RadzenDataGridColumn TItem="VehicleResponse" Property="Status" Title="Status" />
        <RadzenDataGridColumn TItem="VehicleResponse" Context="vehicle" Filterable="false" Sortable="false" TextAlign="TextAlign.Right" Width="156px">
            <Template Context="vehicle">
                <RadzenButton Icon="edit" ButtonStyle="ButtonStyle.Light" Variant="Variant.Flat" Size="ButtonSize.Medium" Click="@(args => EditRow(vehicle))" @onclick:stopPropagation="true">
                </RadzenButton>
                <RadzenButton ButtonStyle="ButtonStyle.Danger" Icon="delete" Variant="Variant.Flat" Shade="Shade.Lighter" Size="ButtonSize.Medium" class="my-1 ms-1" Click="@(args => DeleteRow(vehicle))" @onclick:stopPropagation="true">
                </RadzenButton>
            </Template>
            <EditTemplate Context="vehicle">
                <RadzenButton Icon="check" ButtonStyle="ButtonStyle.Success" Variant="Variant.Flat" Size="ButtonSize.Medium" Click="@((args) => SaveRow(vehicle))">
                </RadzenButton>
                <RadzenButton Icon="close" ButtonStyle="ButtonStyle.Light" Variant="Variant.Flat" Size="ButtonSize.Medium" class="my-1 ms-1" Click="@((args) => CancelEdit(vehicle))">
                </RadzenButton>
                <RadzenButton ButtonStyle="ButtonStyle.Danger" Icon="delete" Variant="Variant.Flat" Shade="Shade.Lighter" Size="ButtonSize.Medium" class="my-1 ms-1" Click="@(args => DeleteRow(vehicle))">
                </RadzenButton>
            </EditTemplate>
        </RadzenDataGridColumn>
    </Columns>
</RadzenDataGrid>

 */