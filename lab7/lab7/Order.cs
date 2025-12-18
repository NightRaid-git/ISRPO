using System;

namespace lab7
{
    public class Order
    {
        public int id_Order { get; set; }
        public string OrderName { get; set; }
        public int TypeProductID { get; set; }
        public int PublicationID { get; set; }  
        public int OfficeID { get; set; }  
        public int CustomerID { get; set; }
        public DateTime DateOfAdmission { get; set; }
        public DateTime? DateOfCompletion { get; set; }  
        public decimal Price { get; set; }


        public int Quantity { get; set; }
        public decimal TotalAmount { get; set; }
        public string CustomerName { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public int BookId { get; set; }  

        public Order()
        {
            DateOfAdmission = DateTime.Now;
            Quantity = 1;
            BookId = 0;
        }
    }
}