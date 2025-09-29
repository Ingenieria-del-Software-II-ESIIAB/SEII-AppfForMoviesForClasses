namespace AppForSEII2526.API.Models
{
    [Index(nameof(Name),IsUnique =true)]
    public class Purchase
    {
        public int Id { get; set; }


        [StringLength(20, 
            ErrorMessage ="At least five names must be provided for the name",
            MinimumLength =5)]
        public string Name { get; set; }

        [StringLength(20,
       ErrorMessage = "At least five names must be provided for the name",
       MinimumLength = 5)]
        public string? Description { get; set; }

        public IList<PurchaseItem> PurchaseItems { get; set; }
    }
}
