namespace PKHeX.Web.Shared.PokemonEditor.Main;

public partial class MainPokemon
{
    protected override void OnInitialized() => PE.OnChange += StateHasChanged;

    private static Task<IEnumerable<int>> Search(string value)
    {
        var species = GameInfo.SpeciesDataSource.Skip(1).Where(x => x.Text.Contains(value, StringComparison.InvariantCultureIgnoreCase)).OrderBy(x => x.Text).Select(x => x.Value);
        return Task.FromResult(species);
    }

    private void SpeciesChanged(int speciesId)
    {
        PE.PKM.Species = speciesId;
        PE.PKM.IsNicknamed = false;
        PE.PKM.RefreshAbility(0);
        PE.PKM.ClearNickname();
        PE.NotifyDataChanged();
    }

    private void SetForm(int formId) =>
        //PE.PKM.SetForm(formId);
        PE.NotifyDataChanged();

    private static Task<IEnumerable<int>> SearchNatures(string value)
    {
        var natures = Util.GetNaturesList("en").Select((name, index) => new
        {
            Name = name,
            Index = index
        }).Where(x => x.Name.Contains(value, StringComparison.InvariantCultureIgnoreCase)).OrderBy(x => x.Name).Select(x => x.Index);
        return Task.FromResult(natures);
    }

    private void NatureChanged(int natureId)
    {
        if (SE.SAV.Generation >= 8)
        {
            PE.PKM.Nature = natureId;
            return;
        }

        PE.PKM.SetNature(natureId);
    }

    private static Task<IEnumerable<int>> SearchItems(string value)
    {
        var items = GameInfo.ItemDataSource.Where(x => x.Text.Contains(value, StringComparison.InvariantCultureIgnoreCase)).Select(x => x.Value);
        return Task.FromResult(items);
    }

    private void ItemsChanged(int itemId) => PE.PKM.ApplyHeldItem(itemId, SE.SAV.Generation);

    private void SetNickname(string value)
    {
        PE.PKM.IsNicknamed = true;
        PE.PKM.Nickname = value;
    }

    private void SetIsNicknamed(bool value)
    {
        PE.PKM.IsNicknamed = value;
        if (value == false)
        {
            PE.PKM.ClearNickname();
        }
    }

    public void Dispose() => PE.OnChange -= StateHasChanged;
}
