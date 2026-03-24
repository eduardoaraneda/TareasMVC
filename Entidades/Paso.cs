namespace TareasMVC.Entidades
{
    public class Paso
    {
        public Guid Id { get; set; }
        public string TareaId { get; set; }
        public Tarea Tarea { get; set; }
        public string Descripcion { get; set; }
        public bool Completado { get; set; }
        public int Orden { get; set; }
    }
}
