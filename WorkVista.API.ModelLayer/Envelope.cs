using System.Data;

namespace WorkVista.API.ModelLayer
{
    public class Envelope
    {
        public bool Status { get; set; }
        public string Message { get; set; }
        public string Data { get; set; }
        public object ObjectData { get; set; }
        public int TotalRecords { get; set; }
        public DataTable dataTable { get; set; }

        public Envelope()
        {

        }
        public Envelope(bool status, string data)
        {
            SetData(status, data, string.Empty, 0);
        }
        public Envelope(bool status, string data, string message)
        {
            SetData(status, data, message, 0);
        }
        public Envelope(bool status, string data, string message, int totalRecords)
        {
            SetData(status, data, message, totalRecords);
        }

        public Envelope(bool status, object objectData, string message, int totalRecords)
        {
            SetData(status, objectData, message, totalRecords);
        }

        public Envelope(bool status, string data, string message, int totalRecords, DataTable dataTable)
        {
            SetData(status, data, message, totalRecords, dataTable);
        }

        public void SetData(bool status, string data, string message, int totalRecords)
        {
            this.Status = status;
            this.Data = data;
            this.Message = message;
            this.TotalRecords = totalRecords;
        }

        public void SetData(bool status, object objectData, string message, int totalRecords)
        {
            this.Status = status;
            this.ObjectData = objectData;
            this.Message = message;
            this.TotalRecords = totalRecords;
        }
        public void SetData(bool status, string data, string message, int totalRecords, DataTable dataTable)
        {
            this.Status = status;
            this.Data = data;
            this.Message = message;
            this.TotalRecords = totalRecords;
            this.dataTable = dataTable;
        }
    }
}
