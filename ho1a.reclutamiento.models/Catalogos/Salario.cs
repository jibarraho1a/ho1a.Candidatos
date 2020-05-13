using ho1a.applicationCore.Entities;

namespace ho1a.reclutamiento.models.Catalogos
{
    public class Salario : BaseEntityId
    {
        public decimal? Maximo { get; set; }
        public decimal? Minimo { get; set; }
        public override string ToString()
        {
            var minimo = string.Empty;
            var maximo = string.Empty;

            minimo = this.Minimo != null ? $"{this.Minimo.Value:C} -" : "Menos de ";

            if (this.Maximo == null)
            {
                minimo = $"Más de {this.Minimo:C}";
            }

            maximo = this.Maximo != null ? $"{this.Maximo:C}" : string.Empty;

            return $"{minimo} {maximo}";
        }
    }
}