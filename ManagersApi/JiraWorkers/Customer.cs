namespace ManagersApi.JiraWorkers
{
    public struct Customer
    {
        public Customer(int id, string firstName, string lastName, string imageUrl)
        {
            Id = id;
            FirstName = firstName;
            LastName = lastName;
            ImageUrl = imageUrl;
        }

        public int Id;
        public string FirstName;
        public string LastName;
        public string ImageUrl;
    }
}