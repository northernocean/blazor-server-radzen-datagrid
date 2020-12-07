namespace DataGridTest.Data
{
    public class Ship
    {
        public int Id { get; set; }
        public string Name { get; set; } = "-";
        public int Launched { get; set; }

        public void Update(Ship ship)
        {
            Name = ship.Name;
            Launched = ship.Launched;
        }

        public Ship Clone()
        {
            return (Ship)MemberwiseClone();
        }
    }
}
