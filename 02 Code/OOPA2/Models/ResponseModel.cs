namespace OOPA2.Models
{
    // ResponseModel class - Represents a successful/failed response with a reason
    public class ResponseModel
    {
        public bool IsSuccess { get; set; }
        public string Reason { get; set; }

        public ResponseModel(bool isSuccess, string reason)
        {
            /*
            Constructor for ResponseModel class

            Parameters:
                isSuccess (bool): Whether the response was successful
                reason (string): The reason for the response
            */
            
            IsSuccess = isSuccess;
            Reason = reason;
        }
    }

}
