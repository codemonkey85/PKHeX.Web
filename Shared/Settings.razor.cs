namespace PKHeX.Web.Shared;

public partial class Settings
{
    [CascadingParameter]
    private MudDialogInstance MudDialog { get; set; }

    private readonly IReadOnlyList<ComboItem> GameVersions = GameInfo.VersionDataSource;
    private ComboItem BlankVersion;
    protected override async Task OnInitializedAsync()
    {
        var storedVersion = await LocalStorage.GetItemAsync<GameVersion>("BlankVersion");
        BlankVersion = GameVersions.First(x => x.Value == Convert.ToInt32(storedVersion));
    }

    protected async void Submit()
    {
        var gameVersion = (GameVersion)BlankVersion.Value;
        if (await LocalStorage.GetItemAsync<GameVersion>("BlankVersion") != gameVersion)
        {
            SE.SAV = SaveUtil.GetBlankSAV(gameVersion, "PKHeX");
            PE.PKM = SE.SAV.LoadTemplate();
        }

        await LocalStorage.SetItemAsync("BlankVersion", gameVersion);
        MudDialog.Close(DialogResult.Ok(true));
    }

    private void Cancel() => MudDialog.Cancel();
}
