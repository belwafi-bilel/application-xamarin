namespace RealEstate.Models.API
{
    public class RealEstateResponse<TData>
    {
        public static RealEstateResponse<TData> Create(TData data)
        {
            return new RealEstateResponse<TData>()
            {
                Data = data
            };

        }

        public TData Data { get; set; }
    }

    public class RealEstateResponseWithIssue<TData, TIssueData> : RealEstateResponse<TData>
    {
        public static RealEstateResponseWithIssue<TData, TIssueData> Create(TData data, RealEstateResponseStatus<TIssueData> status)
        {
            return new RealEstateResponseWithIssue<TData, TIssueData>()
            {
                Data = data,
                Status = status
            };
        }

        public RealEstateResponseStatus<TIssueData> Status { get; set; }
    }

    public class RealEstateResponseStatus<TIssueData>
    {
        public static RealEstateResponseStatus<TIssueData> Create<TIssueData>(int code, TIssueData issueData = default(TIssueData))
        {
            return new RealEstateResponseStatus<TIssueData>()
            {
                Code = code
            };
        }

        public int Code { get; set; }

        public TIssueData ErrorData { get; set; }
    }


}
