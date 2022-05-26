namespace PKHeX.Web.Shared.SaveEditor;

public partial class Box
{
    protected override void OnInitialized() => SE.OnChange += StateHasChanged;

    protected void GoBack()
    {
        var currentBox = SE.SAV.CurrentBox;
        var boxCount = SE.SAV.BoxCount;
        SE.SAV.CurrentBox = (currentBox + boxCount - 1) % boxCount;
    }

    protected void GoForward()
    {
        var currentBox = SE.SAV.CurrentBox;
        var boxCount = SE.SAV.BoxCount;
        SE.SAV.CurrentBox = (currentBox + 1) % boxCount;
    }

    protected void SetPokemon(int index)
    {
        var currentBox = SE.SAV.CurrentBox;
        SE.SAV.SetBoxSlotAtIndex(PE.PKM, currentBox, index);
        SE.NotifyDataChanged();
    }

    public void Dispose() => SE.OnChange -= StateHasChanged;
}
