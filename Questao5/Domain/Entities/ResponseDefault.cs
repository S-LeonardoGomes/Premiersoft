using Questao5.Domain.Enumerators;

namespace Questao5.Domain.Entities
{
    public class ResponseDefault<T>
    {
        public StatusEnum StatusEnum { get; set; }
        public string DescricaoErro { get; set; }
        public T? Dado { get; set; }
    }
}
