using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CRMAdmin.Models
{
    public class CandidateGuestMerge
    {
        private int _GuestID;
        private bool _SelectedMerge;
        private bool _Default;

        public int GuestID
        {
            get
            {
                return _GuestID;
            }

            set
            {
                _GuestID = value;
            }
        }

        public bool SelectedMerge
        {
            get
            {
                return _SelectedMerge;
            }

            set
            {
                _SelectedMerge = value;
            }
        }

        public bool Default
        {
            get
            {
                return _Default;
            }

            set
            {
                _Default = value;
            }
        }
    }
}