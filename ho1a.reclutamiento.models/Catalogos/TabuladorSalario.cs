using ho1a.applicationCore.Entities;

namespace ho1a.reclutamiento.models.Catalogos
{
    public class TabuladorSalario : BaseEntityId
    {
        public decimal Maximo { get; set; }
        public decimal Minimo { get; set; }
        public string Tabulador { get; set; }
        public override string ToString()
        {
            return $"{this.Minimo:C} - {this.Maximo:C}";
        }
    }
}