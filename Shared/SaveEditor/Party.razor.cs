namespace PKHeX.Web.Shared.SaveEditor;

public partial class Party
{
    protected override void OnInitialized() => SE.OnChange += StateHasChanged;

    protected void SetPokemon(int index)
    {
        SE.SAV.SetPartySlotAtIndex(PE.PKM, index);
        SE.NotifyDataChanged();
    }

    public void Dispose() => SE.OnChange -= StateHasChanged;
}
