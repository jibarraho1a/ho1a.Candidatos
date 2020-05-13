namespace ho1a.applicationCore.Entities
{
    public class ConfiguracionBase : BaseEntityId
    {
        public string Descripcion { get; set; }
        public string Key { get; set; }
        public string Values { get; set; }
        public string Values2 { get; set; }
        public bool Requerido { get; set; }
    }
}