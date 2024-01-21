using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CRMAdmin.Models
{
    public class Pagination
    {
       
        public int Total { get; set; }
        public int PageNum { get; set; }
        public int PageSize { get; set; }
        public string Url { get; set; }
        public bool DisablePerPage { get; set; }

        public int StartIndex
        {
            get
            {
                return (PageNum - 1) * PageSize + 1;
            }
        }
        public int EndIndex
        {
            get
            {
                int end = PageNum * PageSize;
                if( end > Total)
                {
                    end = Total;
                }
                return end;
            }
        }

        public int TotalPage
        {
            get
            {
                float totalPage = float.Parse(Total.ToString()) / float.Parse(PageSize.ToString());
                return int.Parse(Math.Ceiling(totalPage).ToString());
            }
        }
    }
}