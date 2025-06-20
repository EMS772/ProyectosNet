namespace MoneyMindIA.Models.TransaccionDto
{
    public class TransaccionDto
    {
        public int TransaccionId { get; set; }
        public string Descripcion { get; set; }
        public decimal Monto { get; set; }
        public DateTime Fecha { get; set; }
        public int Tipo { get; set; }
        public string CategoriaNombre { get; set; }
        public string BilleteraNombre { get; set; }
    }
}
