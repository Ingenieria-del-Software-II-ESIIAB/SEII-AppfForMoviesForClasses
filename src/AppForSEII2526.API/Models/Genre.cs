namespace AppForSEII2526.API.Models {
    public class Genre {
        public Genre() {
        }

        public Genre(string name) {
            Name = name;
        }

        public int Id { get; set; }
        public string Name { get; set; }
    }
}
