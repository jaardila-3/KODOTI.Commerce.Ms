namespace Service.Common.Collection
{
    public class DataCollection<T>
    {
        public IEnumerable<T> Items { get; set; }//elementos
        public int Total { get; set; }//total paginas
        public int Page { get; set; }//pagina actual
        public int Pages { get; set; } //paginas generadas
        public bool HasItems
        {
            get
            {
                return Items != null && Items.Any();
            }
        }
    }
}
