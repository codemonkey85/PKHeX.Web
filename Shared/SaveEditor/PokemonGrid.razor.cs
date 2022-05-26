namespace PKHeX.Web.Shared.SaveEditor;

public partial class PokemonGrid
{
    [Parameter]
    public IList<PKM> PokemonList { get; set; } = new PKM[] { };
    [Parameter]
    public int RowSize { get; set; }

    [Parameter]
    public int Cells { get; set; }

    [Parameter]
    public Action<int> SetPokemon { get; set; } = (i) => throw new MissingMemberException();
    protected void ChangePokemon(PKM? pokemon, int index)
    {
        if (PE.IsSetLeft)
        {
            if (pokemon == null || pokemon.Species == 0)
            {
                return;
            }

            PE.PKM = pokemon.Clone();
        }
        else
        {
            SetPokemon(index);
        }
    }
}
