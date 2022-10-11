namespace Api.Gateway.Models
{
    /// <summary>
    /// clase utilizada para la paginación de los datos
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class DataCollection<T>
    {
        public bool HasItems { get; set; }
        public IEnumerable<T> Items { get; set; }
        public int Total { get; set; }
        public int Page { get; set; }
        public int Pages { get; set; }
    }
}
