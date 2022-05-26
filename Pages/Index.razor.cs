namespace PKHeX.Web.Pages;

public partial class Index
{
    protected override void OnInitialized() => SE.OnChange += StateHasChanged;
}
