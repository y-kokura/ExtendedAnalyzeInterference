using Inventor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExtendedAnalyzeInterference
{
    internal class Globals
    {
        public static Inventor.Application InvApp;
        public static Inventor.Document ActiveInvDoc;

        public const string SimpleAddInClientID = "4571000a-b104-4c29-9f85-04d03e740459";
        public const string AddInClientID = "{" + SimpleAddInClientID + "}";
    }
}
