namespace Repository.Model.Consignment
{
    public class CreateConsignmentItemRequestModel
    {
        public string Name { get; set; }
        public string Category { get; set; }
        public string Origin { get; set; }
        public string Sex { get; set; }
        public int Age { get; set; }
        public string Size { get; set; }
        public string Species { get; set; }
        public string? ImageUrl { get; set; }
        public string Personality { get; set; } = "Unknown";
        public string FoodAmount { get; set; } = "Standard";
        public string WaterTemp { get; set; } = "Normal";
        public string MineralContent { get; set; } = "Normal";
        public string PH { get; set; } = "Neutral";
        public string Type { get; set; } = "Default";
    }
}
